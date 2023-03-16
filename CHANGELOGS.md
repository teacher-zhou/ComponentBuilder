# Change Logs
## 3.1.1
* [New]`Input` extension for Fluent API to create `<input />` element
* [New]Support Fluent creating element or component without `Close` at the end
* [Fix]html attributes is not cleared for fluent API
* [Update]Change `CallbackFactory` support at least 3 generic type of arguments
## 3.1.0
### :rainbow: Features
* [New]`RenderTreeBuilder` instance can create `FluentRenderTreeBuilder` of element and component
* [New]More extensions of `FluentRenderTreeBuilder` from `RenderTreeBuilder`
* [New]`IAnchorComponent` interface for pre-definition

### :negative_squared_cross_mark: Fixes
* [Fix]`ComponentBuilderOption.Debug = true` does not write the trace of log
* [Fix]`HtmlAttributeAttribute` support for `Enum` member
* [Fix]Attributes did not clear when using Fluent API to create element
### :small_red_triangle: Chornes
* [Chrone]Change namespace `ComponentBuilder.Parameters` to `ComponentBuilder.Definitions`
## 3.0.0
* [New]FluentRenderTreeBuilder to  create element and component using fluent API
* [New]`IFluentRenderTreeBuilder` and Fluent API extensions by `RenderTreeBuilder`
* [New]`GetAttributes()` supports `.razor` & component class
* [New]Pre-Definition supports `HtmlAttributeAttribute` including `HtmlRole` `HtmlData`
* [New]`HtmlTagAttribute` support interface for pre-definition
* [New]`IFluentCssClassBuilder` interface to support fluent class style
* [New]`StringExtensions` method
* [New]Interceptor pattern to redesign the lifecycle of component
* [New]Full support `.razor` & component class that inherited from `BlazorComponentBase` class
* [Fix]The child component association error
* [Fix]`HtmlAttribute` for bool always add to html attribute
* [Fix]`IHasNavLink` missing active status
* [Fix]The lifecycle of `BlazorComponentBase` workflow
* [Fix]a lot of bugs...
* [Update]`HtmlResolvers` and `CssClassResolvers` in `ComponentBuilderOptions`
* [Update]namespace of `Abstractions` renamed `Builder` and new `Resolvers` for resolver instances
* [Refactor]The lifecyle of interceptors
* [Remove]`HtmlEventAttribute` & `HtmlDataAttribute`, use `HtmlAttributeAttribute` instead all
* [Update]New documentation using gitbook support English & Chinese
## 2.3
* [New] `CallbackFactory` for invocation from javascript to call C# function
* [New]New method `DisposeComponentResources` and `DisposeComponentResources` for disposable pattern
* [New]A `Concat` property in `CssClassAttribute` to concat CSS from base component class
* [Update] `BlazorAnchorComponentBase`, `BlazorFormComponentBase`, `BlazorInputComponentBase` is obsoleted, using `IHasNavLink`,`IHasForm`,`IHasInput` instead. Now only `BlazorComponentBase` to inherit.
* [Update]Change `IHasAdditinalCssClass` to `IHasAdditionalClass`
* [Update]Change property `AdditionalCssClass` to `AdditionalClass` in `IHasAdditionalClass`
* [Update]`IsWebAssembly` property to `IsWebAssembly()` method
## 2.2
* [New]`Reference` property to capture `ElementReference` in `BlazorAbstractComponentBase`
* [New]`CaptureReference` property to set that can capture `ElementReference` for the `Reference` property
* [Update]The license to Apache version
* [Update]`TagName` property could be removed, add new method `string GetElementTagName()` instead
## 2.1
* [New]`EvaluateAsync` function to execute jsvascript string for `IJSRuntime` instance
* [New]`EventCallbackFactory` extensions
* [New]`BlzorRenderTree` extensions
* [New].NET 7 support

## 2.0
[New]`BlazorAbstractComponentBase` abstraction class
[New]`BlazorRenderTree` class
[Update]`IHasTwoWayBinding` change to `IHasValueBound`
[Update]`HtmlHelper` optimization
[Update]`CreateElement` and `CreateComponent` modified the arguments
[Remove]`RenderComponentAttribute` attribute class

## 1.4
* [New] `<style>...</style>` for `RenderTreeBuilder` extension
* [New]`Append(string value,bool condition)` extension method for `ICssClassUtility` 
* [New]`Append(bool condition, string trueValue, string falseValue)` extension method for `ICssClassBuilder`
* [Fixed]`ComponentRenderAttribute` renamed to `RenderComponentAttribute`
* [Fixed]`ICssClassProvider` renamed to `ICssClassUtility`
## 1.3
* [Remove]`BlazorChildContentComponentBase`, `BlazorParentComponentBase`, `BlazorChildComponentBase`
* [Remove]Pre-definition `CssClassAttribute` of `IHasActive` and `IHasDisabled`
* [Update]Wiki with latest version
* [Update]All comments
## 1.2
* [New]`AnchorComponentBase` to provide user create any component with anchor function.
* [New]Enum supoort `CssClassAttribute` for prefix of CSS class when building.
* [New]Add `Enumeration` abstract class to define a class like enum.
* [Rename]All `ComponentBase` with this library change namespace to `ComponentBuilder`
## 1.1
* [New]`ParentComponentAttribute` to create cascading component automatically
* [New]`ChildComponentAttribute` to get cascading parameter for component and add to be child of component
* [New]`RenderComponentAttribute` to render a specific component
* [Rename]`BlazorFormBase` to `BlazorFormComponentBase`
* [Rename]`BlazorInputBase` to `BlazorInputComponentBase`
## 0.7
* [New]`BlazorFormBase<TForm>` to help create form component.
* [New]`BlazorInputBase<TValue>` to help create input control component with two-way binding
* [New]`CreateStyleBuilder` in `CssHelper`
* [New]`CreateCallback` method in `HtmlHelper` to create `EventCallback` delegate
* [Rename]`CssHelper` to `HtmlHelper`, `MergeAttributes` to `MergeHtmlAttrbutes`, `CreateBuilder` to `CreateCssBuilder`
## 0.6
* [New]`appendFunc` argument witch is a `Func<RenderTreeBuilder, int, int>` type for extension method `CreateElement` and `CreateComponent`
* [New] Override `BuildStyle` method to build `style` pre-definition of element
* [New]`AdditionalStyle` parameter to append value after `BuildStyle` method executed
* [New]`Insert` method for `ICssClassBuilder`
* [New]`Suffix` property for `CssClassAttribute` to change append value of parameter from **prefix** to **suffix**
* [New]`HtmlDataAttribute` to build `data-{name}="{value}"` for parameter
* [New]`BlazorComponentCollection` class
* [New]`BooleanCssClassAttribute` class to apply CSS class for parameter witch is `bool` data type
* [New]interface pre-definition to apply CSS class
* [New]Add events `OnCssClassBuilding` & `OnCssClassBuilt` in `GetCssClassString` method to raise.
* [Fixed]`HtmlAttributeAttribute` to support enum member
* [Fixed]`GetCssClassString` does not support for `CssClassAttribute` and `CssClass` when has parameters
* [Fixed]A lot of bugs
## 0.5
* [Rename]`BuildBlazorComponentAttributes` method to `BuildComponentAttributes`
* [New]`HtmlEventAttribute` for `EventCallback` parameter
* [New]`IHtmlEventAttributeResolver` and default implementation `HtmlEventAttributeResolver` to resolve `HtmlEventAttribute`
* [New]`TagName` property instead `GetElementTagName` method
* [New]`Disabled` property for `CssClassAttribute` to prevent apply CSS class
* [Delete] `GetElementTagName` method
## 0.4
* [New] `Import` method for `IJSRuntime` and call js function in C# dynamiclly.
* [Fix] `CreateElement` & `CreateComponent` Cannot create child content using `string`
* [Fix] Renamed `Js` property in `BlazorComponentBase` to `JS`
## 0.3
* [New] `CssClass` parameter in `BlazorComponentBase` for builing css class utility by extensions class.
* [New] `GetCssClassString()` method can be overrided by drived class.
* [New] Discover `class` attribute in element can override any configurations of `CssClassAttribute` of parameter.
* [New] More extensions for `CreateElement` and `CreateComponent` method for `RenderTreeBuilder` class.
* [Fix] Rename `HtmlHelper` to `CssHelper`
* [Fix] Change `AdditionalAttributes` to be `IDictionary<string, object>` type.
## 0.2
* [Refactor] Some methods in `BlazorComponentBase`
* [New] `BlazorChildContentComponentBase` class within `ChildContent` parameter
* [New] `BlazorChildComponentBase` & `BlazorParentComponentBase` class
* [Update] `ElementAttributeAttribute` support null name for parameter name
## 0.1
Initial projects


