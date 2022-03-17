## ComponentBuilder
A framework based on `RenderTreeBuilder` help you easily to create blazor component library.

[中文](Readme.md) | [English](Readme.en-us.md)

## :sparkles: Features
* Support `RenderTreeBuilder` to create blazor component
* Support CSS class, html attributes to apply attributes on parameter
* Inner common base components
* Load parameters, eventcallbacks you need
* Dynamic invoke JS method

## :rainbow: Demo

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
<!--Use Component-->
<MyButton Color="Color.Primary">Submit</MyButton>
<MyButton Active>Active Button</MyButton>

<!--HTML Rendered-->
<button class="btn btn-primary">Submit</button>
<button class="btn active">Active Button</button>
```

## :computer: Supports
* .NET 5
* .NET 6

## :blue_book: Installation

* Install from Nuget.org
```cmd
Install-Package ComponentBuilder
```

* Registet service
```csharp
services.AddComponentBuilder();
```

## :link: Links
* [Issues](/issues)
* [Releases](/releases)
* [Wiki](/wiki/en-us/readme.md)