using ComponentBuilder.Abstrations;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using Microsoft.AspNetCore.Components.Rendering;

namespace ComponentBuilder
{
    /// <summary>
    /// Provides a base class for component that can build css class quickly.
    /// </summary>
    public abstract class BlazorComponentBase : ComponentBase, IBlazorComponent
    {
        #region Properties
        #region Injection
        [Inject] ICssClassBuilder CssClassBuilder { get; set; }
        [Inject] ICssClassResolver CssClassResolver { get; set; }
        [Inject] IComponentBuilder ComponentBuilder { get; set; }
        #endregion


        #region Parameters
        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, object>();
        [Parameter] public string AdditionalCssClass { get; set; }
        #endregion
        #endregion

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

            CssClassBuilder.Append(CssClassResolver.Resolve(GetType()));

            BuildCssClass(CssClassBuilder);

            if (!string.IsNullOrEmpty(AdditionalCssClass))
            {
                CssClassBuilder.Append(AdditionalCssClass);
            }

            return CssClassBuilder.Build();
        }

        public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);

        public virtual string GetElementName()
        {
            if (GetType().TryGetAttribute<HtmlElementAttribute>(out var element))
            {
                return element.ElementName;
            }
            return "div";
        }
        #endregion

        #region Protected
        protected virtual void BuildCssClass(ICssClassBuilder builder)
        {
        }
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            ComponentBuilder.BuildComponent(this, builder);
        }
        #endregion

        #region Private
        #endregion

        #endregion
    }
}
