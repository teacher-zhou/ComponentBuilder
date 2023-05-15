using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder.Automation;

/// <summary>
/// Define for component parameter generate <c>data-*</c> value when value is set.
/// </summary>
public class HtmlDataAttribute : HtmlAttributeAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlDataAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of data, could be <c>data-{name}</c> format.</param>
    public HtmlDataAttribute([NotNull] string name) : base($"data-{name}")
    {
    }
}
