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

        [Inject] private ICssClassBuilder CssClassBuilder { get; set; }
        [Inject] private ICssClassResolver CssClassResolver { get; set; }

        #endregion Injection

        #region Parameters

        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, object>();
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
        public string GetCssClassString()
        {
            if (AdditionalAttributes.TryGetValue("class", out object value))
            {
                return value.ToString();
            }

            CssClassBuilder.Append(CssClassResolver.Resolve(this));

            BuildCssClass(CssClassBuilder);

            if (!string.IsNullOrEmpty(AdditionalCssClass))
            {
                CssClassBuilder.Append(AdditionalCssClass);
            }

            return CssClassBuilder.Build();
        }

        public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);

        #endregion Public

        #region Protected

        protected virtual void BuildCssClass(ICssClassBuilder builder)
        {
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, GetElementTagName());
            builder.AddAttribute(1, "class", GetCssClassString());
            builder.AddMultipleAttributes(2, AdditionalAttributes);
            AddChildContent(3, builder);
            builder.CloseElement();
        }

        protected virtual string GetElementTagName()
        {
            if (GetType().TryGetAttribute<ElementTagAttribute>(out var element))
            {
                return element.Name;
            }
            return "div";
        }

        protected void AddChildContent(int sequence, RenderTreeBuilder builder)
        {
            if (this is IHasChildContent content)
            {
                builder.AddContent(sequence, content.ChildContent);
            }
        }

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
        // ~BlazorComponentBase()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Protected

        #endregion Method
    }
}