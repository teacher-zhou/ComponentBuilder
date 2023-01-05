using System.Reflection;

namespace ComponentBuilder.Interceptors;
internal class AssociationComponentInterceptor:ComponentInterceptorBase
{
    public override void InterceptOnInitialized(IRazorComponent component)
    {
        var componentType = component.GetType();

        //TODO Replace Nullable to associate with parent component for Optional

        var cascadingComponentAttributes = componentType.GetCustomAttributes<ChildComponentAttribute>();
        ;
        if ( cascadingComponentAttributes is null )
        {
            return;
        }

        foreach ( var attr in cascadingComponentAttributes )
        {
            foreach ( var property in componentType.GetProperties().Where(m => m.IsDefined(typeof(CascadingParameterAttribute))) )
            {
                var propertyType = property.PropertyType;
                var propertyValue = property.GetValue(component);

                if ( propertyType != attr.ComponentType )
                {
                    continue;
                }
                if ( !attr.Optional && propertyValue is null )
                {
                    throw new InvalidOperationException(@$"
Component {componentType.Name} has defined {nameof(ChildComponentAttribute)} attribute, it means this component can only be the child of {attr.ComponentType.Name} component, like:

<{attr.ComponentType.Name}>
    <{componentType.Name}></{componentType.Name}>
    ...
    <{componentType.Name}></{componentType.Name}>
</{attr.ComponentType.Name}>

Then you can have a cascading parameter of {attr.ComponentType.Name} component with public modifier get the instance automatically, like: 

[CascadingParameter]public {attr.ComponentType.Name}? MyParent {{ get; set; }}

Set Optional is true of {nameof(ChildComponentAttribute)} can ignore this exception means current component can be child component of {attr.ComponentType.Name} optionally, and the cascading parameter of parent component may be null.
");
                }

                if ( propertyType is not null && propertyValue is not null )
                {
                    propertyType!.GetMethod("AddChildComponent")!.Invoke(propertyValue!, new[] { component });
                }
            }
        }
    }

}
