using System;
using System.IO;
using System.Threading.Tasks;
using Crawler.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Crawler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("appsettings.json", optional: false);
                    configHost.AddCommandLine(args);
                    configHost.AddEnvironmentVariables();

                })
                .ConfigureAppConfiguration((hostContext, configHost) =>
                {
                    configHost.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                })
                .ConfigureServices(ConfigureServices)
                .UseConsoleLifetime()
                .ConfigureLogging(builder => builder
                    .AddConsole().AddDebug())
                .Build();
            
            var environment = host.Services.GetService<IHostingEnvironment>();
            Console.WriteLine(environment.EnvironmentName);
            
            await host.RunAsync();
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, ScopedJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            AddJobs(hostContext.Configuration, services);

            services.AddHostedService<JobHost>();
            
            services.AddHttpClient(Constants.GoodsHttpClientName, (provider, client) =>
            {
                var configuration = provider.GetService<IConfiguration>();
                client.BaseAddress = new Uri(configuration["GoodsApi"]);
            });

            services.AddTransient(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var connectionString = new ConnectionString(configuration.GetConnectionString(Constants.GoodsDatabase));
                return new MongoClient(connectionString.ToString()).GetDatabase(connectionString.DatabaseName);
            });
        }

        private static void AddJobs(IConfiguration configuration, IServiceCollection services)
        {
            var jobsOptions = configuration.GetSection("jobs").Get<JobsOptions>();

            if (jobsOptions.GoodsCrawlingJob != null)
            {
                services.AddTransient<GoodsCrawlingJob>();
                services.AddSingleton<IJobSchedule>(new JobSchedule<GoodsCrawlingJob>(
                    jobsOptions.GoodsCrawlingJob.Schedule, jobsOptions.GoodsCrawlingJob.IsRunImmediately));
            }
        }
    }
}