using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;


namespace NotificationService
{
    static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                //.UseMicrosoftLogFactoryLogging()
                .UseNServiceBus(ctx =>
                {
                   
                     
                    Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
                    IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json",true, true)
                        .AddEnvironmentVariables()
                        .Build();
                   
                    // TODO: consider moving common endpoint configuration into a shared project
                    // for use by all endpoints in the system
 
                    // TODO: give the endpoint an appropriate name
                    var endpointConfiguration = new EndpointConfiguration("NotificationService");
                   // endpointConfiguration
                    

                    // TODO: ensure the most appropriate serializer is chosen
                    // https://docs.particular.net/nservicebus/serialization/
                    //endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

                    endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

                    // Configuration transport
                    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                    transport.UseConventionalRoutingTopology();
                    endpointConfiguration.EnableInstallers();
                    
                   
                    transport.ConnectionString(configuration.GetConnectionString("RabbitMQ"));
        

                    // TODO: remove this condition after choosing a transport, persistence and deployment method suitable for production
                    if (Environment.UserInteractive && Debugger.IsAttached && false)
                    {
                        // TODO: choose a durable transport for production
                        // https://docs.particular.net/transports/
                        var transportExtensions = endpointConfiguration.UseTransport<LearningTransport>();

                        // TODO: choose a durable persistence for production
                        // https://docs.particular.net/persistence/
                        endpointConfiguration.UsePersistence<LearningPersistence>();

                        // TODO: create a script for deployment to production
                        endpointConfiguration.EnableInstallers();
                    }

                    // TODO: replace the license.xml file with your license file

                    return endpointConfiguration;
                });
        }

        static async Task OnCriticalError(ICriticalErrorContext context)
        {
            // TODO: decide if stopping the endpoint and exiting the process is the best response to a critical error
            // https://docs.particular.net/nservicebus/hosting/critical-errors
            try
            {
                await context.Stop();
            }
            finally
            {
                FailFast($"Critical error, shutting down: {context.Error}", context.Exception);
            }
        }

        static void FailFast(string message, Exception exception)
        {
            try
            {
                // TODO: decide what kind of last resort logging is necessary
                // TODO: when using an external logging framework it is important to flush any pending entries prior to calling FailFast
                // https://docs.particular.net/nservicebus/hosting/critical-errors#when-to-override-the-default-critical-error-action
            }
            finally
            {
                Environment.FailFast(message, exception);
            }
        }
    }
}