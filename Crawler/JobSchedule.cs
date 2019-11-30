using System;
using Quartz;

namespace Crawler
{
    public class JobSchedule<TJob>: IJobSchedule where TJob : IJob
    {
        public JobSchedule(string cronExpression,bool isRunImmediately = false)
        {
            JobType = typeof(TJob);
            CronExpression = cronExpression;
            IsRunImmediately = isRunImmediately;
        }


        public Type JobType { get; }
        public string CronExpression { get; }
        public bool IsRunImmediately { get; }
    }
}