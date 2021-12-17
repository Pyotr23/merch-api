using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OzonEdu.MerchandiseApi;
using OzonEdu.MerchandiseApi.Infrastructure.Extensions;
using Serilog;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSerilog((context, configuration) => configuration
            .ReadFrom
            .Configuration(context.Configuration)
            .WriteTo
            .Console())
        .ConfigureWebHostDefaults(wb => wb.UseStartup<Startup>())
        .AddInfrastructure()
        .AddHttp();