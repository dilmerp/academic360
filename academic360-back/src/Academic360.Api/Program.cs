using Academic360.Api.Services;
using Academic360.Application;
using Academic360.Application.Common.Interfaces;
using Academic360.Infrastructure;
using Academic360.Infrastructure.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// --- OWASP: Configuración para Proxys e Infraestructura de Contenedores (Docker/K8s) ---
// Extrae la IP real del cliente si la API está detrás de Nginx, un Ingress Controller o un API Gateway.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // Limpiar las redes conocidas asumiendo confianza en la red interna del clúster
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Registrar dependencias de Capas (Clean Architecture)
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Registrar servicios propios de la API
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUsuarioActual, UsuarioActual>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // Resolver el conflicto de nombres (SchemaId) en los DTOs
    c.CustomSchemaIds(type => type.FullName);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Autorización JWT.\r\n\r\nEscribe tu token directamente abajo (NO necesitas escribir 'Bearer ' antes)."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// --- Registro del Manejador Global de Excepciones ---
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// --- OWASP A04: Rate Limiting ---
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            // RemoteIpAddress devolverá la IP real gracias a UseForwardedHeaders
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100, // Máximo 100 peticiones por minuto por IP real
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// Configurar Autenticación JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKeyValue = jwtSettings["Secret"];

// OWASP A05: Fail-Fast sin Secretos Hardcodeados ---
if (string.IsNullOrWhiteSpace(secretKeyValue))
{
    throw new InvalidOperationException("Seguridad Comprometida: Falta la configuración crítica Jwt:Secret en appsettings o variables de entorno.");
}

var secretKey = Encoding.UTF8.GetBytes(secretKeyValue);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // OWASP A02: Exigir HTTPS ---
    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("Falta Jwt:Issuer"),
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"] ?? throw new InvalidOperationException("Falta Jwt:Audience"),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // Expiración exacta, sin tolerancia por defecto
    };
});

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        var frontendUrl = builder.Configuration["FrontendUrl"] ?? "http://localhost:4200";

        // Advertencia AppSec: Si frontendUrl es "*", AllowCredentials fallará por seguridad del navegador.
        policy.WithOrigins(frontendUrl)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// --- OWASP A02: Configurar HSTS para Producción ---
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHsts(options =>
    {
        options.Preload = true;
        options.IncludeSubDomains = true;
        options.MaxAge = TimeSpan.FromDays(365);
    });
}

var app = builder.Build();


// CONFIGURACIÓN DEL PIPELINE HTTP (MIDDLEWARES)

// 1. Resolver la IP real del cliente (Debe ir primero para que los logs y rate limit lo vean)
app.UseForwardedHeaders();

// 2. Activar Middleware de Manejo de Excepciones
app.UseExceptionHandler();

// 3. Registro de peticiones (Registrará la IP correcta)
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}
else
{
    app.UseHsts();

    // Nota de AppSec: No usamos UseHttpsRedirection() aquí asumiendo que 
    // la infraestructura (Nginx/Azure) maneja el certificado HTTPS (TLS Termination) 
    // y redirige el tráfico interno por HTTP al contenedor Docker. 
    // Habilitar Si expones el contenedor directo a Internet
}

// 4. --- OWASP A05: Cabeceras de Seguridad Básicas (Refactorizado) ---
app.Use(async (context, next) =>
{
    // OnStarting asegurar que las cabeceras se inyecten de forma segura 
    // antes de que cualquier controlador empiece a escribir la respuesta.
    context.Response.OnStarting(() =>
    {
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("Content-Security-Policy", "default-src 'self';");
        return Task.CompletedTask;
    });
    await next();
});

// 5. Aplicar límites de peticiones antes de autenticar para evitar DDoS a endpoints pesados
app.UseRateLimiter();

// 6. CORS
app.UseCors("AllowAngularApp");

// 7. Autenticación y Autorización
app.UseAuthentication();
app.UseAuthorization();

// 8. Mapeo de Controladores
app.MapControllers();

app.Run();