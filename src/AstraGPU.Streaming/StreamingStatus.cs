namespace AstraGPU.Streaming;

public sealed record StreamingStatus(bool Available, string Message)
{
    public static StreamingStatus NotConfigured => new(false, "Streaming opcional aún no está configurado. Conecta manualmente un PC autorizado en tu red local.");
}
