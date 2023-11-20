using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;

/// <summary>
/// Defines the HTML tag name for component to generate.
/// </summary>
/// <param name="name">The name of HTML tag.</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
public class HtmlTagAttribute([NotNull] string name) : Attribute
{

    /// <summary>
    /// Gets the name of HTML tag.
    /// </summary>
    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));
}
