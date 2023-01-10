# ComponentBuilder

An automation framework to help you build Blazor component libary easier and faster.

[中文介绍](README.zh-cn.md) | [Document](https://github.com/AchievedOwner/ComponentBuilder/wiki) |![Latest Version](https://img.shields.io/github/v/release/AchievedOwner/ComponentBuilder) |![.net6](https://img.shields.io/badge/.net-6-green)|![.net7](https://img.shields.io/badge/.net-7-green)

## :sparkles: Features

* Easy and automation build parameters for component
* Easy to customize and personalize component building
* Easy to build a flexible dynamic component structure
* Easy interoption between code and javascript
* Modular implementation for automation of component building
* Strong extensions and utilities of RenderTreeBuilder
* Other automations...




## :rainbow: Component Definition

* In `Button.razor` file
```html
@inherits BlazorComponentBase

<button @attributes="AdditionalAttributes">
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

	[Parameter][HtmlEvent("onclick")]public EventCallback<ClickEventArgs> OnClick { get; set; } 

    [Parameter][HtmlAttribute]public string? Title { get; set; }
    
    public enum Color
    {
	    Primary,
	    Secondary,
	    [CssClass("info")]Information,
    }
}
```

* In `Button.cs` component class for full automation features
```csharp
[HtmlTag("button")]
[CssClass("btn")]
public class Button : BlazorComponentBase, IHasChildContent, IHasOnClick
{
	[Parameter][CssClass("active")]public bool Active { get; set; } 
	
	[Parameter][CssClass("btn-")]public Color? Color { get; set; } 

	[Parameter]public RenderFragment? ChildContent { get; set; }

	[Parameter][HtmlData("tooltip")]public string? Tooltip { get; set; }

	[Parameter][HtmlEvent("onclick")]public EventCallback<ClickEventArgs> OnClick { get; set; 

    [Parameter][HtmlAttribute]public string? Title { get; set; }
}

public enum Color
{
	Primary,
	Secondary,
	[CssClass("info")]Information,
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


## :key: Interoption between C# code and JS

* Import modules
```js
//in app.js
export function display(){
 // ...your code
}
```

```csharp
[Inject]IJSRuntime JS { get; set; }

var js = await JS.Value.ImportAsync("./app.js");
js.display(); // same as function name
```

* Evaluate js string
```csharp
JS.Value.EvaluateAsync(window => {
    window.console.log("log")
});

JS.Value.EvaludateAsync(@"
    console.log(\"log\");
")
```

## :information_source: Logical CSS/Style/Attributes
* Logical CSS
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
```
* Logical Attributes
```csharp
protected override void BuildAttributes(IDictionary<string, object> attributes)
{
    attributes["onclick"] = HtmlHelper.Event.Create(this, ()=>{ ... });
    
    if(attrbutes.ContainKey("data-toggle"))
    {
        attributes["data-toggle"] = "collapse";
    }
}
```
## :palm_tree: RenderTreeBuilder Extensions
* Create Element
```csharp
protected override void BuildRenderTree(RenderTreeBuilder builder)
{
    builder.Open("div")
            .Class("my-class", (IsActive, "active"), (!string.IsNullOrEmpty(Name), "text-block"))
            .Style((Size.HasValue, $"font-size:{Size}px"))
            .Content("hello world")
           .Close();

    builder.CreateElement(10, "span", "hello", attributes: new { @class = "title-span"});

}
```

* Create Component

```csharp
protected override void BuildRenderTree(RenderTreeBuilder builder)
{
    builder.Open<Button>()
            .Class("my-class", (IsActive, "active"), (!string.IsNullOrEmpty(Name), "text-block"))
            .Style((Size.HasValue, $"font-size:{Size}px"))
            .Content(ChildContent)
           .Close();

    builder.CreateComponent<NavLink>(0, "Home", new { NavLinkMatch = NavLinkMatch.All, ActiveCssClass = "nav-active" })
}
```

## :children_crossing: Component Association
### In .razor file
* For `List.razor` file be parent component
```html
<ul @attributes="AdditionalAttributes">
    <CascadingValue Value="this">
        @ChildContent
    </CascadingValue>
</ul>
```

* For `ListItem.razor` file be child of `List.razor` component
```html
<li @attributes="AdditionalAttributes">@ChildContent</li>

@code{
    [ChildComponent(typeof(List))]
    public ListItem()
    {
    }

    [CascadingParameter] public List CascadedList { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }
}
```

### In RenderTreeBuilder
* For `List` component class
```csharp
[ParentComponent] //be cascading parameter for this component
[HtmlTag("ul")]
public class List : BlazorComponentBase, IHasChildContent
{

}
```
* For `ListItem` component class
```cs
[ChildComponent(typeof(List))] //Strong association with List
[HtmlTag("li")]
public class ListItem : BlazorComponentBase, IHasChildContent
{
    [CascadingParameter]public List CascadedList { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }
}
```

### Use in blazor

```html
<List>
    <ListItem>...</ListItem>
</List>

<ListItem /> <!--throw exception because ListItem must be the child component of List coponent witch defined ChildComponentAttribute in ListItem-->

```

## :six_pointed_star: HtmlHelper

* in `.razor` file

```html
<div class="@GetCssClass">
...
</div>
```

```csharp
@code{
string GetCssClass => HtmlHelper.Class.Append("btn-primary").Append("active", Actived).ToString();
    
[Parameter] public bool Actived { get; set; }
}
```

* Dynamic element attribute

```cs
builder.CreateElement(0, "span", attributes: 
    new { 
            @class = HtmlHelper.Class
                                .Append("btn-primary")
                                .Append("active", Actived),
            style = HtmlHelper.Style.Append($"width:{Width}px"),
            onclick = HtmlHelper.Event.Create<MouseEventArgs>(this, e=>{ //...click... });
        });
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
    configure.Interceptors.Add(new LogInterceptor());
})
```

![BlazorComponentBase Lifecycle](./asset/BlazorComponentBaseLifecycle.png)

## :desktop_computer: Environment

![.net6](https://img.shields.io/badge/.net-6-green)
![.net7](https://img.shields.io/badge/.net-7-green)


## :blue_book: Installation Guide

* Install from `Nuget.org`

```bash
Install-Package ComponentBuilder
```

* Register service

```csharp
builder.Services.AddComponentBuilder();
```


## :pencil: Component Library Solution Template
Use `ComponentBuilder.Templates` to generate a razor component library solution and online demo site
```bash
dotnet new install ComponentBuilder.Templates
dotnet new blazor-sln -n {YourRazorLibraryName}
```
More information see [templates](./templates/readme.md)