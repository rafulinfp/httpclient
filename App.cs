using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TestHttpClient
{
    public class App
    {
        private readonly ITestHttpClient _dataCollectorService;
        private readonly ILogger<App> _logger;
        private readonly IConfigurationRoot _config;

        public App(ITestHttpClient dataCollector, IConfigurationRoot config, ILogger<App> logger)
        {
            _dataCollectorService = dataCollector;
            _logger = logger;
            _config = config;
        }

        public async Task Run(string threadNumber)
        {
            _logger.LogInformation("{number}: Processing thread", threadNumber);
            await _dataCollectorService.GetAsync(threadNumber);
            _logger.LogInformation("{number}: Ending thread", threadNumber);
        }
    }
}