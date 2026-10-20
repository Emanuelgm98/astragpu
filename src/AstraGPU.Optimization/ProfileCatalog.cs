using AstraGPU.Core;

namespace AstraGPU.Optimization;

public sealed class ProfileCatalog
{
    public IReadOnlyList<OptimizationProfile> GetProfiles() =>
    [
        new("Predeterminado", "Sin cambios al sistema; reúne diagnóstico y biblioteca local.", new Dictionary<string,string>()),
        new("Sesión de juego", "Cierra solo aplicaciones que el usuario seleccione. No modifica servicios ni políticas del sistema.", new Dictionary<string,string> { ["close_user_selected_apps"] = "false" })
    ];
}
