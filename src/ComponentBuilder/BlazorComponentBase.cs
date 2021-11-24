using ComponentBuilder.Abstrations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComponentBuilder
{
    /// <summary>
    /// Provides a base class for component that can build css class quickly.
    /// </summary>
    public abstract class BlazorComponentBase : ComponentBase, IBlazorComponent, IDisposable
    {
        private bool disposedValue;
        #region Properties

        #region Injection
        /// <summary>
        /// Injection of <see cref="ICssClassBuilder"/> instance.
        /// </summary>
        [Inject] private ICssClassBuilder CssClassBuilder { get; set; }
        /// <summary>
        /// Injection of <see cref="ICssClassResolver"/> instance.
        /// </summary>
        [Inject] private ICssClassResolver CssClassResolver { get; set; }

        #endregion Injection

        #region Parameters

        /// <summary>
        /// Gets or sets the additional attribute for element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets to append addtional css class at behind for component.
        /// </summary>
        [Parameter] public string AdditionalCssClass { get; set; }

        #endregion Parameters

        #endregion Properties

        #region Method

        #region Public

        /// <summary>
        /// Build the css class for component by settings.
        /// <para>
        /// Ignore all settings when <c>class</c> value of attribute in element is set.
        /// </para>
        /// </summary>
        /// <returns>A series css class string seperated by spece for each item.</returns>
        public string? GetCssClassString()
        {
            if (AdditionalAttributes.TryGetValue("class", out object? value))
            {
                return value?.ToString();
            }

            CssClassBuilder.Append(CssClassResolver.Resolve(this));

            BuildCssClass(CssClassBuilder);

            if (!string.IsNullOrEmpty(AdditionalCssClass))
            {
                CssClassBuilder.Append(AdditionalCssClass);
            }

            return CssClassBuilder.ToString();
        }

        /// <summary>
        /// Notifies the component that its state has changed. When applicable, this will cause the component to be re-rendered. 
        /// </summary>
        public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);

        #endregion Public

        #region Protected

        /// <summary>
        /// Overrides to create css class by special logical process.
        /// </summary>
        /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
        protected virtual void BuildCssClass(ICssClassBuilder builder)
        {
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, GetElementTagName());
            builder.AddAttribute(1, "class", GetCssClassString());
            builder.AddMultipleAttributes(2, AdditionalAttributes);
            TryAddChildContent(builder, 3);
            builder.CloseElement();
        }

        /// <summary>
        /// Gets element tag name if specified <see cref="ElementTagAttribute"/> in component class. Default is 'div';
        /// </summary>
        /// <returns>Html tag name.</returns>
        protected virtual string GetElementTagName()
        {
            if (GetType().TryGetAttribute<ElementTagAttribute>(out var element))
            {
                return element.Name;
            }
            return "div";
        }

        /// <summary>
        /// Try to appends frames representing an arbitrary fragment of content if component has implemeted <see cref="IHasChildContent"/>.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to append.</param>
        /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
        /// <returns><c>true</c> to add content in <see cref="RenderTreeBuilder"/> class, otherwise <c>false</c>.</returns>
        protected bool TryAddChildContent(RenderTreeBuilder builder, int sequence)
        {
            if (this is IHasChildContent content)
            {
                builder.AddContent(sequence, content.ChildContent);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Try to add class a
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="sequence"></param>
        /// <returns><c>true</c> to add class attribute in <see cref="RenderTreeBuilder"/> class, otherwise <c>false</c>.</returns>
        protected bool TryAddClassAttribute(RenderTreeBuilder builder, int sequence)
        {
            var cssClass = GetCssClassString();
            if (!string.IsNullOrEmpty(cssClass))
            {
                builder.AddAttribute(sequence, "class", cssClass);
                return true;
            }
            return false;
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                CssClassBuilder.Dispose();
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~BlazorComponentBase()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #endregion Protected

        #endregion Method
    }
}