# ComponentBuilder
A framework can easily help you to create blazor component from code behind.

# QuickStart

## 1. Add Service
```cs
service.AddComponentBuilder();
```

## 2. Define your component in behind code
```cs
[ElementTag("button")]
[CssClass("btn")]
public class Button : BlazorComponentBase, IHasChildContent
{
    [Parameter] [CssClass("btn-")] public Color? Color { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }
}

public enum Color
{
    Primary,
    Secondary,
    Danger,
    Warning,
    Info,
    Dark,
    Light,
    Success
}
```

## 3. Use your component in razor
```html
<Button Color="Color.Primary">Primary</Button>
<Button Color="Color.Danger">Danger</Button>
```

![](assets\demo1.jpg)

Html display
```html
<button class="btn btn-primary">Primary</button>
<button class="btn btn-danger">Danger</button>
```

# Razor file VS Code behind

In `Button.razor`
```cs
<button class="@(GetCssClass())" @attributes="Attributes">@ChildContent</button>


@code {
    [Parameter] public RenderFragment ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> Attributes { get; set; }

    [Parameter] public Color? Color { get; set; }

    [Parameter] public bool Active { get; set; }


    string GetCssClass()
    {
        var cssList = new List<string>();

        if (Active)
        {
            cssList.Add("active");
        }
        if (Color.HasValue)
        {
            cssList.Add($"btn-{Color.Value.ToString().ToLower()}");
        }
        return string.Join(" ", cssList);
    }
}
```

In `Button.cs`
```cs
[ElementTag("button")]
[CssClass("btn")]
public class Button : BlazorComponentBase, IHasChildContent
{
    [Parameter] public RenderFragment ChildContent { get; set; }

    [Parameter] [CssClass("btn-")] public Color? Color { get; set; }

    [Parameter] [CssClass("active")] public bool Active { get; set; }
}
```

# Customization

## Conditional building css class
> Overrides `BuildCssClass` method to build css by logical code

```cs
[ElementTag("button")]
[CssClass("btn")]
public class Button : BlazorComponentBase, IHasChildContent
{
    [Parameter] public RenderFragment ChildContent { get; set; }

    [Parameter] [CssClass("btn-")] public Color? Color { get; set; }

    [Parameter] [CssClass("active")] public bool Active { get; set; }

    [Inject]IHostingEnvironment Env { get; set; }

    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        if(Env.IsDevelopment())
        {
            builder.Append("container-sm");
        }
    }
}
```

## Element attribute by parameter
Set `ElementAttribute` for pameters to create attribute of element
```cs
[ElementTag("a")]
public class Anchor : BlazorChildContentComponentBase
{
    [ElementAttribute("name")][Parameter]public string Alias { get; set; }
    [ElementAttribute("href")][Parameter]public string Link { get; set; }

    //build same name of parameter with lowercase
    [ElementAttribute][Parameter]public string Title { get; set; } 
}
```
```html
<!--Use component-->
<Anchor Link="www.bing.com" Alias="link" Title="Go To Bing">Click Here</Anchor>
<!--Render html-->
<a href="www.bing.com" name="link" title="Go To Bing">Click Here</a>
```

## Additional attributes captured
```cs
[ElementTag("a")]
public class LinkButton : BlazorChildContentComponentBase
{
}
```

```html
<!--Use component-->
<Button data-toggle="modal">Link</Button>

<!--Render html-->
<a data-toggle="modal">Link</a>
```


# Support
* .NET 5
* .NET 6