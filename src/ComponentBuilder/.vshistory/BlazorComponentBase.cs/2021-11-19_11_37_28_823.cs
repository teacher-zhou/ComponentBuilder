using ComponentBuilder.Abstrations;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;

namespace ComponentBuilder
{
    /// <summary>
    /// Provides a base class for component that can build css class quickly.
    /// </summary>
    public abstract class BlazorComponentBase : ComponentBase, IBlazorComponent
    {
        #region Properties
        #region Injection
        [Inject] ICssClassBuilder CssBuilder { get; set; }
        [Inject] ICssClassResolver CssClassResolver { get; set; }
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
        /// <returns></returns>
        public string BuildCssClassString()
        {
            if (AdditionalAttributes.TryGetValue("class", out object value))
            {
                return value.ToString();
            }

            CssBuilder.Append(CssClassResolver.Resolve(GetType()));

            return CssBuilder.Build();
        }

        public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);
        #endregion

        #region Protected
        #endregion

        #region Private
        #endregion

        #endregion
    }
}
