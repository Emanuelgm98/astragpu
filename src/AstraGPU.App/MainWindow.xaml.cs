using AstraGPU.Benchmark;
using AstraGPU.Core;
using AstraGPU.Hardware;
using AstraGPU.Optimization;
using AstraGPU.Streaming;
using Microsoft.UI.Xaml;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace AstraGPU.App;
public sealed partial class MainWindow : Window
{
    private readonly IHardwareDiagnostics _hardware = new WindowsDiagnostics();
    private readonly LocalLibrary _library = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AstraGPU"));
    private string _page = "dashboard";
    public MainWindow() { InitializeComponent(); }
    private void Show(string title, string body, string action, string page) { Heading.Text = title; ContentTitle.Text = title; Content.Text = body; ActionButton.Content = action; _page = page; Status.Text = ""; }
    private void OnDashboard(object s, RoutedEventArgs e) => Show("Optimización local, con evidencia", "Diagnostica el equipo, organiza tus juegos y revisa mediciones reales. No se aplican cambios ocultos.", "Ejecutar diagnóstico local", "dashboard");
    private void OnDiagnostics(object s, RoutedEventArgs e) => Show("Diagnóstico", "Consulta datos que Windows expone sin elevación. Sensores no compatibles se muestran como No disponible.", "Actualizar diagnóstico", "diagnostics");
    private void OnLibrary(object s, RoutedEventArgs e) => Show("Biblioteca local", "Añade manualmente ejecutables .exe existentes. La biblioteca queda en tu perfil local de Windows.", "Añadir juego…", "library");
    private void OnProfiles(object s, RoutedEventArgs e) => Show("Perfiles conservadores", string.Join("\n\n", new ProfileCatalog().GetProfiles().Select(p => $"{p.Name}: {p.Description}")), "Ver perfiles", "profiles");
    private void OnBenchmark(object s, RoutedEventArgs e) { var summary = new BenchmarkAnalyzer().Analyze([]); Show("Benchmark", summary.Limitations, "Ver metodología", "benchmark"); }
    private void OnStreaming(object s, RoutedEventArgs e) => Show("Streaming opcional", StreamingStatus.NotConfigured.Message, "Estado", "streaming");
    private async void OnAction(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_page is "diagnostics" or "dashboard")
            {
                var snapshot = await _hardware.CaptureAsync(); Content.Text = string.Join("\n", snapshot.Values.Select(v => $"{v.Name}: {v.Value}")); Status.Text = $"Capturado {snapshot.CapturedAt.ToLocalTime():g}. Sin elevación.";
            }
            else if (_page == "library")
            {
                var picker = new FileOpenPicker { SuggestedStartLocation = PickerLocationId.ComputerFolder }; picker.FileTypeFilter.Add(".exe");
                InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(this)); var file = await picker.PickSingleFileAsync();
                if (file is null) return; await _library.AddAsync(Path.GetFileNameWithoutExtension(file.Name), file.Path); var games = await _library.GetAsync();
                Content.Text = games.Count == 0 ? "Biblioteca vacía." : string.Join("\n", games.Select(g => $"{g.Name} — {g.ExecutablePath}")); Status.Text = "Guardado localmente.";
            }
            else Status.Text = _page == "benchmark" ? "Sin muestras reales: no hay resultados numéricos." : _page == "streaming" ? StreamingStatus.NotConfigured.Message : "Este perfil solo describe opciones; no cambia ajustes del sistema.";
        }
        catch (Exception ex) { Status.Text = $"No se pudo completar la operación: {ex.Message}"; }
    }
}
