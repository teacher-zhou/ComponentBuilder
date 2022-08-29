namespace ComponentBuilder;

/// <summary>
/// CSS 类构建事件的参数。
/// </summary>
public class CssClassEventArgs : EventArgs
{

    /// <summary>
    /// 初始化 <see cref="CssClassEventArgs"/> 类的新实例。
    /// </summary>
    /// <param name="builder"></param>
    public CssClassEventArgs(ICssClassBuilder builder)
    {
        Builder = builder;
    }
    /// <summary>
    /// 获取构建 CSS 的构造器。
    /// </summary>
    public ICssClassBuilder Builder { get; }
}
