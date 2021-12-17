using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OzonEdu.MerchandiseApi.Constants;
using OzonEdu.MerchandiseApi.Infrastructure.Filters;
using OzonEdu.MerchandiseApi.Infrastructure.StartupFilters;

namespace OzonEdu.MerchandiseApi.Infrastructure.Extensions
{
    internal static class HostBuilderExtensions
    {
        internal static IHostBuilder AddInfrastructure(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IStartupFilter, TerminalStartupFilter>();
                services.AddSingleton<IStartupFilter, SwaggerStartupFilter>();

                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(RouteConstant.Version,
                        new OpenApiInfo { Title = "OzonEdu.MerchandiseApi", Version = RouteConstant.Version });
                    
                    options.CustomSchemaIds(x => x.FullName);
                    
                    var xmlFileName = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
                    var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                    options.IncludeXmlComments(xmlFilePath);
                });
            }); 
            return builder;
        }
        
        public static IHostBuilder AddHttp(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
            });
            
            return builder;
        }
    }
}