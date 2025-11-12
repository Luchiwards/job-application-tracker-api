using System;

namespace JobApplicationTracker.Domain.Common;

public static class Guard
{
    public static string AgainstNullOrWhiteSpace(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{parameterName} cannot be null or whitespace.", parameterName);
        }

        return value.Trim();
    }
}

