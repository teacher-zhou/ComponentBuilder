using ComponentBuilder;
using ComponentBuilder.Definitions;
using ComponentBuilder.Fluent;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;

namespace ComponentBuilder.Demo.ServerSide.Components;

/// <summary>
/// 表示导航栏组件。
/// </summary>
[HtmlTag("nav")]
[CssClass("navbar")]
[ParentComponent(IsFixed = true)]
public class NewNavbar : BlazorComponentBase,IHasChildContent
{
    /// <summary>
    /// 初始化 <see cref="NewNavbar"/> 类的新实例。
    /// </summary>
    public NewNavbar()
    {
        CaptureReference = true;
    }


    /// <summary>
    /// 设置背景颜色为渐变色。
    /// </summary>
    [Parameter][CssClass("bg-gradient")] public bool Gradient { get; set; }
    /// <summary>
    /// 导航栏放置方位。
    /// </summary>
    [Parameter][CssClass] public NavbarPlacement? Placement { get; set; }

    /// <summary>
    /// 设置 LOGO 品牌部分的 UI 内容。
    /// </summary>
    [Parameter] public RenderFragment? BrandContent { get; set; }
    /// <summary>
    /// LOGO 品牌部分的超链接地址，默认是 “/”。
    /// </summary>
    [Parameter] public string? BrandLink { get; set; } = "/";
    /// <summary>
    /// LOGO 品牌部分的 HTML 元素，默认是 a 元素。
    /// </summary>
    [Parameter] public string BrandTagName { get; set; } = "a";

    /// <summary>
    /// 应用响应式菜单的切换按钮。默认是 <c>true</c>。
    /// </summary>
    [Parameter] public bool EnableToggler { get; set; } = true;

    /// <summary>
    /// 响应式菜单切换按钮的触发方式。
    /// </summary>
    [Parameter] public NavbarToggleTrigger ToggleTrigger { get; set; } = NavbarToggleTrigger.Collapse;

    [Parameter] public RenderFragment? NavItemsContent { get; set; }
    [Parameter]public RenderFragment? ChildContent { get; set; }

    private Collapse? _refToggleCollapse;


    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.Div("container",sequence:sequence).Content(
            content =>
            {
                #region Brand
                content.Element(BrandTagName, "navbar-brand")
                        .Attribute("href", BrandLink, BrandTagName == "a")
                        .Content(BrandContent)
                        .Close();
                #endregion

                #region Toggler
                //content.CreateElement(sequence, "button", button => button.Span("navbar-toggler-icon").Close(), new
                //{
                //    @class = "navbar-toggler",
                //    type = "button",
                //    onclick = EventCallback.Factory.Create(this, Toggle)
                //});

                content.OpenElement(50, "button");
                content.AddAttribute(51, "class", "navbar-toggler");
                content.AddAttribute(52, "onclick", EventCallback.Factory.Create(this, Toggle));
                content.AddContent(5, (button => button.Span("navbar-toggler-icon").Close()));
                content.CloseElement();

                #endregion

                if ( ToggleTrigger == NavbarToggleTrigger.Collapse )
                {
                    content.OpenComponent<Collapse>(100);
                    content.AddAttribute(101, "AdditionalClass", "navbar-collapse");
                    content.AddAttribute(102, "ChildContent", BuildNavItems);
                    content.AddComponentReferenceCapture(103, component => _refToggleCollapse = (Collapse)component);
                    content.CloseComponent();

                    //content.CreateComponent<Collapse>(sequence + 1, BuildNavItems, new
                    //{
                    //    AdditionalClass = "navbar-collapse"
                    //}, captureReference: component => _refToggleCollapse = (Collapse)component);
                }
            }).Close();
    }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder
            .Append("navbar-dark bg-primary")
            ;
    }

    private void BuildNavItems(RenderTreeBuilder builder)
    {
        builder.Ul("navbar-nav me-auto").Content(NavItemsContent).Close();
    }

    public async Task Toggle()
    {
        if ( ToggleTrigger == NavbarToggleTrigger.Collapse && _refToggleCollapse is not null )
        {
            await _refToggleCollapse.Toggle();
        }
    }


}

/// <summary>
/// 导航响应式断点的触发器。
/// </summary>
public enum NavbarToggleTrigger
{
    Collapse,
    Offcanvas
}
/// <summary>
/// 导航栏位置。
/// </summary>
public enum NavbarPlacement
{
    /// <summary>
    /// 固定在顶部。
    /// </summary>
    [CssClass("fixed-top")] FixedTop,
    /// <summary>
    /// 固定在底部。
    /// </summary>
    [CssClass("fixed-bottom")] FixedBottom,
    /// <summary>
    /// 黏在顶部。
    /// </summary>
    [CssClass("sticky-top")] StickyTop,
    /// <summary>
    /// 黏在底部。
    /// </summary>
    [CssClass("sticky-bottom")] StickyBottom
}
