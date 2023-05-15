namespace ComponentBuilder.FluentRenderTree;
/// <summary>
/// Provides attributes builder.
/// </summary>
public interface IFluentAttributeBuilder :IFluentFrameBuilder
{
    /// <summary>
    /// Add element attribute or component parameter and attribute.
    /// </summary>
    /// <param name="name">The name of HTML attribute or parameter.</param>
    /// <param name="value">The value of attribute or parameter.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains attrbutes or parameters.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is null or empty string.</exception>
    IFluentAttributeBuilder Attribute(string name, object? value);
}
