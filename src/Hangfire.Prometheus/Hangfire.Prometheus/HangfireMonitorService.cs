﻿using Hangfire.Storage.Monitoring;

namespace Hangfire.Prometheus.Core
{
    public class HangfireMonitorService : IHangfireMonitorService
    {
        private const string retrySetName = "retries";
        private JobStorage _hangfireJobStorage;

        public HangfireMonitorService(JobStorage hangfireJobStorage)
        {
            _hangfireJobStorage = hangfireJobStorage;
        }
        
        public HangfireJobStatistics GetJobStatistics()
        {
            StatisticsDto hangfireStats = _hangfireJobStorage.GetMonitoringApi().GetStatistics();
            long retryJobs = _hangfireJobStorage.GetConnection().GetAllItemsFromSet(retrySetName).Count;

            return new HangfireJobStatistics
            {
                Failed = hangfireStats.Failed,
                Enqueued = hangfireStats.Enqueued,
                Scheduled = hangfireStats.Scheduled,
                Processing = hangfireStats.Processing,
                Succeeded = hangfireStats.Succeeded,
                Retry = retryJobs
            };
        
        }
    }
}