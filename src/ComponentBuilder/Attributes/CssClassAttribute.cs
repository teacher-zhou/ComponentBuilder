namespace ComponentBuilder;

/// <summary>
/// 标记该特性的属性（组件的参数）、类、枚举、字段、接口都会被构造成组件的 class 字符串。
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public class CssClassAttribute : Attribute
{
    /// <summary>
    /// 使用参数名称作为 CSS 字符串以初始化 <see cref="CssClassAttribute"/> 类的新实例。
    /// <para>
    /// 注意: 该功能只用于标记了 <c>[Parameter]</c> 特性的属性。
    /// </para>
    /// </summary>
    public CssClassAttribute() : this(default)
    {

    }

    /// <summary>
    /// 使用指定的 class 字符串初始化 <see cref="CssClassAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="css">CSS 字符串。</param>
    public CssClassAttribute(string? css) => CSS = css;

    /// <summary>
    /// 获取 CSS 字符串。
    /// </summary>
    public string? CSS { get; }
    /// <summary>
    /// 获取或设置在 class 中的字符串排列顺序。
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// 获取或设置一个布尔值，该值禁止将 CSS 值应用于当前参数。
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// 获取或设置一个布尔值，该值可以连接来自继承组件的 CSS 字符串的值。
    /// <para>
    /// 注意: 该参数仅在组件类中有效。
    /// </para>
    /// <code language="cs">
    /// [CssClass("class1")]
    /// public class Component1 : BlazorComponentBase { }
    /// 
    /// [CssClass("class2", Concat = true)]
    /// public class Component2 : Component1 { }
    /// 
    /// [CssClass("class3")]
    /// public class Component3 : Component1 { }
    /// </code>
    /// <code language="html">
    /// &lt;Component1 /> //生成的 class 为 class="class1"
    /// &lt;Component2 /> //生成的 class 为 class="class1 class2"
    /// &lt;Component3 /> //生成的 class 为 class="class3"
    /// </code>
    /// </summary>
    public bool Concat { get; set; }
}