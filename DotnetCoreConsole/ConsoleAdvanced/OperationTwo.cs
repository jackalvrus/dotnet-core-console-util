using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAdvanced
{
    internal class OperationTwo : IOperation
    {
        private readonly IOptions<GreetingOptions> options;
        private readonly IConfiguration configuration;

        public OperationTwo(
            IOptions<GreetingOptions> options,
            IConfiguration configuration)
        {
            this.options = options;
            this.configuration = configuration;
        }

        public async Task Execute(CancellationToken token)
        {
            // Use our typed configuration
            Console.WriteLine("operation two says " + options.Value.Intro);

            // Use a command line parameter named "also"
            string also = configuration.GetValue<string>("also");
            if (also != null)
            {
                Console.WriteLine("furthermore, " + also);
            }
        }
    }
}