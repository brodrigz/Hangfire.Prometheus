using System;

using Microsoft.AspNetCore.Builder;

using Owin;

using Prometheus;

namespace Hangfire.Prometheus
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

        public static IAppBuilder UsePrometheusHangfireExporter(this IAppBuilder app, HangfirePrometheusSettings settings = null)
        {
            settings = settings ?? new HangfirePrometheusSettings();

            try
            {
                JobStorage js = JobStorage.Current;

                if (js != null)
                {
                    IHangfireMonitorService hangfireMonitor = new HangfireMonitorService(js);
                    IPrometheusExporter exporter = new HangfirePrometheusExporter(hangfireMonitor, settings);
                    Metrics.DefaultRegistry.AddBeforeCollectCallback(() => exporter.ExportHangfireStatistics());
                }
            }
            catch (Exception)
            {
                return app;
            }

            return app;
        }
    }
}