namespace ComponentBuilder;

/// <summary>
/// The extensions of <see cref="StyleSelector"/>.
/// </summary>
public static class StyleSelectorExtensions
{
    /// <summary>
    /// Adds the CSS property that specifies the selector.
    /// </summary>
    /// <param name="selectors"></param>
    /// <param name="selector">The CSS selector.</param>
    /// <param name="attributes">The values of selector.</param>
    public static StyleSelector AddStyle(this StyleSelector selectors, string selector, object attributes) => selectors.Add(selector, new StyleProperty(attributes));

    /// <summary>
    /// Adds style for @keyframes part.
    /// </summary>
    /// <param name="selector"></param>
    /// <param name="name">The name of keyframe.</param>
    /// <param name="configure">An configuration action to create keyframe values.</param>
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
