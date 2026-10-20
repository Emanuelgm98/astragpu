using System.Runtime.InteropServices;
using Microsoft.Win32;
using AstraGPU.Core;
namespace AstraGPU.Hardware;
public interface IHardwareDiagnostics { Task<HardwareSnapshot> CaptureAsync(CancellationToken cancellationToken = default); }
public sealed class WindowsDiagnostics : IHardwareDiagnostics
{
 public Task<HardwareSnapshot> CaptureAsync(CancellationToken cancellationToken = default)
 {
  cancellationToken.ThrowIfCancellationRequested();
  var values = new List<DiagnosticValue>{ RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new("Sistema", RuntimeInformation.OSDescription,"available") : DiagnosticValue.Unavailable("Sistema","Diseñado para Windows."), new("Arquitectura",RuntimeInformation.OSArchitecture.ToString(),"available"), new("Procesadores lógicos",Environment.ProcessorCount.ToString(),"available"), ReadValue("GPU","HardwareInformation.AdapterString","No se encontró un adaptador en el registro de Windows."), ReadValue("VRAM","HardwareInformation.qwMemorySize","Windows no expone VRAM de forma uniforme."), DiagnosticValue.Unavailable("Temperaturas","No disponible sin proveedor de sensores compatible.")};
  return Task.FromResult(new HardwareSnapshot(DateTimeOffset.UtcNow,values));
 }
 private static DiagnosticValue ReadValue(string name,string valueName,string reason)
 {
  if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return DiagnosticValue.Unavailable(name,reason);
  try { using var root=Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Video"); foreach(var n in root?.GetSubKeyNames() ?? []) { using var child=root!.OpenSubKey(n); var raw=child?.GetValue(valueName); if(raw is long bytes) return new(name,$"{bytes/(1024d*1024*1024):0.##} GiB","available","Fuente: registro de Windows."); if(raw is string text && !string.IsNullOrWhiteSpace(text)) return new(name,text,"available","Fuente: registro de Windows."); } } catch(Exception ex) when(ex is SecurityException or UnauthorizedAccessException or IOException) { }
  return DiagnosticValue.Unavailable(name,reason);
 }
}
