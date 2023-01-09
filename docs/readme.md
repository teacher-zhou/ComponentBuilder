## Quick Start
** Only 2 steps to create automation component **

1. Inherits `BlazorComponentBase` instead of `ComponentBase`
2. **For `.razor` file component** must add `@attributes="AdditionalAttributes"` attribute for automation features with specified HTML element
2. Define `CssClassAttribute` for component or parameters

## Example for `.razor` file
* Create `Element.razor` file
```html
@inherits BlazorComponentBase

<span @attributes="AdditionalAttributes"> <!--@attributes is necessary-->
	@ChildContent
</span>

@code{
	[Parameter] public RenderFragment? ChildContent { get; set; }

	[Parameter][CssClass("active")] public bool Active { get; set; }
}
```
* Execution in razor
```html
<Element>Hello</Element>
<span>Hello</span>

<Element Active>Active Hello</Element>
<span class="active">Active Hello</span>
```

## In `Element.cs` class
* Create `Element` class
```csharp
[HtmlTag("span")]
public class Element : BlazorComponentBase, IHasChildContent
{
	[Parameter] public RenderFragment? ChildContent { get; set; }

	[Parameter][CssClass("active")] public bool Active { get; set; }
}
```
* Execution in razor
```html
<Element>Hello</Element>
<span>Hello</span>

<Element Active>Active Hello</Element>
<span class="active">Active Hello</span>
```
> As you can see, using component classes can fully automate components, but the downside is that it requires a certain amount of concrete thinking, especially when dealing with complex components. Of course you can mix the two, but as long as it increases efficiency, it's a good idea.