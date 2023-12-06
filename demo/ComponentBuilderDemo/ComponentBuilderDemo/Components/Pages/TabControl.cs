using ComponentBuilder;
using ComponentBuilder.Definitions;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace ComponentBuilderDemo.Components.Pages;

[ParentComponent]
[CssClass("tab")]
[HtmlTag("ul")]
public class TabControl : BlazorComponentBase, IHasChildContent
{
    [Parameter]public RenderFragment? ChildContent { get; set; }



    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.CreateCascadingComponent(this, 0, ChildContent);

        builder.Content(BuildTitleItems);

        builder.Content(BuildTabpane);
    }

    //protected override void AddContent(RenderTreeBuilder builder, int sequence)
    //{
    //    builder.AddContent(0, ChildContent);
    //    builder.Content(BuildTitleItems);

    //    builder.Content(BuildTabpane);
    //}


    void BuildTitleItems(RenderTreeBuilder builder)
    {
        foreach (var child in ChildComponents)
        {
            var item = (TabItem)child;

            var alias = item.GetAliasId();
            builder.Element("li")
                .Content(b => b.Element("a")
                                    .Attribute("href", $"#{alias}")
                                    .Data("bs-toggle", "tab")
                                    .Role("tab")
                                    .Aria("control", alias)
                                    .Aria("selected", item.Active.ToString().ToLower())
                                    .Content(alias)
                                    .Close())
                //.Role("presentation")
                //.Key(item)
                .Close();
        }
    }

    void BuildTabpane(RenderTreeBuilder builder)
    {
        builder.Div("tab-content")
            .Class("pt-5")
            .Content(pane =>
            {
                foreach (var item in ChildComponents)
                {
                    pane.Component(item.GetType()).Close();
                }
            })
            .Close();
    }
}

[ChildComponent<TabControl>]
[HtmlTag("li")]
public class TabItem : BlazorComponentBase, IHasChildContent
{
    //protected override void OnInitialized()
    //{
    //    CascadingTabControl?.AddChildComponent(this);
    //}

    [CascadingParameter]public TabControl CascadingTabControl { get;private set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter][EditorRequired] public string? Title { get; set; }
    [Parameter][CssClass("active")] public bool Active { get; set; }
    [Parameter] public string? Alias { get; set; }
    internal string? GetAliasId() => Alias ?? Title ?? Guid.NewGuid().ToString();
}
