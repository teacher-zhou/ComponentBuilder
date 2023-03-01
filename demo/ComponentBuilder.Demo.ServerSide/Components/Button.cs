using ComponentBuilder.Parameters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace ComponentBuilder.Demo.ServerSide.Components
{
    [CssClass("btn")]
    public class Button : BlazorComponentBase, IHasChildContent, IHasOnClick, IHasDisabled
    {
        public Button()
        {
        }

        public override string GetTagName()=>"button";
        [Parameter][CssClass("btn-")] public Color? Color { get; set; }

        [Parameter][CssClass("active")] public bool Active { get; set; }

        [Parameter] public bool HasToggle { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public EventCallback<MouseEventArgs?> OnClick { get; set; }
        [Parameter][HtmlAttribute] public bool Disabled { get; set; }

        bool Clicked;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            //builder.AddContent(0, content =>
            //{
            builder.CreateStyleRegion(0, style =>
            {
                style.AddKeyFrames("tran", (kf) =>
                {
                    kf.Add("from", new { width = "0px" });
                    kf.Add("to", new { width = "500px" });
                });
                style.AddStyle(".bigger", new { animation = "tran 3s" });
            });
            //});

            base.BuildRenderTree(builder);
        }
        protected override void BuildCssClass(ICssClassBuilder builder)
        {
            builder.Append("active toggle", HasToggle).Append("bigger", Clicked);

        }

        protected override void BuildAttributes(IDictionary<string, object> attributes)
        {
            attributes["onmouseover"] = HtmlHelper.Event.Create(this, () =>
            {
                Clicked = true;
                StateHasChanged();
            });
        }
    }

    public enum Color
    {
        Primary,
        Secondary,
        Danger,
        Warning,
        Info,
        Dark,
        Light,
        Success
    }
}
