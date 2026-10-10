using System.Text;
using Academic360.Api.Services;
using Academic360.Application;
using Academic360.Application.Common.Interfaces;
using Academic360.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
// Agregado para protección contra fuerza bruta
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// 2. Registrar dependencias de Capas (Clean Architecture)
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// 3. Registrar servicios propios de la API
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUsuarioActual, UsuarioActual>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- CORRECCIÓN OWASP A04: Rate Limiting ---
// Previene ataques de denegación de servicio (DoS) y fuerza bruta en el Login.
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            // Agrupa por IP del cliente
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100, // Máximo 100 peticiones por minuto por IP
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// 4. Configurar Autenticación JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKeyValue = jwtSettings["Secret"];

// --- CORRECCIÓN OWASP A05: Fail-Fast sin Secretos Hardcodeados ---
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
    // --- CORRECCIÓN OWASP A02: Exigir HTTPS ---
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
        ClockSkew = TimeSpan.Zero
    };
});

// 5. Configurar CORS (Para permitir solicitudes desde Angular)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        // Se recomienda leer el origen de appsettings para que en Producción no quede expuesto localhost
        var frontendUrl = builder.Configuration["FrontendUrl"] ?? "http://localhost:4200";

        policy.WithOrigins(frontendUrl)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Habilitado por si migras a Cookies HttpOnly para mayor seguridad del JWT
    });
});

// --- CORRECCIÓN OWASP A02: Configurar HSTS para Producción ---
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

// 6. Configurar Middleware Pipeline
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Forzar el uso de políticas de seguridad estricta de transporte en producción
    app.UseHsts();
}

// --- CORRECCIÓN OWASP A05: Cabeceras de Seguridad Básicas ---
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Frame-Options", "DENY"); // Evita que tu API sea embebida en Iframes (Clickjacking)
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff"); // Evita inyecciones de MIME
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self';"); // Mitigación XSS para respuestas API
    await next();
});

// --- CORRECCIÓN OWASP A02: Habilitar redirección HTTPS ---
app.UseHttpsRedirection();

// Aplicar límites de peticiones antes de autenticar
app.UseRateLimiter();

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();