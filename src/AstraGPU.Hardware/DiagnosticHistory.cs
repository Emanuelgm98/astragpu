using System.Text.Json;
using AstraGPU.Core;
namespace AstraGPU.Hardware;
/// Persists only user-requested diagnostic snapshots locally; it never sends telemetry.
public sealed class DiagnosticHistory
{
    private const int MaxEntries = 50;
    private readonly string _file;
    public DiagnosticHistory(string dataDirectory) { Directory.CreateDirectory(dataDirectory); _file = Path.Combine(Path.GetFullPath(dataDirectory), "diagnostics-history.json"); }
    public async Task<IReadOnlyList<HardwareSnapshot>> GetAsync(CancellationToken ct = default)
    {
        if (!File.Exists(_file)) return [];
        await using var stream = File.OpenRead(_file);
        return await JsonSerializer.DeserializeAsync<List<HardwareSnapshot>>(stream, cancellationToken: ct) ?? [];
    }
    public async Task AppendAsync(HardwareSnapshot snapshot, CancellationToken ct = default)
    {
        var entries = (await GetAsync(ct)).Append(snapshot).TakeLast(MaxEntries).ToList(); var temp = _file + ".tmp";
        try { await File.WriteAllTextAsync(temp, JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true }), ct); File.Move(temp, _file, true); }
        finally { if (File.Exists(temp)) File.Delete(temp); }
    }
}
