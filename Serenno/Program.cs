using System;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serenno.Bot;
using Serenno.Bot.Infrastructure;
using Serenno.Domain;
using Serenno.Domain.Models;
using Serenno.Services;
using Serilog;

namespace Serenno
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();
            
            Log.Information("Starting application");

            var genericHost = CreateHostBuilder(args).Build();

            try
            {
                await genericHost.RunAsync();
            }
            catch (Exception ex)
            {
                Log.ForContext<Program>()
                    .Fatal(ex, "Host terminated unexpectedly");

                return ex.HResult;
            }
            finally
            {
                Log.CloseAndFlush();
            }

            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile("connections.json")
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<SerennoContext>(builder =>
                        builder.UseSqlServer(
                            hostContext.Configuration.GetValue<string>(nameof(SerennoConfig.DbConnection))));

                    services
                        .AddLazyCache()
                        .AddSerennoServices(hostContext.Configuration)
                        .AddMediatR(typeof(SerennoBot).Assembly, typeof(ServiceResponse).Assembly);
                    
                    services
                        .AddHostedService<SerennoBot>();
                })
                .UseSerilog((context, provider, configuration) =>
                {
                    configuration
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext();
                    
                    var sentry = context.Configuration.GetValue<string>(nameof(SerennoConfig.SentryIOToken));
                    if (!string.IsNullOrWhiteSpace(sentry))
                    {
                        configuration
                            .WriteTo.Sentry(sentry)
                            .MinimumLevel.Warning();
                    }
                });
    }
}