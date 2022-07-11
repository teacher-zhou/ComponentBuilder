using ComponentBuilder.Abstrations;


using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Test.Components
{
    public class Button : BlazorChildContentComponentBase
    {
        protected override string TagName => "button";

        [Parameter] [CssClass("block")] public bool Block { get; set; }

        [Parameter] public bool Active { get; set; }

        protected override void BuildCssClass(ICssClassBuilder builder)
        {
            if (Active)
            {
                builder.Append("active");
            }
        }
    }

}
