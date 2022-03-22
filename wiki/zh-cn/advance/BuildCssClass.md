# 重写 BuildCssClass 方法
> 当组件具备逻辑条件才能按需加载相应的 CSS 类时重写 `BuilderCssClass` 方法

```csharp
protected override void BuildCssClass(ICssClassBuilder builder)
{
    builder.Append("myclass") //总是添加
        .Append("hide", Hide); //当 Hide = true 时添加 CSS
}
```

**阻止重复的 CSS 类**

```csharp
public class MyComponent : BlazorComponentBase
{
    [Parameter][CssClass("readonly")]public bool ReadOnly { get; set; }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("myclass")
            .Append("readonly", ReadOnly);  //重复，不加入
    }
}
```