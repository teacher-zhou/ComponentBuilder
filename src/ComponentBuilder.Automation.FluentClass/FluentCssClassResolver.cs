using ComponentBuilder.FluentClass;

namespace ComponentBuilder.Automation.Resolvers;

/// <summary>
/// Resolves parameter supports <see cref="IFluentClassProvider"/> type to build CSS class string.
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
