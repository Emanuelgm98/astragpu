using System.Text.Json;
using AstraGPU.Core;
using AstraGPU.Security;

namespace AstraGPU.Optimization;

public sealed class LocalLibrary
{
    private readonly string _file;
    public LocalLibrary(string dataDirectory) { Directory.CreateDirectory(dataDirectory); _file = Path.Combine(Path.GetFullPath(dataDirectory), "library.json"); }
    public async Task<IReadOnlyList<GameEntry>> GetAsync(CancellationToken ct = default) =>
        File.Exists(_file) ? await JsonSerializer.DeserializeAsync<List<GameEntry>>(File.OpenRead(_file), cancellationToken: ct) ?? [] : [];
    public async Task AddAsync(string name, string executable, CancellationToken ct = default)
    {
        var safe = PathGuard.RequireExistingFile(executable, ".exe");
        await using (var stream = new FileStream(safe, FileMode.Open, FileAccess.Read, FileShare.Read, 2, true))
        {
            var header = new byte[2];
            if (await stream.ReadAsync(header, ct) != 2 || header[0] != (byte)'M' || header[1] != (byte)'Z')
                throw new InvalidDataException("El archivo no tiene una cabecera ejecutable PE válida.");
        }
        var entries = (await GetAsync(ct)).ToList();
        if (entries.Any(x => string.Equals(Path.GetFullPath(x.ExecutablePath), safe, StringComparison.OrdinalIgnoreCase))) return;
        entries.Add(new GameEntry(string.IsNullOrWhiteSpace(name) ? Path.GetFileNameWithoutExtension(safe) : name.Trim(), safe, DateTimeOffset.UtcNow));
        var temp = _file + ".tmp";
        await File.WriteAllTextAsync(temp, JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true }), ct);
        File.Move(temp, _file, true);
    }
    public async Task RemoveAsync(string executable, CancellationToken ct = default)
    {
        var target = Path.GetFullPath(executable);
        var entries = (await GetAsync(ct)).Where(x => !string.Equals(Path.GetFullPath(x.ExecutablePath), target, StringComparison.OrdinalIgnoreCase)).ToList();
        await File.WriteAllTextAsync(_file, JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true }), ct);
    }
}
