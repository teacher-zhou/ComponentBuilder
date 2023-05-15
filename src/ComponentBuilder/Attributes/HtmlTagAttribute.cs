using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder.Automation;

/// <summary>
/// Apply for component class to generate HTML tag name.
/// </summary>
[AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface,AllowMultiple =false)]
public class HtmlTagAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlTagAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of HTML tag.</param>
    public HtmlTagAttribute([NotNull] string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <summary>
    /// Gets the HTML tag name.
    /// </summary>
    public string Name { get; }
}
