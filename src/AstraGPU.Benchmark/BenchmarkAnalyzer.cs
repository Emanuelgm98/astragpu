using AstraGPU.Core;

namespace AstraGPU.Benchmark;

/// <summary>Summarizes observed frame-time samples only; never synthesizes benchmark measurements.</summary>
public sealed class BenchmarkAnalyzer
{
    public BenchmarkSummary Analyze(IEnumerable<BenchmarkSample> samples)
    {
        var values = samples.Where(s => s.FrameTimeMs is > 0 and var ms && double.IsFinite(ms)).Select(s => s.FrameTimeMs!.Value).Order().ToArray();
        if (values.Length == 0) return new("Importación de telemetría real", 0, null, null, null, "Aún no hay muestras. AstraGPU no ejecuta juegos ni genera valores simulados.");
        var median = Percentile(values, .5); var p95 = Percentile(values, .95);
        var onePercent = values.Length < 100 ? null : 1000d / Percentile(values, .99);
        return new("Muestras observadas; condiciones y proveedor deben registrarse", values.Length, median, p95, onePercent,
            "Resultados dependen del juego, escena, resolución, controlador y método de captura. Repetir varias pasadas y comparar frametimes bajo condiciones iguales.");
    }
    private static double Percentile(double[] xs, double p) => xs[(int)Math.Ceiling(p * xs.Length) - 1];
}
