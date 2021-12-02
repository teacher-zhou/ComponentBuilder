namespace ComponentBuilder.Attributes;

/// <summary>
/// Represents the element role name with specify value.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class ElementRoleAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="ElementRoleAttribute"/> class.
    /// </summary>
    /// <param name="roleName">The role value of element.</param>
    public ElementRoleAttribute(string roleName)
    {
        RoleName = roleName;
    }
    /// <summary>
    /// Gets the role name of element.
    /// </summary>
    public string RoleName { get; }
}
