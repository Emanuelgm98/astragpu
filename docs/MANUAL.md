# Manual local

1. Ejecuta la aplicación como usuario estándar.
2. En Diagnóstico, revisa cada fuente y estado. `unavailable` significa que Windows no expone ese dato mediante las fuentes integradas. Las últimas 50 capturas se conservan localmente para auditoría.
3. En Biblioteca selecciona un `.exe` existente. AstraGPU valida la cabecera `MZ`, guarda solo la ruta y nunca inicia el proceso.
4. Backup crea un ZIP de `%LOCALAPPDATA%\AstraGPU`; Restaurar escribe únicamente dentro de esa carpeta y rechaza traversal.
5. Benchmark solo analiza muestras observadas. Un informe sin muestras no contiene FPS.
6. Los perfiles son recomendaciones conservadoras; no cambian servicios, registro, drivers, seguridad ni hardware.

## Publicación

Con .NET 8 y el runtime Windows App SDK instalados, ejecuta `packaging/Publish-Windows.ps1`. El resultado es una carpeta distribuible, no un instalador MSI/MSIX.
