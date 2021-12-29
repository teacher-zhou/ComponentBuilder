namespace ComponentBuilder;

/// <summary>
/// Represents the element role name with specify value.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class HtmlRoleAttribute : HtmlAttributeAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="HtmlRoleAttribute"/> class.
    /// </summary>
    /// <param name="value">The role value of element.</param>
    public HtmlRoleAttribute(object value) : base("role", value)
    {
    }
}
