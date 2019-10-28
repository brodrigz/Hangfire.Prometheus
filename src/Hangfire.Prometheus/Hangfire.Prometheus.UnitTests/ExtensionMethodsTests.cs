using Hangfire.Prometheus.Core;
using Hangfire.Prometheus.NetCore;
using Hangfire.Prometheus.NetFramework;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;

using Moq;
using Owin;
using Xunit;

namespace Hangfire.Prometheus.UnitTests
{
    public class ExtensionMethodsTests
    {
        [Fact]
        public void UninitializedJobStorage_DoesNotThrow()
        {
            Mock<IApplicationBuilder> appBuilderMock = new Mock<IApplicationBuilder>();
            appBuilderMock.Setup(x => x.ApplicationServices.GetService(typeof(JobStorage)))
                          .Returns(null);

            appBuilderMock.Object.UsePrometheusHangfireExporter(new HangfirePrometheusSettings());
        }

        [Fact]
        public void InitializedJobStorage_DoesNotThrow()
        {
            Mock<IApplicationBuilder> appBuilderMock = new Mock<IApplicationBuilder>();
            appBuilderMock.Setup(x => x.ApplicationServices.GetService(typeof(JobStorage)))
                          .Returns(new Mock<JobStorage>().Object);

            appBuilderMock.Object.UsePrometheusHangfireExporter(new HangfirePrometheusSettings());
        }
    }
}