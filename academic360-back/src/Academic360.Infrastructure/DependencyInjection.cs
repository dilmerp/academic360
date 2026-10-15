using Academic360.Application.Actores.Interfaces;
using Academic360.Application.Alumnos.Interfaces;
using Academic360.Application.Auth.Interfaces;
using Academic360.Application.Catalogos.Interfaces;
using Academic360.Application.Common.Interfaces;
using Academic360.Application.MatriculasCursosLibres.Interfaces;
using Academic360.Infrastructure.Data;
using Academic360.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

namespace Academic360.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

        // Proveer una instancia transitoria de IDbConnection para las consultas de solo lectura
        services.AddTransient<IDbConnection>(sp => new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAlumnoRepository, AlumnoRepository>();
        services.AddScoped<IActorFotoRepository, ActorFotoRepository>();
        services.AddScoped<IAlumnoMatriculaRepository, AlumnoMatriculaRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ICatalogoRepository, CatalogoRepository>();
        services.AddScoped<IMatriculaCursoLibreRepository, MatriculaCursoLibreRepository>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "Academic360_";
        });

        return services;
    }
}


//using Academic360.Application.Actores.Interfaces;
//using Academic360.Application.Alumnos.Interfaces;
//using Academic360.Application.Auth.Interfaces;
//using Academic360.Application.Catalogos.Interfaces;
//using Academic360.Application.Common.Interfaces;
//using Academic360.Infrastructure.Data;
//using Academic360.Infrastructure.Repositories;
//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using System.Data;



//namespace Academic360.Infrastructure;

//public static class DependencyInjection
//{
//    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
//    {
//        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

//        // Proveer una instancia transitoria de IDbConnection para las consultas de solo lectura
//        services.AddTransient<IDbConnection>(sp => new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

//        services.AddScoped<IAlumnoRepository, AlumnoRepository>();
//        services.AddScoped<IActorFotoRepository, ActorFotoRepository>();
//        services.AddScoped<IAlumnoMatriculaRepository, AlumnoMatriculaRepository>();
//        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
//        services.AddScoped<ICatalogoRepository, CatalogoRepository>();


//        services.AddStackExchangeRedisCache(options =>
//        {
//            options.Configuration = configuration.GetConnectionString("RedisConnection");
//            options.InstanceName = "Academic360_";
//        });

//        return services;
//    }
//}