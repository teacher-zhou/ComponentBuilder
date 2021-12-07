namespace ComponentBuilder
{
    /// <summary>
    /// Represents a base class for child component of <typeparamref name="TParentComponent"/> type.
    /// </summary>
    /// <typeparam name="TParentComponent">The parent component type.</typeparam>
    public abstract class BlazorChildComponentBase<TParentComponent> : BlazorChildContentCompoentnBase
        where TParentComponent : BlazorParentComponentBase<TParentComponent>
    {
        /// <summary>
        /// Gets instance of parent component.
        /// </summary>
        [CascadingParameter] protected TParentComponent ParentComponent { get; private set; }

        /// <summary>
        /// Overried to validate and throw exception when <see cref="ParentComponent"/> is <c>null</c> value.
        /// </summary>
        protected override void OnInitialized()
        {
            ThrowIfParentComponentNull();
            base.OnInitialized();
        }

        /// <summary>
        /// Throws an exception when <see cref="ParentComponent"/> is <c>null</c> value.
        /// </summary>
        /// <exception cref="InvalidOperationException">This component must be the child of <see cref="ParentComponent"/> component.</exception>
        protected virtual void ThrowIfParentComponentNull()
        {
            if (ParentComponent is null)
            {
                throw new InvalidOperationException($"The '{GetType().Name}' component must be the child of '{typeof(TParentComponent).Name}' component");
            }
        }
    }
}
