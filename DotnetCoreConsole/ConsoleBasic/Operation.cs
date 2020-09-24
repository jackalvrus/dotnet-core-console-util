using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleBasic
{
    /// <summary>
    /// This class implements the operation that our utility will perform.
    /// </summary>
    internal class Operation
    {
        private readonly IExampleService service;
        private readonly ILogger<Operation> logger;

        /// <summary>
        /// Our operation relies on a dependency graph, such as...
        /// </summary>
        /// <param name="service">A service of some sort.</param>
        /// <param name="logger">The standard logging provider.</param>
        public Operation(IExampleService service, ILogger<Operation> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// This method executes the operation.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns></returns>
        public async Task Execute(string[] args)
        {
            logger.LogInformation("Executing Operation");
            await service.Say(args.FirstOrDefault());
        }
    }
}
