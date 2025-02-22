using System.Diagnostics.CodeAnalysis;

namespace Serilog.Dashboard.Provider.Extensions;

/// <summary>
/// <see cref="T:String"/> extension methods
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Indicates whether the specified String object is null or an empty string
    /// </summary>
    /// <param name="item">A String reference</param>
    /// <returns>
    ///     <c>true</c> if is null or empty; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? item)
    {
        return string.IsNullOrEmpty(item);
    }

    /// <summary>
    /// Indicates whether a specified string is null, empty, or consists only of white-space characters
    /// </summary>
    /// <param name="item">A String reference</param>
    /// <returns>
    ///      <c>true</c> if is null or empty; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? item)
    {
        if (item == null)
            return true;

        for (int i = 0; i < item.Length; i++)
            if (!char.IsWhiteSpace(item[i]))
                return false;

        return true;
    }

    /// <summary>
    /// Determines whether the specified string is not <see cref="IsNullOrEmpty"/>.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified <paramref name="value"/> is not <see cref="IsNullOrEmpty"/>; otherwise, <c>false</c>.
    /// </returns>
    public static bool HasValue([NotNullWhen(true)] this string? value)
    {
        return !string.IsNullOrEmpty(value);
    }

}
