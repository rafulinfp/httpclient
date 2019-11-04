using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TestHttpClient
{
    internal class Program
    {
        public static IConfigurationRoot configuration;

        private static void Main(string[] args)
        {
            // Start!
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            // Create service collection
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            int threads = 1;
            int.TryParse(configuration["totalConcurrenThreads"], out threads);
            var sources = new List<string>();
            for (int i = 1; i <= threads; i++)
            {
                sources.Add(i.ToString());
            }

            // Run all tasks
            await Task.WhenAll(sources.Select(i => serviceProvider.GetService<App>().Run(i)).ToArray());
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add logging
            serviceCollection.AddLogging(loggingBuilder =>
            {
                // loggingBuilder.AddConsole();
                loggingBuilder.AddSerilog();
                loggingBuilder.AddDebug();
            });

            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Initialize serilog logger
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                 .MinimumLevel.Debug()
                 .Enrich.FromLogContext()
                 .CreateLogger();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton(configuration);

            // Add services
            serviceCollection.AddHttpClient<ITestHttpClient, TestHttpClient>();

            // Add app
            serviceCollection.AddSingleton<App>();
        }
    }
}
