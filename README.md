# ComponentBuilder

The best automation framework for RCL(Razor Component Library) to help you easy and quickly building your own Razor Component Library

[中文介绍](README.zh-cn.md) | [Quick Start](./docs/readme.md) | [Document](https://playermaker.gitbook.io/componentbuilder/english/introduction)

![Latest Version](https://img.shields.io/github/v/release/AchievedOwner/ComponentBuilder)

v4.x supports
![.net6](https://img.shields.io/badge/.net-6-blue)
![.net7](https://img.shields.io/badge/.net-7-blue)

v5.x supports
![.net8](https://img.shields.io/badge/.net-8-blue)

## :sparkles: Features
* OOP mindset creating component
* Attribute first, easy define CSS from parameters
* Easy to associate with components via Attributes
* Cusomization CSS and attributes of component by coding logic
* Support `Pre-definition` for components with simular parameters
* New lifecycle definition of Component with interceptor design pattern
* Renderer pipeline pattern to regonize dynamic render of components


## :rainbow: Quick Start

**Only change `ComponentBase` to `BlazorComponetBase` for derived component class**

* Sample to create a button component with C# class:
```csharp
[HtmlTag("button")] //define HTML element tag
[CssClass("btn")] //define component necessary CSS class
public class Button : BlazorComponentBase, IHasChildContent, IHasOnClick
{
	[Parameter][CssClass("active")]public bool Active { get; set; } 	
	[Parameter][CssClass("btn-")]public Color? Color { get; set; } 
	[Parameter]public RenderFragment? ChildContent { get; set; }
	[Parameter][HtmlData("tooltip")]public string? Tooltip { get; set; }
	[Parameter][HtmlAttribute("onclick")]public EventCallback<ClickEventArgs> OnClick { get; set; 
	[Parameter][HtmlAttribute]public string? Title { get; set; }
}

public enum Color
{
	Primary,
	Secondary,
	[CssClass("info")]Information,
}
```
OR you also can define most part of automation features in razor file:

```cshtml
@inherits BlazorComponentBase

<!--Bind GetAttributes() for @attributes to getting automation features-->

<button @attributes="@GetAttributes()"> 
	@ChildContent
</button>

@code{
	[CssClass("btn")]
	public Button()
	{
	}

	[Parameter][CssClass("active")]public bool Active { get; set; } 	
	[Parameter][CssClass("btn-")]public Color? Color { get; set; } 
	[Parameter]public RenderFragment? ChildContent { get; set; } 
	[Parameter][HtmlData("tooltip")]public string? Tooltip { get; set; }
	[Parameter][HtmlAttribute("onclick")]public EventCallback<ClickEventArgs> OnClick { get; set; } 
	[Parameter][HtmlAttribute]public string? Title { get; set; }
	
	public enum Color
	{
		Primary,
		Secondary,
		[CssClass("info")]Information,
	}
}
```

* Use component
```html
<!--razor-->
<Button Color="Color.Primary">Submit</Button>
<!--html-->
<button class="btn btn-primary">Submit</button>

<!--razor-->
<Button Active Tooltip="active button" Color="Color.Information" Title="click me">Active Button</Button>
<!--html-->
<button class="btn btn-info active" data-tooltip="active button" title="click me">Active Button</button>
```


## :information_source: Logical CSS/Style/Attributes
* Using different logic for groups creates code with OOP mindset

```csharp
protected override void BuildCssClass(ICssClassBuilder builder)
{
	if(builder.Contains("annotation-enter"))
	{
		builder.Remove("annotation-exist");
	}
	else
	{
		builder.Append("annotation-enter").Append("annotation-exist");
	}
}

protected override void BuildStyle(IStyleBuilder builder)
{
	if(Height.HasValue)
	{
		builder.Append($"height:{Height}px");
	}
}

protected override void BuildAttributes(IDictionary<string, object> attributes)
{	
	if(attrbutes.ContainKey("data-toggle"))
	{
		attributes["data-toggle"] = "collapse";
	}
}
```

## :children_crossing: Parent/Child component

Easy to create parent/child pair components using `ParentComponentAttribute` and `ChildComponentAttribute<TParent>`

* For `List` component class
```csharp
[ParentComponent] //auto creating the cascading parameter for current 
[HtmlTag("ul")]
public class List : BlazorComponentBase, IHasChildContent
{

}
```
* For `ListItem` component class
```cs
[ChildComponent<List>] //Strong association with List
[HtmlTag("li")]
public class ListItem : BlazorComponentBase, IHasChildContent
{
	[CascadingParameter]public List? CascadedList { get; set; }//Auto getting the instance of cascading parameter

	[Parameter] public RenderFragment? ChildContent { get; set; }
}
```
### Use in blazor
```html
<List>
	<ListItem>...</ListItem>
</List>

<ListItem /> <!--throw exception because ListItem must be the child component of List coponent-->

```

### In .razor file

In razor file component, you should create cascading parameter by yourself

`List.razor`:
```html
<ul @attributes="@GetAttributes()">
	<CascadingValue Value="this">
		@ChildContent
	</CascadingValue>
</ul>
```

`ListItem.razor`:
```html
<li @attributes="GetAttributes()">@ChildContent</li>

@code{
	[ChildComponent<List>]
	public ListItem()
	{
	}

	[CascadingParameter] public List? CascadedList { get; set; }

	[Parameter] public RenderFragment? ChildContent { get; set; }
}
```

## :crossed_swords: Interceptors
You can intercept the lifecycle of component

* Define an interceptor
```csharp
public class LogInterceptor : ComponentInterceptorBase
{
	private readonly ILogger<LogInterceptor> _logger;
	public LogInterceptor(ILogger<LogInterceptor> logger)
	{
		_logger = logger;
	}

	//Run in SetParameterAsync method is called
	public override void InterceptSetParameters(IBlazorComponent component, ParameterView parameters)
	{
		foreach(var item in parameters)
		{
			_logger.LogDebug($"Key:{item.Name}, Value:{item.Value}");
		}
	}
}
```
* Register interceptor
```csharp
builder.Services.AddComponentBuilder(configure => {
	configure.Interceptors.Add<LogInterceptor>();
})
```

![BlazorComponentBase Lifecycle](./asset/BlazorComponentBaseLifecycle.png)

### Why interceptors?
Follow SOLID pricipal when designing a component. So you no need break the lifecycle or using override any protected method such as `OnParameterSet` to create new HTML attribute whatever you want.


## :recycle: Renderer Pipeline
Recognize special case to render specified component
```csharp
public class NavLinkComponentRender : IComponentRender
{
	public bool Render(IBlazorComponent component, RenderTreeBuilder builder)
	{
		if ( component is IHasNavLink navLink )
		{
			builder.OpenComponent<NavLink>(0);
			builder.AddAttribute(1, nameof(NavLink.Match), navLink.Match);
			builder.AddAttribute(2, nameof(NavLink.ActiveClass), navLink.ActiveCssClass);
			builder.AddAttribute(3, nameof(NavLink.ChildContent), navLink.ChildContent);
			builder.AddMultipleAttributes(4, component.GetAttributes());
			builder.CloseComponent();
			return false;
		}
		return true;
	}
}
```
* Register renderer in configuration
```csharp
builder.Services.AddComponentBuilder(configure => {
	configure.Renderers.Add<NavLinkComponentRenderer>();
});
```


## :blue_book: Installation Guide

* Install from `Nuget.org`

```bash
Install-Package ComponentBuilder
```

* Register service

```csharp
builder.Services.AddComponentBuilder();

//configure costomization such as Interceptors
builder.Services.AddComponentBuilder(configure => {
	//...
})
```

[Read document for more informations](https://playermaker.gitbook.io/componentbuilder)


## :toolbox: Other extension packages
* ComponentBuilder.Extensions
> Enhancement for `ComponentBuilder`, auto install by default when installing `ComponentBuilder`

```cs
builder.CreateElement();
builder.CreateComponent<MyComponent>();
builder.CreateCascadingValue<T>(value);

//create dynamic css class string
HtmlHelper.Instance.Class().Append("class1").Append("disabled", Disabled).ToString();

//create dynamic style string
HtmlHelper.Instance.Style().Append($"width:{Width}px").Append($"height:{Height}px", Height.HasValue).ToString();

//create dynamic EventCallback
HtmlHelper.Instance.Callback().Create(this, ()=>{ //action for callback });
```
* ComponentBuilder.FluentRenderTreeBuilder
> Write RenderTreeBuilder as fluent API
```cmd
> Install-Package ComponentBuilder.FluentRenderTreeBuilder
```

```cs
builder.Element("p", "default-class") // create <p> element with default class
		.Class("hover", Hoverable) //append class if Hoverable parameter is true
		.Attribute("disabled", Disabled) // add HTML attribute if Disabled is true
		.Data("trigger", String) // add data-trigger="string" HTML attribute if String parameter not empty
		.Callback<MouseEventArgs>("onmouseover", this, e => {  }) // add event with onmouseover with a delegate code
		.Content("content text") // add inner text for this element
	.Close()

//HTML element generate like:
<p class="default-class">content text</p>

// normally in razor file:
<p class="default-class @(Hoverable?"hover":"")" disabled="@Disabled" data-trigger="@String" @onmouseover="@(e=>{ })">content text</p>


builder.Component<MyComponent>()
		.Parameter(m => m.Disabled, true)
		.Parameter(m => Size, 5)
		.ChildContent("My name is hello world")
	.Close();

```
* ComponentBuilder.JSInterop
> Interactive with C# and JS

```cmd
> Install-Package ComponentBuilder.JSInterop
```

```js
export function sayHello(){
	//...
}

export function getClient(){
	//..
	return name;
}
```

```cs
//Import js module

@inject IJSRuntime JS

var module = JS.ImportAsync("./module.js");

await module.Module.InvokeVoidAsync("sayHello");
var name = await module.Module.InvokeAsync<string>("getClient");
```



## :pencil: Component Library Solution Template
Use `ComponentBuilder.Templates` to generate a razor component library solution and online demo site
```bash
dotnet new install ComponentBuilder.Templates
dotnet new blazor-sln -n {YourRazorLibraryName}
```
More information see [templates](./templates/readme.md)