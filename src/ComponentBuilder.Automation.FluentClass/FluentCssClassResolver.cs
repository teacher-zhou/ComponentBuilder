using ComponentBuilder.FluentClass;

namespace ComponentBuilder.Resolvers;

/// <summary>
/// 用于组件参数可以解析 <see cref="IFluentClassProvider"/> 的解析器。
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
