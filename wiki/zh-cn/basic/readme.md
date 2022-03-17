# 基础应用

#### BlazorComponentBase
让组件继承自 `BlazorComponentBase` 你就完成了 **50%** 的组件创建工作。

```csharp
public class MyComponent : BlazorComponentBase
{
    //即使是空的，你也成功地创建一个了组件，默认是 div 元素
}
```
```html
<MyComponent />

<div></div>
```

#### BlazorChildContentComponentBase
继承基类 `BlazorChildContentComponentBase` 你就可以在组件内部写任意内容了

```csharp
public class MyComponent : BlazorChildContentComponentBase
{
    //即使是空的，你也成功地创建一个了组件，默认是 div 元素
}
```
```html
<MyComponent>任意内容</MyComponent>

<div>任意内容</div>
```
或者继承 `IHasChildContent` 接口
```csharp
public class MyComponent : BlazorComponentBase, IHasColdContent
{
    [Parameter]public RederFragment? ChildContent { get; set; }
}
```

#### HTML 标记定义
用于输出指定名称的 HTML 元素名称。
* 组件上使用 `HtmlTagAttrbute` 特性
```csharp
[HtmlTag("a")]
public class MyComponent : BlazorComponentBase
{

}

```
* 重写 `TagName` 属性
```csharp
public class MyComponent : BlazorComponentBase
{
    protected override string TagName => "a";
}
```
这种方式就可以做成动态组件

[参阅更多 >>](HtmlTag.md)


#### CSS 类在参数中的应用
通过 `CssClassAttribute` 可以根据组件参数的设置应用 CSS 类
* 应用到组件类
```csharp
[CssClass("myclass")]
public class MyComponent : BlazorComponentBase
{
}
```
```html
<MyComponent />

<div class="myclass"></div>
```

* 应用到参数
```csharp
public class MyComponent : BlazorComponentBase
{
    [Parameter]
    [CssClass] //使用属性名的小写作为 css 类，即 active
    public bool Active { get; set; } 

    [Parameter]
    [CssClass("hello")]//使用自定义名称
    public bool Disabled { get; set; }
}
```
```html
<MyComponent Active Disabled/>

<div class="active hello"></div>
```

[参阅更多 >>](CssClassAttribute.md)

#### HTML 属性在参数中的应用
使用 `HtmlAttributeAttribute` 将参数生产 HTML 元素中的属性

* 应用到组件类
```csharp
[HtmlAttribute("role","nav")]
public class MyComponent : BlazorComponentBase
{
}
```
```html
<MyComponent />

<div role="nav"></div>
```
* 应用到参数
参数名称将作为属性的 name 或自定义名称

```csharp
public class MyComponent : BlazorComponentBase
{
    [Parameter]
    [HtmlAttribute]
    public string Data { get; set; } 
    
    [Parameter]
    [HtmlAttribute("id")]
    public int UniqueId { get; set; } 
}
```
```html
<MyComponent Data="toggle" UniqueId="5" />

<div data="toggle" id="5"></div>
```
[参阅更多 >>](HtmlAttributeAttribute.md)