using ComponentBuilder.Abstrations;
using ComponentBuilder.Parameters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace ComponentBuilder.Demo.ServerSide.Components
{
    [CssClass("form-control")]
    [HtmlTag("input")]
    [ChildComponent(typeof(TestForm))]
    public class FormInput<TValue> : BlazorComponentBase,IHasInputValue<TValue>
    {
        public FormInput()
        {
        }

        private void EditContext_OnValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
        {
            StateHasChanged();
        }

        [CascadingParameter] public TestForm Form { get; set; }
        [Parameter]public Expression<Func<TValue?>>? ValueExpression { get; set; }
        [Parameter]public TValue? Value { get; set; }
        [Parameter]public EventCallback<TValue?> ValueChanged { get; set; }
        public EditContext? EditContext { get; set; }
        [CascadingParameter]public EditContext? CascadedEditContext { get; set; }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            this.InitializeInputValue();

            InvokeOnParameterSetInterceptors();

            await base.SetParametersAsync(parameters);
        }

        /// <summary>
        /// Build input attributes
        /// </summary>
        /// <param name="attributes">The attributes contains all resolvers to build attributes and <see cref="P:ComponentBuilder.BlazorComponentBase.AdditionalAttributes" />.</param>
        protected override void BuildAttributes(IDictionary<string, object> attributes)
        {
            attributes["type"] = "text";
            attributes["value"] = this.GetValueAsString();
            attributes["onchange"] = this.CreateValueChangedCallback();
        }

        protected override void DisposeComponentResources()
        {
            base.DisposeComponentResources();

            this.DisposeInputValue();
        }
    }
}
