namespace AstraGPU.Security;

public static class PathGuard
{
    public static string RequireExistingFile(string path, string requiredExtension)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("La ruta está vacía.", nameof(path));
        var full = Path.GetFullPath(path);
        if (!string.Equals(Path.GetExtension(full), requiredExtension, StringComparison.OrdinalIgnoreCase)) throw new ArgumentException($"Se requiere un archivo {requiredExtension}.", nameof(path));
        if (!File.Exists(full)) throw new FileNotFoundException("El archivo seleccionado no existe.", full);
        return full;
    }
    public static string RequireChildPath(string root, string candidate)
    {
        var fullRoot = Path.GetFullPath(root).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        var full = Path.GetFullPath(candidate);
        if (!full.StartsWith(fullRoot, StringComparison.OrdinalIgnoreCase)) throw new UnauthorizedAccessException("La ruta debe permanecer dentro del directorio elegido.");
        return full;
    }
}
