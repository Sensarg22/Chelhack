using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Crawler
{
    public class ScopedJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public ScopedJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var scope = _serviceProvider.CreateScope();

            var job = (IJob)scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType);

            return new ScopedJob(scope, job);
        }

        public void ReturnJob(IJob job)
        {
            using (job as IDisposable)
            {
                //ignore 
            }
        }

        private class ScopedJob : IJob, IDisposable
        {
            private readonly IServiceScope _scope;
            private readonly IJob _innerJob;

            public ScopedJob(IServiceScope scope, IJob innerJob)
            {
                _scope = scope;
                _innerJob = innerJob;
            }

            public Task Execute(IJobExecutionContext context) => _innerJob.Execute(context);


            public void Dispose()
            {
                using (_scope)
                using (_innerJob as IDisposable)
                {
                   //ignore 
                }
            }
        }
    }
}