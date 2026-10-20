namespace AstraGPU.Core;

public sealed record DiagnosticValue(string Name, string Value, string Status, string? Detail = null)
{
    public static DiagnosticValue Unavailable(string name, string? reason = null) => new(name, "No disponible", "unavailable", reason);
}
public sealed record HardwareSnapshot(DateTimeOffset CapturedAt, IReadOnlyList<DiagnosticValue> Values);
public sealed record GameEntry(string Name, string ExecutablePath, DateTimeOffset AddedAt);
public sealed record OptimizationProfile(string Name, string Description, IReadOnlyDictionary<string, string> Settings);
public sealed record BenchmarkSample(DateTimeOffset Timestamp, double? FrameTimeMs, string Source);
public sealed record BenchmarkSummary(string Method, int SampleCount, double? MedianMs, double? P95Ms, double? OnePercentLowFps, string Limitations);
