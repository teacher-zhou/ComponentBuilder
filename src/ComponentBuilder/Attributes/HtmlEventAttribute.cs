namespace ComponentBuilder;

/// <summary>
/// Represents a callback event for <see cref="EventCallback"/> or <see cref="EventCallback{TValue}"/> parameter.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class HtmlEventAttribute : HtmlAttributeAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="HtmlEventAttribute"/> class.
    /// </summary>
    /// <param name="name">The event name. Like 'onclick' etc.</param>
    public HtmlEventAttribute(string name):base(name)
    {
    }
}
