using Hangfire.Prometheus.Core;

using Microsoft.AspNetCore.Builder;

using Prometheus;

namespace Hangfire.Prometheus.NetCore
{
    public static class Extensions
    {
        /// <summary>
        /// Initializes Prometheus Hangfire Exporter using current Hangfire job storage and default metrics registry.
        /// </summary>
        /// <param name="app">IApplicationBuilder instance</param>
        /// <returns>Provided instance of IApplicationBuilder</returns>
        public static IApplicationBuilder UsePrometheusHangfireExporter(this IApplicationBuilder app, HangfirePrometheusSettings settings = null)
        {
            settings = settings ?? new HangfirePrometheusSettings();

            JobStorage js = (JobStorage)app.ApplicationServices.GetService(typeof(JobStorage));
            if (js != null)
            {
                IHangfireMonitorService hangfireMonitor = new HangfireMonitorService(js);
                IPrometheusExporter exporter = new HangfirePrometheusExporter(hangfireMonitor, settings);
                Metrics.DefaultRegistry.AddBeforeCollectCallback(() => exporter.ExportHangfireStatistics());
            }

            return app;
        }
    }
}