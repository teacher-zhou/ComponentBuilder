## 重写组件树
> 复杂组件需要利用组件树来渲染

**什么时候需要重写组件树？**
1. 参数不变，但是组件的 HTML 生成复杂；
2. 保留原有的组件公共特性，比如公共参数等；

**重写 AddContent 方法**

比如 HTML 生成是这样的
```html
<div class="header-wrapper">
    <span class="header-content">
        ...
    </span>
</div>
```
希望组件使用如下：
```html
<Header>....</Header>
```

组件开发代码：
```csharp
[CssClass("header-wrapper")]
public class Header : BlazorComponentBase, IHasChildContent
{
    [Parameter]public RenderFragment? ChildContent { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "span", ChildContent);
    }
}
```