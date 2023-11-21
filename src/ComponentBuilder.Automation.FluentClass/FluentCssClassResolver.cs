using ComponentBuilder.FluentClass;

namespace ComponentBuilder.Resolvers;

/// <summary>
/// A parser for which component parameters can parse <see cref="IFluentClassProvider"/>.
/// </summary>
public class FluentCssClassResolver : IParameterClassResolver
{
    /// <inheritdoc/>
    public IEnumerable<string> Resolve(IBlazorComponent component)
    {
        var componentType = component.GetType();

        var list = new List<string>();

        foreach ( var parameter in componentType.GetProperties().Where(m => typeof(IFluentClassProvider).IsAssignableFrom(m.PropertyType)) )
        {
            if ( parameter.GetValue(component) is IFluentClassProvider builder )
            {
                list.AddRange(builder.Create().Where(x => !string.IsNullOrEmpty(x)));
            }
        }
        return list;
    }
}
