using System.Linq;

namespace ComponentBuilder
{
    /// <summary>
    /// The extensions of <see cref="RenderTreeBuilder"/> class.
    /// </summary>
    public static class RenderTreeBuilderExtensions
    {
        /// <summary>
        /// Create element withing specified element name.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="elementName">Element tag name.</param>
        /// <param name="content">A delegate to render UI content of this element.</param>
        /// <param name="attributes">Attributes of element.</param>
        /// <returns>A element has created for <see cref="RenderTreeBuilder"/> instance.</returns>
        public static RenderTreeBuilder CreateElement(this RenderTreeBuilder builder, int sequence, string elementName, RenderFragment? content = default, object? attributes = default)
        {
            if (string.IsNullOrEmpty(elementName))
            {
                throw new ArgumentException($"'{nameof(elementName)}' cannot be null or empty.", nameof(elementName));
            }

            builder.OpenRegion(sequence);
            builder.OpenElement(0, elementName);
            if (attributes is not null)
            {
                builder.AddMultipleAttributes(1, attributes.GetType().GetProperties().Select(m => new KeyValuePair<string, object>(m.Name, m.GetValue(attributes))));
            }
            if (content is not null)
            {
                builder.AddContent(100, content);
            }
            builder.CloseElement();
            builder.CloseRegion();
            return builder;
        }

        /// <summary>
        /// Create component withing specified component type.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
        /// <param name="componentType">The type of the child component.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="content">A delegate to render UI content of this element.</param>
        /// <param name="attributes">Attributes of element.</param>
        /// <returns>A element has created for <see cref="RenderTreeBuilder"/> instance.</returns>
        public static void CreateComponent(this RenderTreeBuilder builder, Type componentType, int sequence, RenderFragment? content = default, object? attributes = default)
        {
            if (componentType is null)
            {
                throw new ArgumentNullException(nameof(componentType));
            }

            builder.OpenRegion(sequence);
            builder.OpenComponent(0, componentType);

            if (attributes is not null)
            {
                builder.AddMultipleAttributes(1, attributes.GetType().GetProperties().Select(m => new KeyValuePair<string, object>(m.Name, m.GetValue(attributes))));
            }
            if (content is not null)
            {
                builder.AddContent(100, content);
            }
            builder.CloseComponent();
            builder.CloseRegion();
        }

        /// <summary>
        /// Create component withing specified component type.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="content">A delegate to render UI content of this element.</param>
        /// <param name="attributes">Attributes of element.</param>
        /// <typeparam name="TComponent">The type of the child component. </typeparam>
        /// <returns>A element has created for <see cref="RenderTreeBuilder"/> instance.</returns>
        public static void CreateComponent<TComponent>(this RenderTreeBuilder builder, int sequence, RenderFragment? content = default, object attributes = default) where TComponent : Microsoft.AspNetCore.Components.ComponentBase
        => builder.CreateComponent(typeof(TComponent), sequence, content, attributes);

        /// <summary>
        /// Create a cascading component for <see cref="CascadingValue{TValue}"/> component.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
        /// <param name="value">The value of cascading parameter to create.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="content">A delegate to render UI content of this element.</param>
        /// <param name="name">The name of cascading parameter.</param>
        /// <param name="isFixed">If <c>true</c>, indicates that <see cref="CascadingValue{TValue}.Value"/> will not change. This is a performance optimization that allows the framework to skip setting up change notifications.</param>
        /// <returns>A element has created for <see cref="RenderTreeBuilder"/> instance.</returns>
        public static RenderTreeBuilder CreateCascadingComponent<TValue>(this RenderTreeBuilder builder, TValue value, int sequence, RenderFragment content, string? name = default, bool isFixed = default)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            builder.OpenRegion(sequence);
            builder.OpenComponent<CascadingValue<TValue>>(0);
            builder.AddAttribute(1, nameof(CascadingValue<TValue>.ChildContent), content);
            if (!string.IsNullOrEmpty(name))
            {
                builder.AddAttribute(2, nameof(CascadingValue<TValue>.Name), name);
            }
            builder.AddAttribute(3, nameof(CascadingValue<TValue>.IsFixed), isFixed);
            builder.AddAttribute(4, nameof(CascadingValue<TValue>.Value), value);
            builder.CloseComponent();
            builder.CloseRegion();
            return builder;
        }

        /// <summary>
        /// Create a cascading value for this component into specify <see cref="RenderTreeBuilder"/> class.
        /// </summary>
        /// <param name="component">The cascading parameter to create.</param>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="content">A delegate to render UI content of this element.</param>
        /// <param name="name">The name of cascading parameter.</param>
        /// <param name="isFixed">If <c>true</c>, indicates that <see cref="CascadingValue{TValue}.Value"/> will not change. This is a performance optimization that allows the framework to skip setting up change notifications.</param>
        /// <returns>A element has created for <see cref="RenderTreeBuilder"/> instance.</returns>
        public static RenderTreeBuilder CreateCascadingComponent<TValue>(this Microsoft.AspNetCore.Components.ComponentBase component, RenderTreeBuilder builder, int sequence, RenderFragment content, string? name = default, bool isFixed = default)
        {
            if (component is null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            builder.OpenRegion(sequence);
            builder.OpenComponent<CascadingValue<TValue>>(0);
            builder.AddAttribute(1, nameof(CascadingValue<TValue>.ChildContent), content);
            if (!string.IsNullOrEmpty(name))
            {
                builder.AddAttribute(2, nameof(CascadingValue<TValue>.Name), name);
            }
            builder.AddAttribute(3, nameof(CascadingValue<TValue>.IsFixed), isFixed);
            builder.AddAttribute(4, nameof(CascadingValue<TValue>.Value), component);
            builder.CloseComponent();
            builder.CloseRegion();
            return builder;
        }
    }
}
