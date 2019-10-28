using Hangfire.Prometheus.Core;
using Hangfire.Prometheus.NetCore;
using Hangfire.Prometheus.NetFramework;
using Hangfire.SqlServer;

using Moq;

using Owin;

using Xunit;

namespace Hangfire.Prometheus.UnitTests
{
    public class NetFrameworkExtensionMethodsTests
    {
        [Fact]
        public void Owin_InitializedJobStorage_DoesNotThrow()
        {
            Mock<IAppBuilder> appBuilderMock = new Mock<IAppBuilder>();
            var sqlStorage = new SqlServerStorage("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

            JobStorage.Current = sqlStorage;

            appBuilderMock.Object.UsePrometheusHangfireExporter(new HangfirePrometheusSettings());
        }

        [Fact]
        public void Owin_UninitializedJobStorage_DoesNotThrow()
        {
            Mock<IAppBuilder> appBuilderMock = new Mock<IAppBuilder>();

            appBuilderMock.Object.UsePrometheusHangfireExporter(new HangfirePrometheusSettings());
        }
    }
}