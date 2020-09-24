using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAdvanced
{
    internal interface IOperation
    {
        Task Execute(CancellationToken token);
    }
}