# ComponentBuilder
分分钟就能创建属于自己的 Blazor 组件库。

## :sparkles: 特点
* 基于 `RenderTreeBuilder` 的动态组件
* 参数特性化，完成参数即能完成组件的 CSS、事件的定义
* 可无限扩展自己的组件特色
* 支持 JS 模块的动态调用

## :rainbow: 创建组件
```csharp
[HtmlTag("button")]
[CssClass("btn")]
public class MyButton : BlazorComponentBase, IHasChildContent, IHasOnClick
{
	[Parameter][CssClass("active")]public bool Active { get; set; }
	
	[Parameter][CssClass("btn-")]public Color? Color { get; set; }

	[Parameter]public RenderFragment? ChildContent { get; set; }

	[Parameter][HtmlData("tooltip")]public string? Tooltip { get; set; }

	[Parameter][HtmlEvent("onclick")]public EventCallback<ClickEventArgs> OnClick { get; set; }
}

public enum Color
{
	Primary,
	Secondary,
	[CssClass("info")]Information,
}
```
```html
<!--razor-->
<MyButton Color="Color.Primary">Submit</MyButton>

<!--html-->
<button class="btn btn-primary">Submit</button>


<!--razor-->
<MyButton Active Tooltip="active button" Color="Color.Information">Active Button</MyButton>

<!--html-->
<button class="btn btn-info active" data-tooltip="active button">Active Button</button>
```

## :key: 动态 JS
```js
//保存在 app.js 中
export function display(){
	// 你的代码
}
```
```cs
[Inject]IJSRuntime JS { get;set; }

var js = await JS.Import("./app.js");
js.display();
```

## :large_blue_circle: 创建元素
```cs
protected override void BuildRenderTree(RenderTreeBuilder builder)
{
    builder.CreateElement(0, "div", ChildContent, new { @class = "my-class" });
}
```

## :large_orange_diamond: 创建组件
```cs
protected override void BuildRenderTree(RenderTreeBuilder builder)
{
    builder.CreateComponent<Button>(0, ChildContent, new { @class = "my-class" });
}
```
## :children_crossing: 父子组件
* 父组件
	```cs
	[ParentComponent]
	[HtmlTag("ul")]
	public class List : BlazorComponentBase, IHasChildContent
	{

	}
	```
* 子组件
	```cs
	[ChildComponent(typeof(List))] //强制关联到 List 组件
	[ChildComponent(typeof(Menu), Optional = true)] //不强制关联
	[HtmlTag("li")]
	public class ListItem : BlazorComponentBase
	{
		//一定要设置为 public
		[CascadingParameter]public List CascadingList { get; set; }

		// 如果是 Optional, 允许组件可为空类型
		[CascadingParameter]public Menu? CascadingMenu { get; set; }
	}
	```

## :computer: 支持环境
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
* [文档地址](/wiki/readme.md)