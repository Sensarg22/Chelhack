using System;

namespace Crawler
{
    public interface IJobSchedule
    {
        Type JobType { get; }
        /// <summary>
        /// Docs https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/crontriggers.html
        /// </summary>
        string CronExpression { get; }

        bool IsRunImmediately { get; }
    }
}