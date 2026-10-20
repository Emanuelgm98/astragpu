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

The current authoring environment did not have `dotnet`, MSBuild, or CMake available, so no local build or test result is claimed. CI is configured for Windows.

## MVP features

- Local diagnostics with explicit unavailable states
- Add existing `.exe` files to a local library
- Conservative profile catalog (descriptive only at this stage)
- Benchmark summary math for observed frame-time samples; collection adapters remain future work
- Backup/restore service in the Security project
- Streaming status placeholder; no network listener or remote control yet

## Project layout

`Core`, `Hardware`, `Optimization`, `Benchmark`, `Streaming`, `Security`, and `App` are separate projects. `tests/AstraGPU.Tests` covers benchmark math, path traversal, and profile safety.

## Contributing

Read [architecture](docs/ARCHITECTURE.md), [security](docs/SECURITY.md), [benchmark methodology](docs/BENCHMARKING.md), [licensing](docs/LICENSING.md), and [installation](docs/INSTALLATION.md). Keep features local-first and make unsupported values explicit. Never add fabricated benchmark records or hidden machine changes.

## License

MIT. See [LICENSE](LICENSE).
