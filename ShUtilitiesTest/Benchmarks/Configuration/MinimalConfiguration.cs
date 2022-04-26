using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Toolchains.DotNetCli;

namespace ShUtilitiesTest.Benchmarks.Configuration
{
    public class MinimalConfiguration : ManualConfig
    {
        public MinimalConfiguration()
        {
            AddColumnProvider(DefaultColumnProviders.Instance);
            AddJob(Job.Default
                .WithToolchain(CsProjCoreToolchain.From(NetCoreAppSettings
                    .NetCoreApp60
                    .WithCustomDotNetCliPath(@"C:\Program Files\dotnet\dotnet.exe")))
                .WithPowerPlan(PowerPlan.HighPerformance)
                .WithPlatform(Platform.X64));
            AddLogger(new SummaryFirstLogger());
        }
    }
}
