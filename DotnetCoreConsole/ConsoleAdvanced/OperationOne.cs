using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAdvanced
{
    internal class OperationOne : IOperation
    {
        public async Task Execute(CancellationToken token)
        {
            Console.WriteLine("operation one started");
            await Task.Delay(30000, token);
            Console.WriteLine("operation one completed");
        }
    }
}