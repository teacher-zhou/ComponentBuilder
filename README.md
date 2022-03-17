## ComponentBuilder
基于 `RenderTreeBuilder` 快速且简单地创建 Blazor 组件库。

[中文](Readme.md) | [English](Readme.en-us.md)

## :sparkles: 特点
* 支持基于 `RenderTreeBuilder` 快速编写组件
* 支持对 CSS 类、HTML 属性的参数化特性
* 内置通用组件的基本行为、事件
* 按需加载行为、属性、参数
* 动态调用 js 函数

## :rainbow: 示例

```csharp
[HtmlTag("button")]
[CssClass("btn")]
public class MyButton : BlazorComponentBase, IHasChildContent
{
	[Parameter][CssClass("active")]public bool Active { get; set; }
	
	[Parameter][CssClass("btn-")]public Color? Color { get; set; }

	[Parameter]public RenderFragment? ChildContent { get; set; }
}

public enum Color
{
	Primary,
	Secondary,
	[CssClass("info")]Information,
}
```
```html
<!--使用组件-->
<MyButton Color="Color.Primary">Submit</MyButton>
<MyButton Active>Active Button</MyButton>

<!--HTML 渲染-->
<button class="btn btn-primary">Submit</button>
<button class="btn active">Active Button</button>
```

## :computer: 支持环境
* .NET 5
* .NET 6

## :blue_book: 安装使用

* 从 Nuget.org 安装
```cmd
Install-Package ComponentBuilder
```

* 注册服务
```csharp
services.AddComponentBuilder();
```

## :link: 链接地址
* [问题反馈](/issues)
* [版本发布](/releases)
* [文档地址](/wiki/zh-cn/readme.md)