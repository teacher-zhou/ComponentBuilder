## `HtmlAttributeAttribute`
应用于组件类、参数和字段，生成 HTML 元素的属性

### Demo
```cs
[HtmlAttrbute("role","anchor")]
public class Anchor : BlazorChildContentComponentBase
{
    [Parameter][HtmlAttribute("href")]public string Link { get; set; }
    [Parameter][HtmlAttribute]public AnchorTarget? Target { get; set; }
    [Parameter][HtmlAttribute("title","link")]public bool ShowTip { get; set; }
}

public class AnchorTarget
{
    [HtmlAttribute("_blank")]Blank,
    [HtmlAttribute("_self")]Self,
    [HtmlAttribute("_parent")]Parent,
}
```
```html
<Anchor Link="http://www.bing.com" Target="AnchorTarget.Blank" ShowTip>Link</Anchor>

<div role="anchor" href="http://www.bing.com" target="_blank" title="link">Link</div>
```

##### 应用于组件类
创建一个指定的属性名称和值

```CS
[HtmlAttribute("name","value")] //固定 name=value
public class MyComponent  : BlazorComponentBase
{
    //...
}
```

**使用 `HtmlRole` 代替 `HtmlAttribute("role", 值)`**

```CS
[HtmlAttribute("role","value")]
public class MyComponent  : BlazorComponentBase
{    
}

//等价于

[HtmlRole("value")]
public class MyComponent  : BlazorComponentBase
{
}
```

##### 应用于参数
支持 `int` `bool` `decimal` `double` `float` `short` `long` `string` `enum` 和可空类型。

> 如果是引用类型，当参数值不是 `null` 时则会应用参数的特性配置

* bool 类型
当 `true` 时，应用特性配置
```cs
public class MyComponent  : BlazorComponentBase
{    
     [Parameter][HtmlAttribute("data","active")]public bool Active { get; set; }
}
```
```html
<MyComponent Active />

<div data="active"></div>
```
****


* 其他类型
参数值则会作为属性值使用
```cs
public class Anchor : BlazorChildContentComponentBase
{
    //属性名自定义
    [Parameter][HtmlAttribute("href")]public string Link { get; set; }

    //属性名=参数名
    [Parameter][HtmlAttribute]public string Target { get; set; }
}
```

### 其他常用的 `HtmlAttribute`
* `HtmlRoleAttribute`
同等于 `HtmlAttribute("role", value)`，应用于 `class`
    ```csharp
    [HtmlRole("nav")]
    public class MyComponent : BlazorComponentBase
    {
    }

    //<div role="nav"></div>
    ```


* `HtmlDataAttribute`
  同等于 `HtmlAttribute("data-{parameter}", value)`，应用 **参数**

    ```csharp
    //使用参数名称作为 data-{name} 的名称
    [Parameter][HtmlData]public string Name { get; set; } //data-name="{value}"

    //自定义 data-{name} 的名称
    [Parameter][HtmlData("title")]public string Tip { get; set; } //data-title="{value}"

    [Parameter][HtmlData]public int? Height { get; set; } //data-height="{value}"
    ```

    **bool 类型特殊处理**
    ```csharp
    [Parameter][HtmlData]public bool Drop { get; set; } //data-drop="drop"

    [Parameter][HtmlData("drop")]public bool Drag { get; set; } //data-drop="drag"
    ```

* `HtmlEventAttribute`
    可以生成事件属性，例如 `onclick` `onfocus` 等，要求参数必须是 `EventCallback` 或 `EventCallback<TValue>` 类型

    ```csharp
    [HtmlTag("button")]
    public class Button : BlazorComponentBase
    {
        [Parameter][HtmlEvent("onclick")]public EventCallback<MouseEventArgs> OnClick { get; set; }
    }
    ```