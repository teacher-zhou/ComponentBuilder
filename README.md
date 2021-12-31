# ComponentBuilder
A framework can easily help you to create blazor component from code behind.

# QuickStart

## 1. Install package
```cmd
> Install-Package ComponentBuilder
```
## 2. Add Service
```cs
service.AddComponentBuilder();
```

## 3. Define your component in behind code
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

## 4. Use your component in razor
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

# Support
* .NET 5
* .NET 6

# Guideline

## Component definition
### BlazorComponentBase
> This is the base component to get all features for component, any component that contains features need to inherited from this class.

```cs
public class MyComponent : BlazorComponentBase
{
}
```
```html
<!-- Use Component -->
<MyComponent />

<!-- Render in HTML -->
<div />
```

### BlazorChildContentComponentBase
> This is a base component that inherited from `BlazorComponentBase` and has child content parameter.

```cs
public class MyComponent : BlazorChildContentComponentBase
{
}
```

```html
<!-- Use Component -->
<MyComponent>...</MyComponent>
<MyComponent />
<MyComponent>
    <ChildContent>....</ChildContent>
</MyComponent>

<!-- Render in HTML -->
<div>...</div>
```

or you can inplement `IHasChildContent` or `IHasChildContent<TValue>` to create child content attribute automatically.

```cs
public class MyChildContentComponent : BlazorComponentBase, IHasChildContent
{
    [Parameter]public RenderFragment ChildContent { get; set; }
}
```
```html
<!-- Use Component -->
<MyChildContentComponent>...</MyChildContentComponent>
<MyChildContentComponent />
<MyChildContentComponent>
    <ChildContent>....</ChildContent>
</MyChildContentComponent>

<!-- Render in HTML -->
<div>...</div>
```
**OR**
```cs
public class MyChildModelComponent : BlazorComponentBase, IHasChildContent<MyModel>
{
    [Parameter]public RenderFragment<MyModel> ChildContent { get; set; }
}
```
```html

<MyChildModelComponent>
    @context.ModelProperty <!--context type is MyModel-->
</MyChildModelComponent>
```

### Strong relationship for child component
> Inherit `BlazorParentComponentBase` and `BlazorChildComponentBase` to create child component with strong relationship between them.

```cs
public class MyParentComponent : BlazorParentComponentBase<MyParentComponent>
{
}

public class MyChildComponent : BlazorChildComponentBase<MyParentComponent>
{
}
```

```html
<MyParentComponent>
    <MyChildComponent>...</MyChildComponent>
</MyParentComponent>

OR

<MyParentComponent>
    ...
    <ChildContent>
        <MyChildComponent>...</MyChildComponent>
    </ChildContent>
    ...
</MyParentComponent>


<!--Exception thrown 'MyChildComponent must be the child component of MyParentCompont' while no parent component for child component-->
<MyChildComponent>...</MyChildComponent>
```

## Quick CSS apply
> Define `CssClassAttribute` for `Parameters`, `Class`, 'Enum members' ... and will be applied when component created.

### Basic
#### Apply for parameters

#### Apply for component class

#### Apply for interface

### Advance
#### Override `BuildCssClass` method