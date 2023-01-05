using ComponentBuilder.Abstrations.Internal;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Reflection;

namespace ComponentBuilder;

/// <summary>
/// Represents a base class with automation component features. This is an abstract class.
/// </summary>
public abstract partial class BlazorComponentBase : RazorComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BlazorComponentBase"/> class.
    /// </summary>
    protected BlazorComponentBase() : base()
    {
    }

    #region Properties

    #region Protected

    /// <summary>
    /// Returns the HTML element tag name. Default to get <see cref="HtmlTagAttribute"/> defined by component class.
    /// </summary>
    /// <returns>The element tag name to create HTML element.</returns>
    protected virtual string? GetElementTagName()
    {
        var tagName = ServiceProvider?.GetRequiredService<HtmlTagAttributeResolver>().Resolve(this);
        return tagName;
    }

    /// <summary>
    /// Overrides the source code sequence that generates the startup sequence in the instance.
    /// </summary>
    protected virtual int RegionSequence => GetHashCode();

    /// <summary>
    /// Gets or sets a boolean value wheither to capture the reference of html element.
    /// </summary>
    protected bool CaptureReference { get; set; }

    /// <summary>
    /// Gets the reference of element when <see cref="CaptureReference"/> is <c>true</c>.
    /// </summary>
    protected ElementReference? Reference { get; private set; }
    #endregion

    #endregion Properties

    #region Method

    #region Core

    /// <summary>
    /// Automatically build component by ComponentBuilder with new region. 
    /// <para>
    /// NOTE: Override to build component by yourself, and remember call <see cref="BuildComponentFeatures(RenderTreeBuilder)"/> to apply automatic features for specific <see cref="RenderTreeBuilder"/> instance.
    /// </para>
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> .</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenRegion(RegionSequence);
        CreateComponentTree(builder, BuildComponentFeatures);
        builder.CloseRegion();
    }

    #endregion

    #region Protected

    #region Can Override

    /// <summary>
    /// Build component attributes to supplies <see cref="RenderTreeBuilder"/> instance.
    /// <para>
    /// <note type="important">
    /// NOTE: Overrides may lose all features of ComponentBuilder framework.
    /// </note>
    /// </para>
    /// </summary>
    /// <param name="builder">A instance of <see cref="RenderTreeBuilder"/> .</param>
    /// <param name="sequence">Return an integer number representing the last sequence of source code.</param>
    protected void BuildComponentAttributes(RenderTreeBuilder builder, out int sequence)
    {
        builder.AddMultipleAttributes(sequence = 4, AdditionalAttributes);
    }

    /// <summary>
    /// Capture the element reference if <see cref="CaptureReference"/> is <c>true</c>.
    /// <para>
    /// <see cref="Reference"/> will be null if this method is never called.
    /// </para>
    /// </summary>
    /// <param name="builder">A instance of <see cref="RenderTreeBuilder"/> .</param>
    /// <param name="sequence">Return an integer number representing the last sequence of source code.</param>
    protected virtual void CaptureElementReference(RenderTreeBuilder builder, int sequence)
    {
        if ( Options.CaptureReference || CaptureReference)
        {
            builder.AddElementReferenceCapture(sequence, element => Reference = element);
        }
    }
    #endregion

    #region AddContent
    /// <summary>
    /// Add innter content to component.
    /// <para>
    /// Implement from <see cref="IHasChildContent"/> interface to add ChildContent automatically.
    /// </para>
    /// <para>
    /// Normally, override this method to build inner html structures.
    /// </para>
    /// </summary>
    /// <param name="builder">A instance of <see cref="RenderTreeBuilder"/> .</param>
    /// <param name="sequence">An integer number representing the sequence of source code.</param>
    protected virtual void AddContent(RenderTreeBuilder builder, int sequence)
        => AddChildContent(builder, sequence);

    /// <summary>
    /// Add ChildContent parameter to this component if <see cref="IHasChildContent"/> is implemented.
    /// </summary>
    /// <param name="builder">A instance of <see cref="RenderTreeBuilder"/> .</param>
    /// <param name="sequence">An integer number representing the sequence of source code.</param>
    protected void AddChildContent(RenderTreeBuilder builder, int sequence)
    {
        if (this is IHasForm form)
        {
            builder.CreateCascadingComponent(form.FixedEditContext, 0, content =>
            {
                content.AddContent(0, form.ChildContent?.Invoke(form.FixedEditContext));

            }, isFixed: true);
        }
        else if (this is IHasChildContent content)
        {
            builder.AddContent(sequence, content.ChildContent);
        }
    }

    #endregion


    /// <summary>
    /// Build the features of element or component by ComponentBuilder Framework.
    /// <para>
    /// You can build features for any specific <see cref="RenderTreeBuilder"/> instance.
    /// </para>
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> to apply the features.</param>
    protected void BuildComponentFeatures(RenderTreeBuilder builder)
    {
        BuildComponentAttributes(builder, out var sequence);
        CaptureElementReference(builder, sequence + 1);
        AddContent(builder, sequence + 2);
    }

    //protected override void BuildAttributes(IDictionary<string, object> attributes)
    //{
    //    SetComponentAttribute(attributes);
    //    base.BuildAttributes(attributes);
    //}

    //protected void SetComponentAttribute(IDictionary<string,object> attributes)
    //{
    //    BuildForm(attributes);
    //    BuildNavLink(attributes);

    //    void BuildForm(IDictionary<string, object> htmlAttributes)
    //    {
    //        if ( this is IHasForm && _handleSubmitDelegate != null )
    //        {
    //            htmlAttributes["onsubmit"] = _handleSubmitDelegate;
    //        }
    //    }

    //    void BuildNavLink(IDictionary<string, object> htmlAttributes)
    //    {
    //    }
    //}

    #endregion

    #region Private

    /// <summary>
    /// Create the component tree.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="continoues">An action after component or element is created.</param>
    private void CreateComponentTree(RenderTreeBuilder builder, Action<RenderTreeBuilder> continoues)
    {
        var componentType = GetType();

        var parentComponent = componentType.GetCustomAttribute<ParentComponentAttribute>();
        if (parentComponent is null)
        {
            CreateComponentOrElement(builder, continoues);
        }
        else
        {
            var extensionType = typeof(RenderTreeBuilderExtensions);

            var methods = extensionType.GetMethods()
                .Where(m => m.Name == nameof(RenderTreeBuilderExtensions.CreateCascadingComponent));

            var method = methods.FirstOrDefault();
            if (method is null)
            {
                return;
            }

            var genericMethod = method.MakeGenericMethod(componentType);

            RenderFragment content = new(content =>
            {
                CreateComponentOrElement(content, _ => continoues(content));
            });

            genericMethod.Invoke(null, new object[] { builder, this, 0, content, parentComponent.Name!, parentComponent.IsFixed });
        }

        void CreateComponentOrElement(RenderTreeBuilder builder, Action<RenderTreeBuilder> continoues)
        {
            var tagName = GetElementTagName() ?? throw new InvalidOperationException("Tag name cannot be null or empty");
            builder.OpenElement(0, tagName);
            continoues(builder);
            builder.CloseElement();
        }
    }
    #endregion
    #endregion

}
