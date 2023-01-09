## Quick Start
**仅需 2 步即可创建自动化组件**

1. 继承 `BlazorComponentBase` 而不是 `ComponentBase`
2. **对于“.razor”文件组件**必须为具有指定 HTML 元素的自动化功能添加`@attributes="Additional Attributes"`属性
2. 为组件或参数定义 `CssClassAttribute` 特性

## `.razor` 文件的例子
* 创建 `Element.razor` 文件
```html
@inherits BlazorComponentBase

<span @attributes="AdditionalAttributes"> <!--@attributes 是必须的-->
	@ChildContent
</span>

@code{
	[Parameter] public RenderFragment? ChildContent { get; set; }

	[Parameter][CssClass("active")] public bool Active { get; set; }
}
```
* 在 razor 中执行
```html
<Element>Hello</Element>
<span>Hello</span>

<Element Active>Active Hello</Element>
<span class="active">Active Hello</span>
```

## 使用 `Element.cs` 类
* 创建 `Element` 类
```csharp
[HtmlTag("span")]
public class Element : BlazorComponentBase, IHasChildContent
{
	[Parameter] public RenderFragment? ChildContent { get; set; }

	[Parameter][CssClass("active")] public bool Active { get; set; }
}
```
* 在 razor 中执行
```html
<Element>Hello</Element>
<span>Hello</span>

<Element Active>Active Hello</Element>
<span class="active">Active Hello</span>
```

> 如您所见，使用组件类可以完全自动化组件，但缺点是它需要一定的具体思维，尤其是在处理复杂组件时。当然，您可以将两者混合使用，但只要能提高效率，就是个好主意。