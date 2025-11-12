using System;

namespace JobApplicationTracker.Domain.Common;

/// <summary>
/// Provides guard clause helpers for validating domain invariants.
/// </summary>
public static class Guard
{
    /// <summary>
    /// Ensures that a string argument is neither null nor whitespace.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <param name="parameterName">Name of the parameter being validated.</param>
    /// <returns>The validated value.</returns>
    /// <exception cref="ArgumentException">Thrown when the value is null or whitespace.</exception>
    public static string AgainstNullOrWhiteSpace(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{parameterName} cannot be null or whitespace.", parameterName);
        }

        return value.Trim();
    }
}

