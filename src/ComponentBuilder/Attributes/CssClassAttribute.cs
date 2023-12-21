namespace ComponentBuilder;

/// <summary>
/// Concate parameters, component class, enum members with all CSS string after renderering to build a 'class' value of element.
/// </summary>
/// <param name="cssClass">CSS 字符串。</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public class CssClassAttribute(string? cssClass = default) : Attribute
{

    /// <summary>
    /// Gets the CSS value.
    /// </summary>
    public string? CSS { get; } = cssClass;
    /// <summary>
    /// Gets the sequence of CSS value.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets a <see cref="bool"/> weither disable concating or not.
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets a Boolean value that can be concatenated with the value of the CSS string from the inherited component.
    /// <para>
    /// NOTE: The property only valid for Component Class.
    /// </para>
    /// <code language="cs">
    /// [CssClass("class1")]
    /// public class Component1 : BlazorComponentBase { }
    /// 
    /// [CssClass("class2", Inherited = true)]
    /// public class Component2 : Component1 { }
    /// 
    /// [CssClass("class3")]
    /// public class Component3 : Component1 { }
    /// </code>
    /// <code language="html">
    /// &lt;Component1 /> // class = "class1"
    /// &lt;Component2 /> // class = "class1 class2"
    /// &lt;Component3 /> // class = "class3"
    /// </code>
    /// </summary>
    public bool Inherited { get; set; }
}