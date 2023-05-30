namespace ComponentBuilder;

/// <summary>
/// The extensions of <see cref="StyleSelector"/>.
/// </summary>
public static class StyleSelectorExtensions
{
    /// <summary>
    /// 添加指定选择器的CSS属性。
    /// </summary>
    /// <param name="selectors"></param>
    /// <param name="selector">CSS 选择器。</param>
    /// <param name="attributes">选择器的值。</param>
    public static StyleSelector AddStyle(this StyleSelector selectors, string selector, object attributes) => selectors.Add(selector, new StyleProperty(attributes));

    /// <summary>
    /// 添加 @keyframes 部分的选择器。
    /// </summary>
    /// <param name="selector"></param>
    /// <param name="name">关键帧名称。</param>
    /// <param name="configure">用于配置关键帧内容的委托。</param>
    /// <exception cref="ArgumentException"><paramref name="name"/> is null or empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="configure"/> is null.</exception>
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
