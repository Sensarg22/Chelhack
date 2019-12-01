using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;

namespace Crawler
{
    public class JobHost: IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<IJobSchedule> _jobSchedules;
        private readonly IConfiguration _configuration;

        public JobHost(ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IEnumerable<IJobSchedule> jobSchedules,
            IConfiguration configuration)
        {
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _jobSchedules = jobSchedules;
            _configuration = configuration;
        }

        public IScheduler Scheduler { get; set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
                Scheduler.JobFactory = _jobFactory;

                var triggerNowJobs = new List<JobKey>();

                foreach (var jobSchedule in _jobSchedules)
                {
                    if (!string.IsNullOrEmpty(_configuration["j"]))
                    {
                        if (jobSchedule.JobType.Name == _configuration["j"] + "Job")
                        {
                            var job = CreateJob(jobSchedule);
                            var trigger = CreateTrigger(jobSchedule);
                            await Scheduler.ScheduleJob(job, trigger, cancellationToken);
                            triggerNowJobs.Add(job.Key);
                        }
                    }
                    else
                    {
                        var job = CreateJob(jobSchedule);
                        var trigger = CreateTrigger(jobSchedule);
                        await Scheduler.ScheduleJob(job, trigger, cancellationToken);
                        if (jobSchedule.IsRunImmediately)
                        {
                            triggerNowJobs.Add(job.Key);
                        }
                    }
                }

                await Scheduler.Start(cancellationToken);

                foreach (var triggerNowJobKey in triggerNowJobs)
                {
                    await Scheduler.TriggerJob(triggerNowJobKey, cancellationToken);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
        }

        private static IJobDetail CreateJob(IJobSchedule schedule)
        {
            var jobType = schedule.JobType;
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName)
                .WithDescription(jobType.Name)
                .Build();
        }

        private static ITrigger CreateTrigger(IJobSchedule schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.JobType.FullName}.trigger")
                .WithCronSchedule(schedule.CronExpression)
                .WithDescription(schedule.CronExpression)
                .Build();
        }
    }
}