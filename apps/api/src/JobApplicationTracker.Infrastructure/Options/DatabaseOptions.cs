namespace JobApplicationTracker.Infrastructure.Options;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    public string Provider { get; set; } = "sqlite";

    public string? ConnectionString { get; set; }

    public bool EnableSensitiveLogging { get; set; }
}

