using System;

namespace JobApplicationTracker.Application.Common.Exceptions;

/// <summary>
/// Represents an error that occurs when a requested entity cannot be located.
/// </summary>
public sealed class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="name">The type name of the missing entity.</param>
    /// <param name="key">The unique identifier associated with the missing entity.</param>
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
        EntityName = name;
        Key = key;
    }

    /// <summary>
    /// Gets the name of the entity that could not be found.
    /// </summary>
    public string EntityName { get; }

    /// <summary>
    /// Gets the identifier of the entity that could not be found.
    /// </summary>
    public object Key { get; }
}

