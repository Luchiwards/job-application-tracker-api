using System;

namespace JobApplicationTracker.Domain.Common;

/// <summary>
/// Serves as the base class for entities that are identified by a primary key.
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Entity"/> class.
    /// </summary>
    /// <param name="id">Unique identifier for the entity.</param>
    protected Entity(int id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    public int Id { get; protected set; }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not Entity other || other.GetType() != GetType())
        {
            return false;
        }

        return Id.Equals(other.Id);
    }

    /// <inheritdoc />
    public override int GetHashCode() => Id.GetHashCode();
}

