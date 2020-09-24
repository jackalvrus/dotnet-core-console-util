using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace ConsoleBasic
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create a standard generic host builder,
            // which includes support for configuration, logging, and DI
            var builder = Host.CreateDefaultBuilder()
                // Configure our DI container, just like ConfigureServices in Startup
                .ConfigureServices(services =>
                {
                    // Register our services
                    services.AddSingleton<IExampleService, ExampleService>();
                    // Enrole the class that implements our operation in DI
                    // so that the container can create and inject its dependencies
                    services.AddTransient<Operation>();
                });
            // Build the host to create the service provider
            var host = builder.Build();
            // Use the service provider to create an instance of our operation implementation
            // with a complete dependency graph based on registered services
            var operation = host.Services.GetService<Operation>();
            // Execute our operation
            await operation.Execute(args);
            // Process terminates after operation executes
        }
    }
}
