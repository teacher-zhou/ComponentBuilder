# Change Logs
## 2.1
[New]`JS.Evaluate` function to execute jsvascript string
[Update].NET 7 support
## 2.0
[New]`BlazorAbstractComponentBase` abstraction class
[New]`BlazorRenderTree` class
[Update]`IHasTwoWayBinding` change to `IHasValueBound`
[Update]`HtmlHelper` optimization
[Update]`CreateElement` and `CreateComponent` modified the arguments
[Remove]`RenderComponentAttribute` attribute class

## 1.5.1
[新增]`CreateElementIf`、`CreateComponentIf` 的扩展方法
[新增]`RenderTreeBuilderFragment` 类型
[更新]`RenderTreeBuilder` 的 `CreateXXX` 元素方法的最后一个参数的数据类型改为 `Action<RenderTreeBuilderFragment>` 委托

## 1.5
[新增] `HtmlHelper` 增加 `CreateContent` 方法并返回 `RenderFragment` 委托
[新增] `RenderTreeBuilder` 的 `CreateElement` 和 `CreateComponent` 具备 `Action<IDictionary<string,object>>` 的动态 HTML 属性的扩展方法
[新增]常用的 `RenderTreeBuilder` 创建元素，如 `div` `span` `p` `br` `hr` 等的扩展方法
[新增]对 `OneOf` 的支持
[新增]`ICssClassBuilder` 新增 `IsEmpty(string? value)` `Remove(string? value)` `Contains(string? value)` 方法
[优化]`CssClassAttribute` 特性支持 `string.Format` 的形式占位
[优化]`BlazorComponentBase` 加载 CSS 方法，可以通过 `CssClassBuilder` 动态改变组件的 CSS 
[优化]删除 `RenderTreeBuilder` 的 `CreateElement` 和 `CreateComponent` 方法的重载
[移除]`CssClassAttribute` 的 `Suffix` 属性
## 1.4.1.1
* [Fixed]Missing `onsubmit` callback in `BlazorFormComponentBase`
## 1.4.1
* [Fixed]`HtmlAttributeAttribute` 特性在 `bool` 为 `true` 时不应用值生成 HTML 元素属性的问题
* [Remove]`ServiceComponentAttribute` 组件服务特性
## 1.4
* [New]创建 `<style>...</style>` 代码块的 `RenderTreeBuilder` 的扩展方法
* [New]`ICssClassUtility` 的 `Append(string value,bool condition)` 扩展方法
* [New]`ICssClassBuilder` 的 `Append(bool condition, string trueValue, string falseValue)` 扩展方法
* [Fixed]`ComponentRenderAttribute` 改为 `RenderComponentAttribute`
* [Fixed]`ICssClassProvider` 改为 `ICssClassUtility`
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


