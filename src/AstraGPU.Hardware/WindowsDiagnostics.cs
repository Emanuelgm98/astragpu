using System.Runtime.InteropServices;
using AstraGPU.Core;

namespace AstraGPU.Hardware;

public interface IHardwareDiagnostics { Task<HardwareSnapshot> CaptureAsync(CancellationToken cancellationToken = default); }

/// <summary>Collects OS facts without elevation. Unsupported hardware metrics remain explicitly unavailable.</summary>
public sealed class WindowsDiagnostics : IHardwareDiagnostics
{
    public Task<HardwareSnapshot> CaptureAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var values = new List<DiagnosticValue>
        {
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? new("Sistema", RuntimeInformation.OSDescription, "available")
                : DiagnosticValue.Unavailable("Sistema", "AstraGPU está diseñado para Windows."),
            new("Arquitectura", RuntimeInformation.OSArchitecture.ToString(), "available"),
            new("Procesadores lógicos", Environment.ProcessorCount.ToString(), "available"),
            DiagnosticValue.Unavailable("GPU", "La detección fiable de modelo y sensores requiere una fuente Windows validada."),
            DiagnosticValue.Unavailable("VRAM", "No se consultan sensores de terceros."),
            DiagnosticValue.Unavailable("Temperaturas", "No disponible sin proveedor de sensores compatible.")
        };
        return Task.FromResult(new HardwareSnapshot(DateTimeOffset.UtcNow, values));
    }
}
