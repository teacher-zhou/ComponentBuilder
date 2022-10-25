using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;

/// <summary>
/// Define for component parameter generate <c>data-*</c> value when value is set.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface)]
public class HtmlDataAttribute : HtmlAttributeAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlDataAttribute"/> class.
    /// </summary>
    public HtmlDataAttribute()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlDataAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of data, could be <c>data-{name}</c> format.</param>
    public HtmlDataAttribute([NotNull] string name) : base($"data-{name}")
    {
    }
}
