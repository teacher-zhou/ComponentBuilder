using ComponentBuilder.Abstrations;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComponentBuilder
{
    /// <summary>
    /// Provides a base class for component that can build css class quickly.
    /// </summary>
    public abstract class BlazorComponentBase : ComponentBase, IBlazorComponent
    {
        #region Properties
        #region Injection
        [Inject] protected ICssClassBuilder CssBuilder { get; set; }
        #endregion


        #region Parameters
        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
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
            if (Attributes.TryGetValue("class", out object value))
            {
                return value.ToString();
            }

            return CssBuilder.Build();
        }

        public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);
        #endregion
        #endregion
    }
}
