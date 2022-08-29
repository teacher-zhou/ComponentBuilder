## HtmlTag
定义组件成的 HTML 元素名称

### 使用 `HtmlTagAttribute`
在组件定义 `HtmlTag` 特性
```csharp
[HtmlTag("span")]
public class MyComponent : BlazorComponentBase
{

}
```
```html
<MyComponent />

<span />
```

### 重写 `TagName` 属性
```csharp
public class MyComponent : BlazorComponentBase
{
    protected override string TagName
    {
        get
        {
            if(xxxxx)
            {
                return "a";
            }
            return "span";
        }
    }
}
```

**如果什么都不写，默认是 `div` 元素**