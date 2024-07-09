using Coding.Challenge.Dependencies.DataAccess;
using Coding.Challenge.Dependencies.DataAccess.Interfaces;
using Coding.Challenge.Dependencies.DataAccess.Repositories;
using Coding.Challenge.Dependencies.Database;
using Coding.Challenge.Dependencies.Managers;
using Coding.Challenge.Dependencies.Models;
using Coding.Challenge.Dependencies.Services;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace Coding.Challenge.API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder webApplicationBuilder,IConfiguration configuration)
        {
            var serviceCollection = webApplicationBuilder.Services;

            serviceCollection.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.SerializerOptions.PropertyNamingPolicy = null;
            });
            serviceCollection.AddControllers();
            serviceCollection
                .AddEndpointsApiExplorer();

            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Challenge Api", Version = "v1" });
            });

            serviceCollection
                //.RegisterSlowDatabase()
                .RegisterContentsManager()
                .AddInfrastructure()
                .AddDbContext_SQLServer(configuration);
            return webApplicationBuilder;
        }

        private static IServiceCollection RegisterSlowDatabase(this IServiceCollection services)
        {
            services.AddSingleton<IDatabase<Content, ContentDto>, DatabaseLenta<Content, ContentDto>>();
            services.AddSingleton<IMapper<Content, ContentDto>, ContentMapper>();            
            services.AddSingleton<IDadosMockados<Content>, DadosMockados>();

            return services;
        }

        private static IServiceCollection RegisterContentsManager(this IServiceCollection services)
        {
            services.AddScoped<IContentsManager, ContentsManager>();

            return services;
        }
        private static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IContentWriteOnlyRepository,ContentRepository>();
            services.AddScoped<IContentReadOnlyRepository, ContentRepository>();
            services.AddScoped<IMapper<Content, ContentDto>,ContentMapper>();
            services.AddScoped<IUnityOfWork, UnitOfWork>();


            return services;
        }
        private static IServiceCollection AddDbContext_SQLServer(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ChallengeDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
            });
            return services;
        }
        public static WebApplicationBuilder ConfigureWebHost(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder
                .WebHost
                .ConfigureLogging(logging => { logging.ClearProviders(); });

            return webApplicationBuilder;
        }
       
    }
}
