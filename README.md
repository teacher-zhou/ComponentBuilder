# ComponentBuilder
A framework can easily help you to create blazor component.

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
> Define `CssClassAttribute` for `Parameters`, `Class`, `Enum members` ... and will be applied when component created.

### Basic
#### Apply for parameters
* Boolean type, apply value when value is `true`
```cs
[Parameter][CssClass("disabled")]public bool Disabled { get;set;}
```
```html
<MyComponent Disabled/>

<div class="disabled"></div>
```
* Int,String,Double ... those directly value types, append value behind `CssClass` defined
```cs
[Parameter][CssClass("margin-")]public int Margin { get; set; }

[Parameter][CssClass("padding-")]public int Padding { get; set; }
```
```html
<MyComponent Margin="3" Padding="2">...</MyComponent>

<div class="margin-3 padding-2">...</div>
```

* Enum type, append value for member
```cs
public enum Color
{
    Primary,
    Secondary, //apply member value with lowercase

    [CssClass("warn")]Warning   //..apply CssClass value
}

//..parameters in component
[Parameter][CssClass]public Color? Color { get; set; }
[Parameter][CssClass("bg-")]public Color? BgColor { get; set; }
```
```html
<MyComponent Color="Color.Primary"/>
<div class="primary">...</div>

<MyComponent Color="Color.Warining"/>
<div class="warn">...</div>

<MyComponent Color="Color.Secondary" BgColor="Color.Primary"/>
<div class="secondary bg-primary">...</div>
```

**Enum has default value for first member if parameter is not `null`**
```cs
[Parameter][CssClass]public Color Color { get; set; }
```
```html
<MyComponent/> <!--Default enum value is Color.Primary-->
<div class="primary">...</div>
```
#### Apply for component class
To apply default CSS class without any parameters
```cs
[CssClass("btn")]
public class Button : BlazorChildContentComponentBase
{
    [Parameter][CssClass("btn-")]public Color? Color { get; set; }
}
```
```html
<Button>...</Button>
<div class="btn">...</div>

<Button Color="Color.Primary">...</Button>
<div class="btn btn-primary">...</div>
```

#### Apply for interface
Define parameters using interface to reuse same CSS class.
```cs
public interface IHasDisabled
{
    [Parameter][CssClass("disabled")]bool Disabled{ get; set; }
}

public interface IHasMarginSpace
{
    [Parameter][CssClass("margin-")]int Margin { get; set; }
}

public class MyComponent1 : BlazorComponentBase, IHasDisabled
{    
    [Parameter]public bool Disabled{ get; set; }
}

public class MyComponent2 : BlazorComponentBase, IHasDisabled, IHasMargin
{    
    [Parameter]public bool Disabled{ get; set; }
    [Parameter]public int Margin { get; set; }
}

public class MyComponent3 : BlazorComponentBase, IHasMargin
{
    //override interface pre-define value
    [Parameter][CssClass("m-")]public int Margin { get; set; }
}
```

```html
<MyComponent1 Disabled />
<div class="disabled" />

<MyComponent2 Disabled Margin="3" />
<div class="disabled margin-3" />

<MyComponent3 Margin="5" />
<div class="m-5" />
```

#### Order CSS class
Set `Order` parameter for `CssClassAttribute` to order CSS class string when component built followed from small to large.

```cs
[Parameter][CssClass("block", Order=5)]public bool Block { get; set; }
[Parameter][CssClass("padding-")]public int Padding { get; set; }
```
```html
<MyComponent Block Padding="3"/>
<div class="padding-3 block" />
```
#### Disabled to apply CSS class
Set `Disabled` to `true` to disable apply CSS class when parameter has value.
```cs
[Parameter][CssClass("block", Disabled=true)]public bool Block { get; set; }
```
```html
<MyComponent Block />
<div>...</div>
```

### Advance
#### Override `BuildCssClass` method
To control logical code to create CSS class

```cs
protected override void BuildCssClass(ICssClassBuilder builder)
{
    //self logical here
    if(Name is not null)
    {
        builder.Append("show active");
    }

    //enum type can invoke GetCssClass() method
    builder.Append(Color.GetCssClass(), this._hasColor)
        .Append("active")
        .Append("basic");
}
```
## Html tag definition

### Html tag name
Define `HtmlTagAttribute` for component to define elemen tag name. Default is `div`.
```cs
[HtmlTag("button")]
public class Button : BlazorComponentBase
{
}
```
```html
<Button>...</Button>

<!--html rendered-->
<button>...</buton>
```

Override `TagName` property with logical code to output tag name.
```cs
public class MyComponent : BlazorComponentBase
{
    protected override string TagName
    {
        get 
        {
            if(Name is null)
            {
                return "a";
            }
            return "span";
        }
    }
}
```

### Html attributes
Parameters can be html attributes when value is applied using `HtmlAttribute`

* Use parameter value for attribute value
```cs
[Parameter][HtmlAttribute("href")]public string Link { get; set; }
```
```html
<MyComponent Link="www.bing.com"/>
<div href="www.bing.com"/>
```
* Use bool value of parameter for fixed attribute value when `true`
```cs
[Parameter][HtmlAttribute("data-toggle", "toggle")]public bool Toggle{ get; set; }
```
```html
<MyComponent Block />
<div data-toggle="toggle" />
```
* Use parameter name to be attribute's name
```cs
[Parameter][HtmlAttribute]public string Id { get; set; }
```
```html
<MyComponent Id="id-node" />
<div id="id-node" />
```
* Pre-define html attribute for component
```cs
[HtmlAttribute("role","alert")]
public class MyComponent : BlazorComponentBase
{
}
```
OR

use `HtmlRoleAttribute` to instead `HtmlAttribute("role", value)`
```cs
[HtmlRole("alert")]
public class MyComponent : BlazorComponentBase
{
}
```
```html
<MyComponent />

<div role="alert" />
```

## Parameter pre-definition
> Parameter pre-definition always named start with `IHasXXX` specification.
### IHasChildContent
Contains parameter `ChildContent` in `IHasChildContent` or `ChildContent<TValue>` in `IHasChildContent<TValue>`