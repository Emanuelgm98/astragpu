# Installation and development

This MVP has no signed installer or release package yet. To develop, install Visual Studio 2022 with the WinUI 3 / Windows App SDK workload and .NET 8 SDK, clone the repository, then follow the README build commands. The first restore downloads pinned Windows App SDK and test packages from NuGet. Running the resulting app requires Windows 10 build 17763 or newer.

Do not run the app as administrator. The library picker is user initiated and selected `.exe` paths are saved locally. Backup/restore APIs are available to the backend but are not yet wired to the UI.
