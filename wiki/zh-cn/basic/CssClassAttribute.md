## `CssClassAttribute`
> 应用于参数、类、接口、枚举的 CSS 类特性

### Demo
```csharp
[HtmlTag("button")]
[CssClass("btn")]
public class Button : BlazorChildContentComponentBase
{
    [Parameter][CssClass("btn-")]public Color? Color { get; set; }
    [Parameter][CssClass]public bool Disabled { get; set; }
    [Parameter][CssClass("m-")]public int? Margin { get; set; }
}

public enum Color
{
    [CssClass("")]Default,
    Primary,
    Secondary,
    Info,
    [CssClass("warning")]Warn,
    Danger,
    Success
}
```
```html
<!--组件使用-->
<Button Color="Color.Primary" Disabled Margin="3">Submit</Button>

<!--HTML 生成-->
<button class="btn btn-primary disabled m-3">Submit</button>
```

#### 应用于组件类
表示组件必须具备的 class 名称

```cs
[CssClass("myclass")]
public class MyComponent : BlazorComponentBase
{

}
```

#### 应用于参数
和参数的值共同组成 class 名称，默认是前缀。

```cs
public class MyComponent : BlazorComponentBase
{
    [Parameter][CssClass("p")]public int? Padding { get; set; }
}
```

支持 `int` `bool` `decimal` `double` `float` `short` `long` `string` `enum` 和可空类型。

> 如果是引用类型，当参数值不是 `null` 时则会应用参数的配置


##### bool 类型的参数
当 bool 类型的参数被设置，则应用 class 类名称
```csharp
public class MyComponent : BlazorComponentBase
{
    [Parameter][CssClass("disabled")]public bool Disabled { get; set; }
}
```
```html
<MyComponent Disabled />

<div class="disabled"></div>
```

**强烈建议使用可空类型，如 `int?` ，否则会有默认值，如下:**
```csharp
public class MyComponent : BlazorComponentBase
{
    [Parameter][CssClass("p-")]public int Padding { get; set; } //默认值是0

    [Parameter][CssClass("m-")]public int? Margin { get; set; } //null 不生成 class
}
```
```html
<MyComponent />

<div class="p-0"></div>
```

##### enum 类型的参数
枚举类型作为参数，默认使用枚举项名称作为 class 名称。但可以使用 `CssClassAttribute` 重命名 class 名称
```cs
public class State
{
    Default,
    [CssClass("sm")]Small,
    [CssClass("lg")]Big
}

public class MyComponent : BlazorComponentBase
{
    [Parameter][CssClass("com-")]public State? State { get; set; }
}
```
```html
<MyComponent State="State.Small" />

<div class="com-sm"></div>
```

##### 排序
`Order` 属性可定义生成的 class 名称顺序，从小到大排列。
```cs
[CssClass("myclass", Order = 5)]
public class MyComponent : BlazorComponentBase
{
    [CssClass(Order = 10)]public bool Active { get; set; }
    [CssClass("m", Order = 4)]public int Margin { get; set; }
}
```
```html
<MyComponent Margin="3" Active />

<div class="m4 myclass active"></div>
```

##### 禁用特性功能
```cs
[Parameter][CssClass(Disabled = true)]public bool Active { get; set; }
```