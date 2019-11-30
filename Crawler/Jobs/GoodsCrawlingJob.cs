using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace Crawler.Jobs
{
    [DisallowConcurrentExecution]
    public class GoodsCrawlingJob : IJob
    {
        public GoodsCrawlingJob()
        {
            
        }
        
        public Task Execute(IJobExecutionContext context)
        {
            
            return Task.CompletedTask;
        }
    }
}