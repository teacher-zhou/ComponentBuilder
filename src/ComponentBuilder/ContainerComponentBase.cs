namespace ComponentBuilder;

/// <summary>
/// A container component for dynamic animation support.
/// </summary>
public abstract class ContainerComponentBase : ComponentBase
{
    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var index = 0;
        this.GetType().Assembly.GetTypes()
            .Where(m => typeof(IContainerComponent).IsAssignableFrom(m) && m.IsClass && !m.IsAbstract)
            .ForEach(type =>
            {
                builder.CreateComponent(type, index);
                index++;
            });
    }
}
