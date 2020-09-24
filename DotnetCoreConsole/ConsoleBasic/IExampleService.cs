using System.Threading.Tasks;

namespace ConsoleBasic
{
    public interface IExampleService
    {
        Task Say(string quote);
    }
}