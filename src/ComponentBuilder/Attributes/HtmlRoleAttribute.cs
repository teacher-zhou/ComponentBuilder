using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder.Automation;

/// <summary>
/// Apply for component class to generate a <c>role</c> HTML attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class HtmlRoleAttribute : HtmlAttributeAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlRoleAttribute"/> class.
    /// </summary>
    /// <param name="value">The value of role attribute in element.</param>
    public HtmlRoleAttribute([NotNull] string value) : base("role")
    {
        Value = value;
    }
}
