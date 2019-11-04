using System.Threading.Tasks;
namespace TestHttpClient
{
    public interface ITestHttpClient
    {
        Task GetAsync(string threadNumber);
    }
}