using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Quartz;

namespace Crawler.Jobs
{
    [DisallowConcurrentExecution]
    public class GoodsCrawlingJob : IJob
    {
        private HttpClient _httpClient;

        public GoodsCrawlingJob(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(Constants.GoodsHttpClientName);
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var result = await _httpClient.GetAsync(string.Empty);
                result.EnsureSuccessStatusCode();
                var rawResult = await result.Content.ReadAsStringAsync();
                var goods = JsonConvert.DeserializeObject<RawGoodResult>(rawResult, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}