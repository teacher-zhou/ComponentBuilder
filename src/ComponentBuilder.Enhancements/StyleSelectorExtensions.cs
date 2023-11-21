namespace ComponentBuilder;

/// <summary>
/// The extensions of <see cref="StyleSelector"/>.
/// </summary>
public static class StyleSelectorExtensions
{
    /// <summary>
    /// Adds CSS properties for the specified selector.
    /// </summary>
    /// <param name="selectors"></param>
    /// <param name="selector">The selector.</param>
    /// <param name="attributes">The attributes.</param>
    public static StyleSelector AddStyle(this StyleSelector selectors, string selector, object attributes) => selectors.Add(selector, new StyleProperty(attributes));

    /// <summary>
    /// Adds a selector for the @keyframes section.
    /// </summary>
    /// <param name="selector"></param>
    /// <param name="name">Keyframe name.</param>
    /// <param name="configure">Delegate used to configure keyframe content.</param>
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
