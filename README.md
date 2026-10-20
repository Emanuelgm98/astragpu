# AstraGPU

**A local-first, open-source Windows companion for hardware visibility, a private game library, conservative session profiles, and honest frame-time analysis.**

> Early MVP scaffold. This is not a universal FPS optimizer. It does not promise performance gains or generate benchmark results. Unsupported measurements are shown as **No disponible**.

## Principles

- Offline-first. No account, payment, forced telemetry, or cloud processing.
- Windows facts are collected locally. The initial diagnostics report OS and processor count; GPU model, VRAM and temperatures explicitly remain unavailable until a dependable built-in source exists.
- The library stores user-selected executable paths in `%LOCALAPPDATA%\AstraGPU`.
- Profiles are descriptive and conservative. No driver installation, overclocking, voltage changes, service edits, antivirus/firewall/anti-cheat changes, or elevation.
- Benchmark calculations consume observed samples only. No game execution or synthetic benchmark values.
- Backup restore validates every destination path against traversal.
- Optional streaming is a future integration point for another PC the user owns or is authorized to use; no server or cloud compute is included.

## Requirements

- Windows 10 17763 or later (Windows 11 recommended)
- .NET 8 SDK
- Visual Studio 2022 with **Windows App SDK / WinUI 3** workload for the desktop app
- Internet only to restore NuGet packages and fetch the repository; runtime MVP functions are local

The app uses WinUI 3 and `Microsoft.WindowsAppSDK`. A Windows SDK and WinUI workload are needed to build it. No system-wide SDK was installed while preparing this MVP. GitHub Actions runs on Windows and provisions .NET 8 through the hosted runner.

## Build

```powershell
dotnet restore AstraGPU.sln
dotnet test tests/AstraGPU.Tests/AstraGPU.Tests.csproj
dotnet build src/AstraGPU.App/AstraGPU.App.csproj -c Release
```

Para preparar una carpeta distribuible sin instalar un MSI, usa `packaging/Publish-Windows.ps1`. La aplicación es WinUI 3 sin empaquetar y requiere el runtime de Windows App SDK correspondiente; no se crea un instalador silencioso ni se modifican componentes del sistema.

The current authoring environment did not have `dotnet`, MSBuild, or CMake available, so no local build or test result is claimed. CI is configured for Windows.

## MVP features

- Local diagnostics with explicit unavailable states
- Add existing `.exe` files to a local library
- Conservative profile catalog (descriptive only at this stage)
- Benchmark summary math for observed frame-time samples; collection adapters remain future work
- Backup/restore service in the Security project
- Exportación JSON de diagnóstico y benchmark con `ReportExporter`; los valores no observados quedan nulos o explícitamente no disponibles
- Streaming status placeholder; no network listener or remote control yet

## Project layout

`Core`, `Hardware`, `Optimization`, `Benchmark`, `Streaming`, `Security`, and `App` are separate projects. `tests/AstraGPU.Tests` covers benchmark math, path traversal, and profile safety.

La biblioteca local acepta únicamente archivos existentes con extensión `.exe` y firma inicial `MZ`; no ejecuta ni inspecciona binarios mediante procesos. La captura de GPU/VRAM usa el inventario del registro de Windows cuando está disponible y conserva estados explícitos de no disponibilidad.

## Identidad visual y acceso directo

La identidad visual reutilizable está en [docs/BRANDING.md](docs/BRANDING.md) y su recurso vectorial en `assets/branding/astragpu-griffin.svg`. Es una interpretación original basada en el sello de referencia recibido del usuario; no usa imágenes externas.

En Windows puede existir un acceso directo de escritorio llamado `AstraGPU - Proyecto.lnk`. Este acceso directo abre la carpeta del proyecto en el Explorador; no ejecuta juegos, binarios de terceros ni una compilación no verificada. Si no existe, el proyecto sigue teniendo como puntos de entrada reales este README y la solución `AstraGPU.sln`.

## Contributing

Read [architecture](docs/ARCHITECTURE.md), [security](docs/SECURITY.md), [benchmark methodology](docs/BENCHMARKING.md), [licensing](docs/LICENSING.md), and [installation](docs/INSTALLATION.md). Keep features local-first and make unsupported values explicit. Never add fabricated benchmark records or hidden machine changes.

## License

MIT. See [LICENSE](LICENSE).
