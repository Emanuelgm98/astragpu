using System.IO.Compression;

namespace AstraGPU.Security;

public sealed class BackupService
{
    public async Task CreateAsync(string sourceDirectory, string destinationZip, CancellationToken ct = default)
    {
        var source = Path.GetFullPath(sourceDirectory);
        if (!Directory.Exists(source)) throw new DirectoryNotFoundException(source);
        var destination = Path.GetFullPath(destinationZip);
        if (!string.Equals(Path.GetExtension(destination), ".zip", StringComparison.OrdinalIgnoreCase)) throw new ArgumentException("El respaldo debe ser ZIP.", nameof(destinationZip));
        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        var temp = destination + ".tmp";
        try { await Task.Run(() => ZipFile.CreateFromDirectory(source, temp, CompressionLevel.Optimal, false), ct); File.Move(temp, destination, true); }
        finally { if (File.Exists(temp)) File.Delete(temp); }
    }
    public async Task RestoreAsync(string backupZip, string destinationDirectory, CancellationToken ct = default)
    {
        var zip = PathGuard.RequireExistingFile(backupZip, ".zip");
        var destination = Path.GetFullPath(destinationDirectory);
        Directory.CreateDirectory(destination);
        using var archive = ZipFile.OpenRead(zip);
        foreach (var entry in archive.Entries)
        {
            ct.ThrowIfCancellationRequested();
            if (string.IsNullOrEmpty(entry.Name)) continue;
            var target = PathGuard.RequireChildPath(destination, Path.Combine(destination, entry.FullName));
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            await using var input = entry.Open(); await using var output = new FileStream(target, FileMode.Create, FileAccess.Write, FileShare.None);
            await input.CopyToAsync(output, ct);
        }
    }
}
