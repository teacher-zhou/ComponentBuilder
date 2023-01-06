using System.Reflection;

namespace ComponentBuilder.Interceptors;
/// <summary>
/// Associate current component with specified parent instance and check the association of structure.
/// </summary>
internal class AssociationComponentInterceptor : ComponentInterceptorBase
{
    /// <inheritdoc/>
    public override void InterceptOnInitialized(IBlazorComponent component)
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
                    var currentComponentName = componentType.Name;
                    var parentCompoentName = attr.ComponentType.Name;

                    throw new InvalidOperationException(@$"
Component {currentComponentName} has defined {nameof(ChildComponentAttribute)} attribute, it means this component can only be the child of {parentCompoentName} component, like:
<{parentCompoentName}>
    <{currentComponentName}></{currentComponentName}>
</{parentCompoentName}>

Define a cascading parameter of {parentCompoentName} with public modifier get the instance automatically, this step is optional: 

[CascadingParameter]public {parentCompoentName}? Cascading{parentCompoentName} {{ get; set; }}

To ignore the strong association validation, please set Optional is true of {nameof(ChildComponentAttribute)}, therefore the cascading parameter of {parentCompoentName} component may be null.
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
