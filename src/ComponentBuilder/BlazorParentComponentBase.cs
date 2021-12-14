namespace ComponentBuilder
{
    /// <summary>
    /// Represents a base parament component class associate to <see cref="BlazorChildComponentBase{TParentComponent}"/> class.
    /// </summary>
    /// <typeparam name="TComponent">Component type.</typeparam>
    public abstract class BlazorParentComponentBase<TComponent> : BlazorChildContentCompoentnBase
        where TComponent : ComponentBase
    {
        /// <summary>
        /// If <c>true</c>, indicates that <see cref="CascadingValue{TValue}.Value"/> will not change. This is a performance optimization that allows the framework to skip setting up change notifications.
        /// </summary>
        protected virtual bool IsFixed => false;
        /// <summary>
        /// Gets the name of cascading parameter.
        /// </summary>
        /// <value>value can be <c>null</c>.</value>
        protected virtual string? Name => null;

        /// <summary>
        /// Override to create cascading value for this component and continue building component tree.
        /// </summary>
        /// <param name="builder"></param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            this.CreateCascadingComponent<TComponent>(builder, 1, (child) =>
            {
                child.OpenElement(0, GetElementTagName());
                BuildComponentRenderTree(child);
                child.CloseElement();
            }, Name, IsFixed);
        }
    }
}
