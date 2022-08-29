# BuildStyle 方法
用于利用逻辑代码动态加载 `style` 样式

```csharp
protected override void BuildStyle(IStyleBuilder builder)
{
    builder.Append("display:block")
            .Append("width","100px")
}
```