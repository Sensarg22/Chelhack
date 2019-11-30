namespace Crawler
{
    public class JobOptions
    {
        public bool IsRunImmediately { get; set; }
        /// <summary>
        /// https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/crontriggers.html
        /// </summary>
        public string Schedule { get; set; }
    }
}