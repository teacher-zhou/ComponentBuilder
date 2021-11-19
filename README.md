# ComponentBuilder
The easiest way to build blazor component in code behind

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

# Razor file VS ComponentBuilder

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

# Support
.NET Core 3.1+
.NET 5
.NET 6