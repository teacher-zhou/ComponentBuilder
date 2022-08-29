# 扩展方法

## RenderTreeBuilder 的扩展

### CreateElement
> 用于创建元素的组件。

传统方式：
```cs

protected override void BuildRenderTree(RenderTreeBuilder builder)
{
    builder.OpentElement(0, "div");
    builder.AddAttribute(1, "class", "my-class");
    builder.AddContent(2, ChildContent);
    builder.CloseElement();
}
```
使用扩展方法：
```cs
protected override void BuildRenderTree(RenderTreeBuilder builder)
{
    builder.CreateElement(0, "div", ChildContent, new { @class = "my-class" });
}
```

### CreateComponent
> 用于创建指定的组件。
传统方式：
```cs
protected override void BuildRenderTree(RenderTreeBuilder builder)
{
    builder.OpenComponent<Button>(0);
    builder.AddAttribute(1, "class", "my-class");
    builder.AddContent(2, ChildContent);
    builder.CloseComponent();
}
```
使用扩展方法：
```cs
protected override void BuildRenderTree(RenderTreeBuilder builder)
{
    builder.CreateComponent<Button>(0, ChildContent, new { @class = "my-class" });
}
```

**第三个参数：attributes** 支持组件参数或 HTML 属性
* HTML 属性
    * 静态值
    ```cs
    builder.CreateElement(0,"span", attributes: new { @class="my-class", style="font-size:16px"});
    ```
    ```html
    <span class="my-class" style="font-size:16px"></span>
    ```
    * 使用 `HtmlHelper` 动态创建属性值
    ```cs
    builder.CreateElement(0,"span", attributes: new 
    { 
        @class = HtmlHelper.CreateCssBuilder().Append("my-class").Append("active", Active), 
        style = HtmlHelper.CreateStyleBuilder().Append("font-size:16px", Active),

        //使用 CreateCallback 创建组件事件
        onclick = HtmlHelper.CreateCallback(this, () => ...)
    });
    ```
* 组件属性
    ```cs
    builder.CreateComponent<Button>(0, ChildContent, new { Active, Disabled, Name = this.FileName });
    ```
    **不能使用组件中不存在的参数**