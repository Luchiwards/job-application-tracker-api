using System;

namespace JobApplicationTracker.Domain.Common;

public abstract class Entity
{
    protected Entity(int id)
    {
        Id = id;
    }

    public int Id { get; protected set; }

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

    public override int GetHashCode() => Id.GetHashCode();
}

