using AstraGPU.Benchmark;
using AstraGPU.Core;
using AstraGPU.Optimization;
using AstraGPU.Security;
using Xunit;

namespace AstraGPU.Tests;
public class MvpTests
{
    [Fact] public void EmptyBenchmarkContainsNoInventedNumbers() { var result = new BenchmarkAnalyzer().Analyze([]); Assert.Equal(0, result.SampleCount); Assert.Null(result.MedianMs); Assert.Null(result.P95Ms); }
    [Fact] public void PercentilesComeFromObservedSamples() { var result = new BenchmarkAnalyzer().Analyze(Enumerable.Range(1, 100).Select(i => new BenchmarkSample(DateTimeOffset.UtcNow, (double)i, "fixture"))); Assert.Equal(50, result.MedianMs); Assert.Equal(95, result.P95Ms); Assert.NotNull(result.OnePercentLowFps); Assert.InRange(Math.Abs(result.OnePercentLowFps!.Value - 1000d / 99), 0, 0.000001); }
    [Fact] public void PathTraversalIsRejected() { var root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()); Assert.Throws<UnauthorizedAccessException>(() => PathGuard.RequireChildPath(root, Path.Combine(root, "..", "escape.txt"))); }
    [Fact] public void ProfilesAreConservativeAndDoNotChangeServices() { Assert.All(new ProfileCatalog().GetProfiles(), p => Assert.DoesNotContain(p.Settings.Keys, k => k.Contains("service", StringComparison.OrdinalIgnoreCase))); }
    [Fact] public async Task LibraryRejectsNonPeExecutables() { var root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()); Directory.CreateDirectory(root); var file = Path.Combine(root, "fake.exe"); await File.WriteAllTextAsync(file, "not a PE"); await Assert.ThrowsAsync<InvalidDataException>(() => new LocalLibrary(root).AddAsync("fake", file)); Directory.Delete(root, true); }
}
