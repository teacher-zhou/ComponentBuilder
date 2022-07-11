using ComponentBuilder.Abstrations.Internal;

namespace ComponentBuilder;

/// <summary>
/// Provides a static class to get utilities of css extensions.
/// </summary>
public static class Css
{
    /// <summary>
    /// Return a new instance to build css class utilities.
    /// </summary>
    public static ICssClassProvider Class => new DefaultCssClassProviderBuilder();
}
