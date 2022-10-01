namespace ComponentBuilder;

/// <summary>
/// <see cref="StyleSelector"/> 的扩展。
/// </summary>
public static class StyleSelectorExtensions
{
    /// <summary>
    /// 添加指定选择器的 CSS 属性。
    /// </summary>
    /// <param name="selectors"></param>
    /// <param name="selector">选择器。</param>
    /// <param name="attributes">属性的值。</param>
    public static StyleSelector AddStyle(this StyleSelector selectors, string selector, object attributes) => selectors.Add(selector, new StyleProperty(attributes));

    /// <summary>
    /// 添加 @keyframes 的样式。
    /// </summary>
    /// <param name="selector"></param>
    /// <param name="name">关键帧的名称。</param>
    /// <param name="configure">用于配置关键帧的委托。</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static StyleSelector AddKeyFrames(this StyleSelector selector, string name, Action<StyleKeyFrame> configure)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        if (configure is null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        var keyFrames = new StyleKeyFrame();
        configure.Invoke(keyFrames);
        selector.Add($"@keyframes {name}", keyFrames.ToString());
        return selector;
    }
}
