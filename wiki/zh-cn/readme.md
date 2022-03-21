# 欢迎使用 ComponentBuilder 

### 什么是 ComponentBuilder
是基于 `RenderTreeBuilder` 的快速构建 Blazor 组件的框架，用最少的代码构建出属于你的组件库。

### 目录
* [首页](readme.md)
* [基础](basic/readme.md)
* [高级](advance/readme.md)
* [动态JS](basic/dynamicJS.md)


### 示例和比较

以一个 `Button` 组件为例

* 使用 `Button.razor` 创建
```html
<button class="@(GetCssClass())" @attributes="@Attributes">
    @ChildContent
</button>

@code{
    [Parameter]public RenderFragment? ChildContent { get; set; }

    [Parameter]public string Color { get; set; }

    [Parameter]public bool Disabled { get; set; }

    [Parameter(CaptureUnmatchedValues=true)]public IDictionary<string, object> Attributes { get; set; }

    string GetCssClass()
    {
        var cssClassList = new List<string>();
        if(!string.IsNullOrEmpty(Color))
        {
            cssClassList.Add($"btn-{Color}");
        }

        if(Disabled)
        {
            cssClassList.Add("disabled");
        }

        return string.Join(" ", cssClassList);
    }
}
```
* 使用 `RenderTreeBuilder` 创建
```csharp
public class Button : ComponentBase
{
    [Parameter]public RenderFragment? ChildContent { get; set; }

    [Parameter]public string Color { get; set; }

    [Parameter]public bool Disabled { get; set; }

    [Parameter(CaptureUnmatchedValues=true)]public IDictionary<string, object> Attributes { get; set; }

    string GetCssClass()
    {
        var cssClassList = new List<string>();
        if(!string.IsNullOrEmpty(Color))
        {
            cssClassList.Add($"btn-{Color}");
        }

        if(Disabled)
        {
            cssClassList.Add("disabled");
        }

        return string.Join(" ", cssClassList);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "button");
        bullder.AddAttribute(1, "class", GetCssClass());
        builder.AddMultipleAttributes(2, Attributes);
        builder.AddContent(3, ChildContent);
        builder.CloseElement();
    }
}
```
* 使用 `ComponentBuilder` 创建

```csharp
[HtmlTag("button")]
public class Button : BlazorComponentBase, IHasChildContent
{
    [Parameter]public RenderFragment? ChildContent { get; set; }

    [Parameter][CssClass("btn-")]public string Color { get; set; }

    [Parameter][CssClass("disabled")]public bool Disabled { get; set; }
}
```
