using System.Text.Json;
using AstraGPU.Core;
namespace AstraGPU.Benchmark;
public static class ReportExporter
{
 public static async Task ExportAsync(string path,HardwareSnapshot? diagnostics,BenchmarkSummary benchmark,CancellationToken ct=default)
 {
  var full=Path.GetFullPath(path); if(!string.Equals(Path.GetExtension(full),".json",StringComparison.OrdinalIgnoreCase)) throw new ArgumentException("El informe debe ser JSON.",nameof(path));
  Directory.CreateDirectory(Path.GetDirectoryName(full)!); var report=new { exportedAt=DateTimeOffset.UtcNow,diagnostics,benchmark,note="Solo datos observados; los campos no disponibles no se rellenan."};
  await File.WriteAllTextAsync(full,JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}),ct);
 }
}
