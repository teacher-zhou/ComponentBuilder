using System.Reflection;

namespace ComponentBuilder.Rending;
/// <summary>
/// Provides a default component renderer.
/// </summary>
internal class DefaultComponentRender : IComponentRender
{
    /// <inheritdoc/>
    public bool Render(IBlazorComponent component, RenderTreeBuilder builder)
    {
        CreateComponentTree(builder, component.BuildComponent);

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
                var tagName = component.GetTagName() ?? throw new InvalidOperationException("Tag name cannot be null or empty");
                builder.OpenElement(0, tagName);
                continoues(builder);
                builder.CloseElement();
            }
        }
        #endregion
    }
}
