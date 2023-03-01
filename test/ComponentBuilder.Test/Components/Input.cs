using ComponentBuilder.Abstrations;
using ComponentBuilder.Definitions.Parameters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder.Test.Components
{
    [HtmlTag("input")]
    public class Input<TValue> : BlazorComponentBase, IHasInputValue<TValue>
    {
       [Parameter] public Expression<Func<TValue?>>? ValueExpression { get; set; }
        [CascadingParameter]public EditContext? CascadedEditContext { get; set; }
        [Parameter] public TValue? Value { get; set; }
        [Parameter] public EventCallback<TValue?> ValueChanged { get; set; }
        public EditContext? EditContext { get; set; }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            this.InitializeInputValue();

            return base.SetParametersAsync(parameters);
        }

        protected override void BuildAttributes(IDictionary<string, object> attributes)
        {
            attributes["type"] = "text";
            attributes["value"] = this.Value;
            attributes["onchange"] = this.CreateValueChangedCallback();
        }

        protected override void DisposeComponentResources()
        {
            base.DisposeComponentResources();

            this.DisposeInputValue();
        }
    }
}
