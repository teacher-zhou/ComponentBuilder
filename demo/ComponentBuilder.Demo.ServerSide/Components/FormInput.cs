using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Demo.ServerSide.Components
{
    [CssClass("form-control")]
    [HtmlTag("input")]
    [ChildComponent(typeof(TestForm))]
    public class FormInput<TValue> : BlazorInputComponentBase<TValue>
    {
        [CascadingParameter] public TestForm Form { get; set; }
        /// <summary>
        /// Build input attributes
        /// </summary>
        /// <param name="attributes">The attributes contains all resolvers to build attributes and <see cref="P:ComponentBuilder.BlazorComponentBase.AdditionalAttributes" />.</param>
        protected override void BuildAttributes(IDictionary<string, object> attributes)
        {
            attributes["type"] = "text";
            attributes["id"] = FieldIdentifier.FieldName;
            attributes["name"] = FieldIdentifier.FieldName;

            AddValueChangedAttribute(attributes);
            //attributes["onchange"] = EventCallback.Factory.CreateBinder(this, _value => CurrentValue = _value, CurrentValue);
        }
    }
}
