using System.Reflection;

namespace ComponentBuilder.Rendering;
/// <summary>
/// Default renderer.
/// </summary>
internal class DefaultComponentRender : IComponentRenderer
{
    /// <inheritdoc/>
    public bool Render(IBlazorComponent component, RenderTreeBuilder builder)
    {
        if (component is BlazorComponentBase blazorComponent)
        {
            CreateComponentTree(builder, blazorComponent.BuildComponent);
        }

        return false;

        #region CreateComponentTree
        void CreateComponentTree(RenderTreeBuilder builder, Action<RenderTreeBuilder> continoues)
        {
            var componentType = component.GetType();

            var parentComponent = componentType.GetCustomAttribute<ParentComponentAttribute>();
            if ( parentComponent is null )
            {
                CreateComponentOrElement(builder, continoues);
            }
            else
            {
                var extensionType = typeof(RenderTreeBuilderExtensions);

                var methods = extensionType.GetMethods()
                    .Where(m => m.Name == nameof(RenderTreeBuilderExtensions.CreateCascadingComponent));

                var method = methods.FirstOrDefault();
                if ( method is null )
                {
                    return;
                }

                var genericMethod = method.MakeGenericMethod(componentType);

                RenderFragment content = new(content =>
                {
                    CreateComponentOrElement(content, _ => continoues(content));
                });

                genericMethod.Invoke(null, new object[] { builder, component, 0, content, parentComponent.Name!, parentComponent.IsFixed });
            }

            void CreateComponentOrElement(RenderTreeBuilder builder, Action<RenderTreeBuilder> continoues)
            {
                var tagName = blazorComponent.GetTagName() ?? throw new InvalidOperationException("Tag name cannot be null or empty");

                //builder.OpenRegion(Guid.NewGuid().GetHashCode());
                builder.OpenElement(0, tagName);
                continoues(builder);
                builder.CloseElement();
                //builder.CloseRegion();
            }
        }
        #endregion
    }
}
