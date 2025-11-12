namespace JobApplicationTracker.Infrastructure.Options;

/// <summary>
/// Represents database configuration options bound from configuration.
/// </summary>
public sealed class DatabaseOptions
{
    /// <summary>
    /// Name of the configuration section that contains database settings.
    /// </summary>
    public const string SectionName = "Database";

    /// <summary>
    /// Gets or sets the database provider to use (for example, sqlite).
    /// </summary>
    public string Provider { get; set; } = "sqlite";

    /// <summary>
    /// Gets or sets the connection string used to connect to the database.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether sensitive data logging should be enabled.
    /// </summary>
    public bool EnableSensitiveLogging { get; set; }
}

