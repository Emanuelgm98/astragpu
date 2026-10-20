using AstraGPU.Benchmark;
using AstraGPU.Core;
using AstraGPU.Hardware;
using AstraGPU.Optimization;
using AstraGPU.Security;
using AstraGPU.Streaming;
using Microsoft.UI.Xaml;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace AstraGPU.App;

public sealed partial class MainWindow : Window
{
    private readonly IHardwareDiagnostics _hardware = new WindowsDiagnostics();
    private readonly string _dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AstraGPU");
    private readonly BackupService _backup = new();
    private HardwareSnapshot? _lastDiagnostics;
    private string _page = "dashboard";
    public MainWindow() { InitializeComponent(); }
    private void Show(string title, string body, string action, string page) { Heading.Text = title; ContentTitle.Text = title; Content.Text = body; ActionButton.Content = action; _page = page; Status.Text = ""; }
    private void OnDashboard(object s, RoutedEventArgs e) => Show("Optimización local, con evidencia", "Diagnostica el equipo, organiza tus juegos y exporta datos locales. No se aplican cambios ocultos.", "Ejecutar diagnóstico local", "dashboard");
    private void OnDiagnostics(object s, RoutedEventArgs e) => Show("Diagnóstico", "Fuentes: APIs/runtime de Windows y registro del sistema. Sensores no compatibles aparecen como No disponible.", "Actualizar diagnóstico", "diagnostics");
    private void OnLibrary(object s, RoutedEventArgs e) => Show("Biblioteca local", "Añade manualmente ejecutables .exe existentes. Se valida la cabecera PE y no se ejecutan.", "Añadir juego…", "library");
    private void OnProfiles(object s, RoutedEventArgs e) => Show("Perfiles conservadores", string.Join("\n\n", new ProfileCatalog().GetProfiles().Select(p => $"{p.Name}: {p.Description}")), "Revisar perfil", "profiles");
    private void OnBenchmark(object s, RoutedEventArgs e) => Show("Benchmark observado", "No hay captura automática ni resultados inventados. Importa muestras reales mediante una futura fuente autorizada; este MVP solo analiza datos observados.", "Exportar informe", "benchmark");
    private void OnBackup(object s, RoutedEventArgs e) => Show("Backup local", "Crea un ZIP reversible de la configuración local. La restauración requiere confirmación y valida rutas.", "Crear backup", "backup");
    private void OnRestore(object s, RoutedEventArgs e) => Show("Restauración", "Selecciona un ZIP creado por AstraGPU. Se restaura solo dentro de la carpeta local de datos.", "Seleccionar backup", "restore");
    private void OnStreaming(object s, RoutedEventArgs e) => Show("Streaming opcional", StreamingStatus.NotConfigured.Message, "Estado", "streaming");
    private async void OnAction(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_page is "diagnostics" or "dashboard")
            {
                _lastDiagnostics = await _hardware.CaptureAsync();
                Content.Text = string.Join("\n", _lastDiagnostics.Values.Select(v => $"{v.Name}: {v.Value} [{v.Status}] — {v.Detail ?? "Fuente local"}"));
                Status.Text = $"Capturado {_lastDiagnostics.CapturedAt.ToLocalTime():g}. Sin elevación.";
            }
            else if (_page == "library") await AddLibraryEntryAsync();
            else if (_page == "backup") await CreateBackupAsync();
            else if (_page == "restore") await RestoreBackupAsync();
            else if (_page == "benchmark") await ExportReportAsync();
            else if (_page == "streaming") Status.Text = StreamingStatus.NotConfigured.Message;
            else Status.Text = "Este perfil es descriptivo y no modifica servicios, políticas, drivers, seguridad ni hardware.";
        }
        catch (Exception ex) { Status.Text = $"No se pudo completar la operación: {ex.Message}"; }
    }
    private async Task AddLibraryEntryAsync()
    {
        var picker = new FileOpenPicker { SuggestedStartLocation = PickerLocationId.ComputerFolder }; picker.FileTypeFilter.Add(".exe"); InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(this));
        var file = await picker.PickSingleFileAsync(); if (file is null) return;
        await new LocalLibrary(_dataDirectory).AddAsync(Path.GetFileNameWithoutExtension(file.Name), file.Path);
        var games = await new LocalLibrary(_dataDirectory).GetAsync(); Content.Text = string.Join("\n", games.Select(g => $"{g.Name} — {g.ExecutablePath}")); Status.Text = "Guardado localmente; no se ejecutó el archivo.";
    }
    private async Task CreateBackupAsync()
    {
        var picker = new FileSavePicker { SuggestedStartLocation = PickerLocationId.DocumentsLibrary, SuggestedFileName = $"AstraGPU-backup-{DateTime.Now:yyyyMMdd-HHmmss}.zip" }; picker.FileTypeChoices.Add("ZIP", new[] { ".zip" }); InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(this));
        var file = await picker.PickSaveFileAsync(); if (file is null) return; await _backup.CreateAsync(_dataDirectory, file.Path); Status.Text = "Backup creado. No se modificó el sistema.";
    }
    private async Task RestoreBackupAsync()
    {
        var picker = new FileOpenPicker { SuggestedStartLocation = PickerLocationId.DocumentsLibrary }; picker.FileTypeFilter.Add(".zip"); InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(this));
        var file = await picker.PickSingleFileAsync(); if (file is null) return; await _backup.RestoreAsync(file.Path, _dataDirectory); Status.Text = "Backup restaurado dentro de los datos locales.";
    }
    private async Task ExportReportAsync()
    {
        var picker = new FileSavePicker { SuggestedStartLocation = PickerLocationId.DocumentsLibrary, SuggestedFileName = $"AstraGPU-report-{DateTime.Now:yyyyMMdd-HHmmss}.json" }; picker.FileTypeChoices.Add("JSON", new[] { ".json" }); InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(this));
        var file = await picker.PickSaveFileAsync(); if (file is null) return; var summary = new BenchmarkAnalyzer().Analyze(Array.Empty<BenchmarkSample>()); await ReportExporter.ExportAsync(file.Path, _lastDiagnostics, summary); Status.Text = "Informe exportado; no contiene métricas simuladas.";
    }
}
