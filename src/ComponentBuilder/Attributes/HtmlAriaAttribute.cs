using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;

/// <summary>
/// Define for component parameter generate <c>aria-*</c> value when value is set.
/// </summary>
public class HtmlAriaAttribute : HtmlAttributeAttribute
{

    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlAriaAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of aria, could be <c>aria-{name}</c> format.</param>
    public HtmlAriaAttribute([NotNull] string name) : base($"aria-{name}")
    {
    }
}
