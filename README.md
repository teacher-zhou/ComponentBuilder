# ComponentBuilder

An automation framework to create Blazor component by `RenderTreeBuilder`.

[English](README.md) | [中文](./README.zh-cn.md)

## :sparkles: Features

* It's a framework, not a component library
* Automatically build component by attribute definitions
* Strong extensions of `RenderTreeBuilder`
* Support modular JS to import and invoke dynamically
* Flexibility in the layout of any HTML elements
* Write logic to present different components
* The challenge of representational thinking

![.net6](https://img.shields.io/badge/.net-6-green)
![.net7](https://img.shields.io/badge/.net-7-green)

![Latest Version](https://img.shields.io/github/v/release/AchievedOwner/ComponentBuilder)

## :rainbow: Define Component

```csharp
[HtmlTag("button")] // element tag to render
[CssClass("btn")] // fixed css
public class MyButton : BlazorComponentBase, IHasChildContent, IHasOnClick
{
	[Parameter][CssClass("active")]public bool Active { get; set; } // true to append active CSS
	
	[Parameter][CssClass("btn-")]public Color? Color { get; set; } // combine with definition and enum member

	[Parameter]public RenderFragment? ChildContent { get; set; } // support inner html content

	[Parameter][HtmlData("tooltip")]public string? Tooltip { get; set; } // generate data-tooltip attribute of element

	[Parameter][HtmlEvent("onclick")]public EventCallback<ClickEventArgs> OnClick { get; set; } //automatically register a callback for onclick event.

        [Parameter][HtmlAttribute]public string? Title { get; set; } //generate title attribute in element
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
<MyButton Active Tooltip="active button" Color="Color.Information" Title="click me">Active Button</MyButton>
<!--html-->
<button class="btn btn-info active" data-tooltip="active button" title="click me">Active Button</button>
```

## :muscle: Supports razor file component `v2.2`
```html
<button @attributes="@AdditionalAttributes">@ChildContent</button>
```

## :key: JS import and invoke

```js
//in app.js
export function display(){
 // ...your code
}
```

```cs
[Inject]IJSRuntime JS { get; set; }

var js = await JS.Value.ImportAsync("./app.js");
js.display(); // same as function name
```

## :large_blue_circle: Create Element

```cs
protected override void BuildRenderTree(RenderTreeBuilder builder)
{
    builder.Open("div")
            .Class("my-class", (IsActive, "active"), (!string.IsNullOrEmpty(Name), "text-block"))
            .Style((Size.HasValue, $"font-size:{Size}px"))
            .Content("hello world")
           .Close();
}
```

## :large_orange_diamond: Create Component

```cs
protected override void BuildRenderTree(RenderTreeBuilder builder)
{
    builder.Open<Button>()
            .Class("my-class", (IsActive, "active"), (!string.IsNullOrEmpty(Name), "text-block"))
            .Style((Size.HasValue, $"font-size:{Size}px"))
            .Content(ChildContent)
           .Close();
}
```

## :children_crossing: Nested component

* Parent component

```cs
[ParentComponent] //be cascading parameter for this component
[HtmlTag("ul")]
public class List : BlazorComponentBase, IHasChildContent
{

}
```
* Child component
  
```cs
[ChildComponent(typeof(List))] //Strong association with List
[ChildComponent(typeof(Menu), Optional = true)] //Soft association
[HtmlTag("li")]
public class ListItem : BlazorComponentBase
{        
        // Required
    [CascadingParameter]public List CascadingList { get; set; }

    // Optional, maybe null
    [CascadingParameter]public Menu? CascadingMenu { get; set; }
}
```

* Usage

    ```html
    <List>
        <ListItem>...</ListItem>
    </List>

    <ListItem /> <!--throw because should be child component of List-->

    <Menu>
        <ListItem>...</ListItem>
    </Menu>
    ```

## :six_pointed_star: HtmlHelper

* in `.razor` file

```html
<div class="@GetCssClass">
...
</div>
```

```cs
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

* Logical code for component

  * Build CSS

  ```cs
  protected override void BuildCssClass(ICssClassBuilder builder)
  {
      if(User.Identity.IsAuthenticated)
      {
          builder.Append("user-plus");
      }
  }
  ```

  * Build style

  ```cs
  protected override void BuildStlye(IStyleBuilder builder)
  {
      if(IsAdmin)
      {
          builder.Append("display:block");
      }
  }
  ```

  * Build attributes

  ```cs
  protected override void BuildAttributes(IDictionary<string,object> attributes)
  {
      if(!Disabled)
      {
          attributes["onclick"] = HtmlHelper.Event.Create<MouseEventArgs>(this, ()=> Clicked = true);
      }
  }
  ```

## :boom: Dynamic style

```cs
builder.CreateStyleRegion(0, selector => {
    selector.AddStyle(".fade-in" , 
                        new { 
                            opacity = 1 
                        })
            .AddStyle("#element", 
                        new { 
                            width = "120px", 
                            height = "80px", 
                            border_right="solid 1px #ccc"
                        });

    selector.AddKeyFrames("FadeIn", k => {
        k.Add("from", 
                new { 
                    width = "40px"，
                    height = "150px"
                })
        .Add("to", 
                new { 
                    width = "150px",
                    height = "30px"
                });
    })
});
```
Generate style:
```css
.fade-in {
    opacity:1;
}
#element {
    width:120px;
    height:80px;
    border-right:"solid 1px #ccc";
}
@keyframes FadeIn{
    from {
        width:40px;
        height:150px;
    },
    to {
       width:150x;
       height:30px 
    }
}
```

## :computer: Environment

* .NET 6
* .NET 7

## :blue_book: Installation

* Install from `Nuget.org`

```bash
Install-Package ComponentBuilder
```

* Register service

```csharp
builder.Services.AddComponentBuilder();
```

## :link: Link

* [Issues](https://github.com/AchievedOwner/ComponentBuilder/issues)
* [Releases](https://github.com/AchievedOwner/ComponentBuilder/releases)
* [Documents](https://github.com/AchievedOwner/ComponentBuilder/wiki)