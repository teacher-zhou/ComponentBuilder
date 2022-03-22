## 重写组件树
> 复杂组件需要利用组件树来渲染


**尽量重写 `BuildComponentRenderTree` 而不是 `BuildRenderTree` 方法，除非你完全理解内部特性**

```csharp
[HtmlTag("button")]
public class MyComponent : BlazorComponentBase
{
    protected override void BuildComponentRenderTree(RenderTreeBuilder builder)
    {
        //保证 HtmlTagAttribute 可用
    }
}
```
源码：
```cs
protected override void BuildRenderTree(RenderTreeBuilder builder)
{
    builder.OpenRegion(RegionSequence);
    builder.OpenElement(0, TagName ?? "div");
    BuildComponentRenderTree(builder);
    builder.CloseElement();
    builder.CloseRegion();
}

protected virtual void BuildComponentRenderTree(RenderTreeBuilder builder)
{
    BuildComponentAttributes(builder, out var sequence);
    AddContent(builder, sequence + 2);
}
```

#### BuildCommonAttributes
构造通用的属性到 `RenderTreeBuilder` 渲染树中，用于将解析器的内容构造到指定的 `RenderTreeBuilder` 对象
```csharp
[CssClass("myclass")]
public class MyComponent : BlazorComponentBase, IHasChildContent
{
    [Parameter][CssClass("Disabled")]public bool Disabled { get; set; }

    [Parameter]public RenderFragment? ChildContent { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "div");
        builder.AddContent(1, (RenderFragment)(content =>
        {
            content.OpenElement(0,"span");
            BuildCommonAttributes(content, out var sequence); // 构造 ComponentBuilder 的特性，包括 CSS, style html 属性等
            content.AddContent(sequence + 1, ChildContent);
            content.CloseElement();
        }));
        builder.CloseElement();
    }
}
```
```html
<MyComponent Disabled >My Component</MyComponent>

<div><span class="myclass disabled">My Component</span></div>
```

源码：
```csharp
protected virtual void BuildComponentAttributes(RenderTreeBuilder builder, out int sequence)
{
    AddClassAttribute(builder, 1);
    AddStyleAttribute(builder, 2);
    AddMultipleAttributes(builder, sequence = 3);
}
```
输出参数是渲染树的最后一个 `sequence` 的序号

#### AddContent
如果继承了接口 `IHasChildContent` 则会自动调用 `builder.AddContent(sequence, ChildContent)` 方法

> 重写该方法用于构建子组件，而不改父组件的内置配置

