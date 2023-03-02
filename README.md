# ComponentBuilder

Easy to create a Blazor Component Library with automation features supports both razor file way and RenderTreeBuilder way.

[中文介绍](README.zh-cn.md) | [Quick Start](./docs/readme.md) | [Document](https://playermaker.gitbook.io/componentbuilder/english/introduction)

![Latest Version](https://img.shields.io/github/v/release/AchievedOwner/ComponentBuilder)
![.net6](https://img.shields.io/badge/.net-6-blue)
![.net7](https://img.shields.io/badge/.net-7-blue)

## :sparkles: Features

* Attribute first, easy define CSS from parameters
* Easy to associate with components via Attributes
* Cusomization CSS and attributes of component by coding logic
* Both supports `.razor` file or `RenderTreeBuilder` to create component
* Support `Pre-definition` for components with simular parameters
* Dynamic JS interoption
* New lifecycle definition of Component with interceptor design pattern
* Renderer pipeline pattern to regonize dynamic render of components
* More extensions for `RenderTreeBuilder` instance
* Create element with Fluent API

## :rainbow: Quick Start

* In `Button.razor` file
```html
@inherits BlazorComponentBase

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


## :key: JS Interoption

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
## :palm_tree: Fluent API
```csharp

builder.Div()
        .Class("my-class")
        .Class("active", IsActive)
        .Class("text-block", !string.IsNullOrEmpty(Name))
        .Style($"font-size:{Size}px", Size.HasValue)
        .Content("hello world")
       .Close();

builder.Component<Button>()
        .Class("my-class")
        .Class("active", IsActive)
        .Class("text-block", !string.IsNullOrEmpty(Name))
        .Style((Size.HasValue, $"font-size:{Size}px"))
        .Content(ChildContent)
       .Close();

builder.Ul().ForEach("li", result => {
    result.attribute.Content($"content{result.index}");
});
```

## :children_crossing: Component Association
### In .razor file
* For `List.razor` file be parent component
```html
<ul @attributes="@GetAttributes()">
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
    configure.Renderers.Add(typeof(NavLinkComponentRenderer));
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
```

[Read document for more informations](https://playermaker.gitbook.io/componentbuilder)


## :pencil: Component Library Solution Template
Use `ComponentBuilder.Templates` to generate a razor component library solution and online demo site
```bash
dotnet new install ComponentBuilder.Templates
dotnet new blazor-sln -n {YourRazorLibraryName}
```
More information see [templates](./templates/readme.md)