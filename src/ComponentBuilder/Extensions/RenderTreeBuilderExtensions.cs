namespace ComponentBuilder
{
    /// <summary>
    /// The extensions of <see cref="RenderTreeBuilder"/> class.
    /// </summary>
    public static class RenderTreeBuilderExtensions
    {
        #region CreateElement
        /// <summary>
        /// Create element withing specified element name.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="elementName">Element tag name.</param>
        /// <param name="childContent">Child content to add.</param>
        /// <param name="attributes">Attributes of element.</param>
        /// <param name="condition">A condition to create element.</param>
        /// <param name="appendFunc">A delegate of function to append custom frames. 
        /// <para>
        /// The instance <see cref="RenderTreeBuilder"/> for first argument and the second argument is the sequence for lastest position of source code in <see cref="RenderTreeBuilder"/>, and you have to return the last sequence of source code after frames appended.
        /// </para>
        /// </param>
        /// <exception cref="ArgumentException"><paramref name="elementName"/> is empty or null.</exception>
        public static void CreateElement(this RenderTreeBuilder builder, int sequence, string elementName, RenderFragment? childContent = default, object? attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int> appendFunc = default)
        => builder.CreateElement(sequence, elementName, (object)childContent, attributes, condition, appendFunc);

        /// <summary>
        /// Create element withing specified element name.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="elementName">Element tag name.</param>
        /// <param name="markupString">Content for the markup text frame.</param>
        /// <param name="attributes">Attributes of element.</param>
        /// <param name="condition">A condition to create component.</param>
        /// <param name="appendFunc">A delegate of function to append custom frames. 
        /// <para>
        /// The instance <see cref="RenderTreeBuilder"/> for first argument and the second argument is the sequence for lastest position of source code in <see cref="RenderTreeBuilder"/>, and you have to return the last sequence of source code after frames appended.
        /// </para>
        /// </param>
        /// <exception cref="ArgumentException"><paramref name="elementName"/> is empty or null.</exception>
        public static void CreateElement(this RenderTreeBuilder builder, int sequence, string elementName, string markupString, object? attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int> appendFunc = default)
        => builder.CreateElement(sequence, elementName, (object)markupString, attributes, condition, appendFunc);

        /// <summary>
        /// Create element withing specified element name.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="elementName">Element tag name.</param>
        /// <param name="content">Content for the markup text frame.</param>
        /// <param name="attributes">Attributes of element.</param>
        /// <param name="condition">A condition to create component.</param>
        /// <param name="appendFunc">A delegate of function to append custom frames. 
        /// <para>
        /// The instance <see cref="RenderTreeBuilder"/> for first argument and the second argument is the sequence for lastest position of source code in <see cref="RenderTreeBuilder"/>, and you have to return the last sequence of source code after frames appended.
        /// </para>
        /// </param>
        /// <exception cref="ArgumentException"><paramref name="elementName"/> is empty or null.</exception>
        internal static void CreateElement(this RenderTreeBuilder builder, int sequence, string elementName, object content
            , object? attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int> appendFunc = default)
        {
            if (string.IsNullOrEmpty(elementName))
            {
                throw new ArgumentException($"'{nameof(elementName)}' cannot be null or empty.", nameof(elementName));
            }

            if (!condition)
            {
                return;
            }

            builder.OpenRegion(sequence);
            builder.OpenElement(0, elementName);

            int lastSequence = 0;
            if (appendFunc is not null)
            {
                lastSequence = appendFunc.Invoke(builder, lastSequence);
            }

            if (attributes is not null)
            {
                builder.AddMultipleAttributes(lastSequence + 1, HtmlHelper.MergeHtmlAttributes(attributes));
            }

            builder.AddChildContent(lastSequence + 2, content);


            builder.CloseElement();
            builder.CloseRegion();
        }
        #endregion

        #region CreateComponent
        /// <summary>
        /// Create component withing specified component type.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
        /// <param name="componentType">The type of the component.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="childContent">Child content frame to add.</param>
        /// <param name="attributes">Attributes of component.</param>
        /// <param name="condition">A condition to create component.</param>
        /// <param name="appendFunc">A delegate of function to append custom frames. 
        /// <para>
        /// The instance <see cref="RenderTreeBuilder"/> for first argument and the second argument is the sequence for lastest position of source code in <see cref="RenderTreeBuilder"/>, and you have to return the last sequence of source code after frames appended.
        /// </para>
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="componentType"/> is null.</exception>
        public static void CreateComponent(this RenderTreeBuilder builder, Type componentType, int sequence, RenderFragment? childContent = default, object? attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int> appendFunc = default)
        => builder.CreateComponent(componentType, sequence, (object)childContent, attributes, condition, appendFunc);


        /// <summary>
        /// Create component withing specified component type.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
        /// <param name="componentType">The type of the component.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="markupString">Content for the markup text frame.
        /// <para>
        /// Mark sure component has <c>ChildContent</c> parameter to create child markup string.
        /// </para> 
        /// </param>
        /// <param name="attributes">Attributes of component.</param>
        /// <param name="condition">A condition to create component.</param>
        /// <param name="appendFunc">A delegate of function to append custom frames. 
        /// <para>
        /// The instance <see cref="RenderTreeBuilder"/> for first argument and the second argument is the sequence for lastest position of source code in <see cref="RenderTreeBuilder"/>, and you have to return the last sequence of source code after frames appended.
        /// </para>
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="componentType"/> is null.</exception>
        public static void CreateComponent(this RenderTreeBuilder builder, Type componentType, int sequence, string markupString, object attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int> appendFunc = default)
        => builder.CreateComponent(componentType, sequence, (object)markupString, attributes, condition, appendFunc);

        internal static void CreateComponent(this RenderTreeBuilder builder, Type componentType, int sequence, object content, object attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int> appendFunc = default)
        {
            if (componentType is null)
            {
                throw new ArgumentNullException(nameof(componentType));
            }

            if (!condition)
            {
                return;
            }

            builder.OpenRegion(sequence);
            builder.OpenComponent(0, componentType);

            int lastSequence = 1;
            if (appendFunc is not null)
            {
                lastSequence = appendFunc.Invoke(builder, lastSequence);
            }

            if (attributes is not null)
            {
                //foreach (var item in CssHelper.MergeAttributes(attributes))
                //{
                //    builder.AddAttribute(lastSequence + 1, item.Key, item.Value);
                //}

                builder.AddMultipleAttributes(lastSequence + 1, HtmlHelper.MergeHtmlAttributes(attributes));
            }
            builder.AddChildContentAttribute(lastSequence + 2, content);



            builder.CloseComponent();
            builder.CloseRegion();
        }

        /// <summary>
        /// Create component withing specified component type.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="childContent">Child content frame to add.</param>
        /// <param name="attributes">Attributes of component.</param>
        /// <param name="condition">A condition to create component.</param>
        /// <param name="appendFunc">A delegate of function to append custom frames. 
        /// <para>
        /// The instance <see cref="RenderTreeBuilder"/> for first argument and the second argument is the sequence for lastest position of source code in <see cref="RenderTreeBuilder"/>, and you have to return the last sequence of source code after frames appended.
        /// </para>
        /// </param>
        /// <typeparam name="TComponent">The type of the child component. </typeparam>
        public static void CreateComponent<TComponent>(this RenderTreeBuilder builder, int sequence, RenderFragment? childContent = default, object attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int> appendFunc = default) where TComponent : ComponentBase
        => builder.CreateComponent(typeof(TComponent), sequence, childContent, attributes, condition, appendFunc);

        /// <summary>
        /// Create component withing specified component type.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="markupString">Content for the markup text frame.
        /// <para>
        /// Mark sure component has <c>ChildContent</c> parameter to create child markup string.
        /// </para> 
        /// </param>
        /// <param name="attributes">Attributes of component.</param>
        /// <param name="condition">A condition to create component.</param>
        /// <param name="appendFunc">A delegate of function to append custom frames. 
        /// <para>
        /// The instance <see cref="RenderTreeBuilder"/> for first argument and the second argument is the sequence for lastest position of source code in <see cref="RenderTreeBuilder"/>, and you have to return the last sequence of source code after frames appended.
        /// </para>
        /// </param>
        /// <typeparam name="TComponent">The type of the component. </typeparam>
        public static void CreateComponent<TComponent>(this RenderTreeBuilder builder, int sequence, string markupString, object attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int> appendFunc = default) where TComponent : ComponentBase
        => builder.CreateComponent(typeof(TComponent), sequence, markupString, attributes, condition, appendFunc);
        #endregion


        /// <summary>
        /// Create a cascading component for <see cref="CascadingValue{TValue}"/> component.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
        /// <param name="value">The value of cascading parameter to create.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="content">A delegate to render UI content of this element.</param>
        /// <param name="name">The name of cascading parameter.</param>
        /// <param name="isFixed">If <c>true</c>, indicates that <see cref="CascadingValue{TValue}.Value"/> will not change. This is a performance optimization that allows the framework to skip setting up change notifications.</param>
        /// <returns>A cascading component has created for <see cref="RenderTreeBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> or <paramref name="content"/> is null.
        /// </exception>
        public static void CreateCascadingComponent<TValue>(this RenderTreeBuilder builder, TValue value, int sequence, RenderFragment content, string? name = default, bool isFixed = default)
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
        /// <returns>A cascading component has created for <see cref="RenderTreeBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="component"/> or <paramref name="builder"/> or <paramref name="content"/> is null.
        /// </exception>
        public static void CreateCascadingComponent<TValue>(this ComponentBase component, RenderTreeBuilder builder, int sequence, RenderFragment content, string? name = default, bool isFixed = default)
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
        }

        /// <summary>
        /// Appends text frame to <c>ChildContent</c> parameter.
        /// <para>
        /// It is same as <c>builder.AddAttribute(sequence,"ChildContent",content)</c> for <see cref="RenderTreeBuilder"/> class.
        /// </para>
        /// </summary>
        /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="textContent">Content for the new text frame.</param>
        /// <returns>An attribute has added for <see cref="RenderTreeBuilder"/> instance.</returns>
        public static RenderTreeBuilder AddChildContentAttribute(this RenderTreeBuilder builder, int sequence, string textContent)
        => builder.AddChildContentAttribute(sequence, (object)textContent);

        /// <summary>
        /// Appends text frame to <c>ChildContent</c> parameter. 
        /// <para>
        /// It is same as <c>builder.AddAttribute(sequence,"ChildContent",content)</c> for <see cref="RenderTreeBuilder"/> class.
        /// </para>
        /// </summary>
        /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="fragment">Content to add.</param>
        /// <returns>An attribute has added for <see cref="RenderTreeBuilder"/> instance.</returns>
        public static RenderTreeBuilder AddChildContentAttribute(this RenderTreeBuilder builder, int sequence, RenderFragment fragment)
        => builder.AddChildContentAttribute(sequence, (object)fragment);

        /// <summary>
        /// Appends text frame to <c>ChildContent</c> parameter.
        /// <para>
        /// It is same as <c>builder.AddAttribute(sequence,"ChildContent",content)</c> for <see cref="RenderTreeBuilder"/> class.
        /// </para>
        /// </summary>
        /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="markupContent">Markup content for the new markup frame.</param>
        /// <returns>An attribute has added for <see cref="RenderTreeBuilder"/> instance.</returns>
        public static RenderTreeBuilder AddChildContentAttribute(this RenderTreeBuilder builder, int sequence, MarkupString markupContent)
        => builder.AddChildContentAttribute(sequence, (object)markupContent);

        /// <summary>
        /// Appends text frame to <c>ChildContent</c> parameter.
        /// <para>
        /// It is same as <c>builder.AddAttribute(sequence,"ChildContent",content)</c> for <see cref="RenderTreeBuilder"/> class.
        /// </para>
        /// </summary>
        /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="content">Content for the frame. <see cref="string"/> or <see cref="RenderFragment"/> type.</param>
        /// <returns>An attribute has added for <see cref="RenderTreeBuilder"/> instance.</returns>
        internal static RenderTreeBuilder AddChildContentAttribute(this RenderTreeBuilder builder, int sequence, object content)
        {
            if (content is not null)
            {
                builder.AddAttribute(sequence, "ChildContent", (RenderFragment)(child =>
                {
                    switch (content)
                    {
                        case string:
                            child.AddContent(sequence, content?.ToString() ?? string.Empty);
                            break;
                        case RenderFragment fragment:
                            child.AddContent(sequence, fragment);
                            break;
                        case MarkupString markupString:
                            child.AddContent(sequence, markupString);
                            break;
                    }
                }));
            }

            return builder;
        }

        /// <summary>
        /// Appends text frame to <c>Content</c>.
        /// <para>
        /// It is same as <c>builder.AddAttribute(sequence,"ChildContent",content)</c> for <see cref="RenderTreeBuilder"/> class.
        /// </para>
        /// </summary>
        /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <param name="content">Content for the frame. <see cref="string"/> or <see cref="RenderFragment"/> type.</param>
        /// <returns>An attribute has added for <see cref="RenderTreeBuilder"/> instance.</returns>
        internal static RenderTreeBuilder AddChildContent(this RenderTreeBuilder builder, int sequence, object content)
        {
            if (content is not null)
            {
                switch (content)
                {
                    case string:
                        builder.AddContent(sequence, content?.ToString() ?? string.Empty);
                        break;
                    case RenderFragment fragment:
                        builder.AddContent(sequence, fragment);
                        break;
                    case MarkupString markupString:
                        builder.AddContent(sequence, markupString);
                        break;
                }
            }

            return builder;
        }
    }
}
