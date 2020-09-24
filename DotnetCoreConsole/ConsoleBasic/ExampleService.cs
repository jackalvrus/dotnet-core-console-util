using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ConsoleBasic
{
    /// <summary>
    /// An example service.
    /// </summary>
    public class ExampleService : IExampleService
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// This service also has a dependency graph, such as...
        /// </summary>
        /// <param name="configuration">
        /// A configuration accessor instance, which can work with
        /// standard configuration mechanisms such as appsettings.json
        /// </param>
        public ExampleService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task Say(string quote)
        {
            Console.WriteLine(quote ?? configuration.GetValue<string>("DefaultQuote"));
        }
    }
}
