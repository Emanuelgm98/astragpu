param([string]$Configuration = 'Release')
$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$out = Join-Path $root "artifacts\AstraGPU-win-x64"
Write-Output "Publishing AstraGPU to $out"
dotnet publish (Join-Path $root 'src\AstraGPU.App\AstraGPU.App.csproj') -c $Configuration -r win-x64 --self-contained false -o $out
Write-Output "The output is an unpackaged WinUI app. Run AstraGPU.App.exe only on a machine with the Windows App SDK runtime installed."
