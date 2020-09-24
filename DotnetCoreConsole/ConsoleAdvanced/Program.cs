using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace ConsoleAdvanced
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Determine the operation to execute from the command line
            // and get a delegate we will use to register that operation
            var registerOperation = GetOperation(ref args);
            if (registerOperation == null)
            {
                Console.WriteLine("Usage...");
                return;
            }

            // Note that we pass args to CreateDefaultBuilder; that includes it in configuration
            var builder = Host.CreateDefaultBuilder(args)
                // Console lifetime enables Ctrl+C cancellation;
                // it is enabled by default, so this isn't strictly necessary
                .UseConsoleLifetime()
                // Note that our delegate receives both context and servicese
                .ConfigureServices((context, services) =>
                {
                    // The context argument contains configuration and environment
                    services.Configure<GreetingOptions>(context.Configuration.GetSection("Greetings"));
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        // Do something for the dev environment
                    }

                    // Configure services here...

                    // Call the delegate to register our command and hosted service
                    registerOperation(services);
                });
            var host = builder.Build();
            // This time we run the host
            await host.RunAsync();
        }

        /// <summary>
        /// Gets the requested operation from the command line arguments.
        /// </summary>
        /// <param name="args">Command line arguments; modified to remove the operation.</param>
        /// <returns>
        /// A delegate used to register the class that implements the requested operation with the 
        /// DI container. Returns null if the operation is not recognized.
        /// </returns>
        private static Action<IServiceCollection> GetOperation(ref string[] args)
        {
            // We can't do anything if there is no command line;
            // alternatively, implement a default operation here
            if (args.Length == 0)
            {
                return null;
            }

            // Extract the first argument as the requested operation
            string operation = args[0].ToLower();
            args = args[1..];

            // Return operation registration delegate based on operation
            return operation switch
            {
                "one" => services => AddOperation<OperationOne>(services),
                "two" => services => AddOperation<OperationTwo>(services),
                _ => null
            };
        }

        private static void AddOperation<TOperation>(IServiceCollection services)
            where TOperation : class, IOperation
        {
            // Register our operation class so that it can be injected into the hosted service
            services.AddTransient<TOperation>();
            // Register our hosted service, which will execute the operation
            services.AddHostedService<OperationHostedService<TOperation>>();
        }
    }
}
