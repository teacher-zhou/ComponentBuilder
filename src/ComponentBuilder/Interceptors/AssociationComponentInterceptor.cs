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

        var childAtrributes = componentType.GetCustomAttributes<ChildComponentAttribute>();

        foreach ( var childComponentAttribute in childAtrributes )
        {
            foreach ( var property in componentType.GetProperties().Where(m => m.IsDefined(typeof(CascadingParameterAttribute))) )
            {
                var cascadingType = property.PropertyType;
                var cascadingValue = property.GetValue(component);

                if ( !childComponentAttribute.Optional && cascadingValue is null )
                {
                    var currentComponentName = componentType.Name;
                    var parentComponentName = childComponentAttribute.ComponentType.Name;

                    throw new InvalidOperationException(@$"
The component {currentComponentName} has defined {nameof(ChildComponentAttribute)} witch means should be the child component of {parentComponentName}.
Example of code:
<{parentComponentName}>
    <{currentComponentName}>...</{currentComponentName}>
</{parentComponentName}>

Set [ChildComponent(Optional = true)] to ignore this validation.
");
                }

                if ( cascadingType.IsAssignableTo(typeof(BlazorComponentBase)) && cascadingValue is not null )
                {
                    cascadingType!.GetMethod("AddChildComponent")?.Invoke(cascadingValue!, new[] { component });
                }
            }
        }
    }
}
