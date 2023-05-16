//namespace ComponentBuilder.Automation.Resolvers;

///// <summary>
///// A resolver to resolve <see cref="IFluentClassProvider"/> parameter of component.
///// </summary>
//public class FluentCssClassResolver : ICssClassResolver
//{
//    /// <inheritdoc/>
//    public IEnumerable<string> Resolve(IBlazorComponent component)
//    {
//        var componentType = component.GetType();

//        var list = new List<string>();

//        foreach (var parameter in componentType.GetProperties().Where(m => typeof(IFluentClassProvider).IsAssignableFrom(m.PropertyType)))
//        {
//            if ( parameter.GetValue(component) is IFluentClassProvider builder )
//            {
//                list.AddRange(builder.Create().Where(x => !string.IsNullOrEmpty(x)));
//            }
//        }
//        return list;
//    }
//}
