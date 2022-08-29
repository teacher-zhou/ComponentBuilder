namespace ComponentBuilder;

/// <summary>
/// 定义类可以作为枚举。
/// </summary>
public abstract class Enumeration
{
    /// <summary>
    /// 初始化 <see cref="Enumeration"/> 类的新实例。
    /// </summary>
    /// <param name="value">枚举的值。</param>
    protected Enumeration(string value)
    {
        Value = value;
    }
    /// <summary>
    /// 获取枚举的值。
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// 获取枚举成员。
    /// </summary>
    /// <returns>枚举成员值的数组。</returns>
    public IEnumerable<string?> GetMembers()
        => GetType().GetFields(System.Reflection.BindingFlags.Static).Select(m => ((Enumeration?)m.GetValue(this))?.Value);
}
