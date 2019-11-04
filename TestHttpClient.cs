using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TestHttpClient
{
    public class TestHttpClient : ITestHttpClient
    {
        private readonly HttpClient client;
        private readonly ILogger _logger;

        public TestHttpClient(HttpClient _client, ILogger<TestHttpClient> logger)
        {
            client = _client;
            _logger = logger;
        }

        public async Task GetAsync(string threadNumber)
        {
            try
            {
                var stringTask = client.GetStringAsync("https://www.att.com/legal/terms.attWebsiteTermsOfUse.html");
                var msg = await stringTask;
                _logger.LogInformation("{thread}: Finished at {date}", threadNumber, DateTime.Now.ToLongTimeString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{thread}: Failed at {date}", threadNumber, DateTime.Now.ToLongTimeString());
            }
        }

    }
}
