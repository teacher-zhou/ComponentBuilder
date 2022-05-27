namespace ComponentBuilder;

/// <summary>
/// 提供组件可以快速应用相应的 CSS 值。
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public class CssClassAttribute : Attribute
{
    /// <summary>
    /// 初始化 <see cref="CssClassAttribute" /> 类的新实例。
    /// <para>
    /// 若为参数，则使用参数名称作为 CSS 名称。
    /// </para>
    /// </summary>
    public CssClassAttribute() : this(default)
    {

    }

    /// <summary>
    /// 使用指定的 CSS 名称初始化 <see cref="CssClassAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="name">CSS 名称。</param>
    public CssClassAttribute(string? name) => Name = name;

    /// <summary>
    /// 获取 CSS 名称。
    /// </summary>
    public string? Name { get; }
    /// <summary>
    /// 获取或设置 CSS 值得排序，按从小到大排序。
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// 获取或设置一个布尔值，当前参数禁止应用 CSS 值。
    /// <para>
    /// 用于覆盖接口的预定义CSS的值。
    /// </para>
    /// </summary>
    public bool Disabled { get; set; }
    /// <summary>
    /// 当参数不为布尔值时有效，将于参数的值组合成为最新的 CSS 名称
    /// <para>
    /// 若为 <c>true</c> 作为后缀字符串，否则为 <c>false</c>。
    /// </para>
    /// </summary>
    public bool Suffix { get; set; }
}