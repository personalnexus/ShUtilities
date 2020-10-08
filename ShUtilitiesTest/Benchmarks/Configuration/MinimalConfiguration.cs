using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace ShUtilitiesTest.Benchmarks.Configuration
{
    public class MinimalConfiguration : ManualConfig
    {
        public MinimalConfiguration()
        {
            AddColumnProvider(DefaultColumnProviders.Instance);
            AddJob(Job.Default
                .WithPowerPlan(PowerPlan.HighPerformance)
                .WithPlatform(Platform.X64));
            AddLogger(new SummaryFirstLogger());

        }
    }
}
