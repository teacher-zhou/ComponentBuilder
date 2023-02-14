namespace ComponentBuilder.Resolvers;

public class FluentCssClassResolver : ICssClassResolver
{
    /// <inheritdoc/>
    public IEnumerable<string> Resolve(IBlazorComponent component)
    {
        var componentType = component.GetType();
        var list = new List<string>();
        foreach (var parameter in componentType.GetProperties().Where(m => typeof(IFluentCssClassBuilder).IsAssignableFrom(m.PropertyType)))
        {
            var css = parameter.GetValue(component)?.ToString();
            if (!string.IsNullOrEmpty(css))
            {
                list.Add(css);
            }
        }
        return list;
    }
}
