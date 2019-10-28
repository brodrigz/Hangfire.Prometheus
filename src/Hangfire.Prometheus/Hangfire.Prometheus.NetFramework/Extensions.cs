using System;

using Hangfire.Prometheus.Core;

using Owin;

using Prometheus;

namespace Hangfire.Prometheus.NetFramework
{
    public static class Extensions
    {
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