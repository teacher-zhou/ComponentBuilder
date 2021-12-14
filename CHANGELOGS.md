# Change Logs
## 0.3
* [Fixed] Change `AdditionalAttributes` to be `IDictionary<string, object>` type.
* [New] `CssClass` parameter in `BlazorComponentBase` for builing css class utility by extensions class.
* [New] `GetCssClassString()` method can be overrided by drived class.
* [New] Discover `class` attribute in element can override any configurations of `CssClassAttribute` of parameter.
* [New] More extensions for `CreateElement` and `CreateComponent` method for `RenderTreeBuilder` class.
## 0.2
* [Refactor] Some methods in `BlazorComponentBase`
* [New] `BlazorChildContentComponentBase` class within `ChildContent` parameter
* [New] `BlazorChildComponentBase` & `BlazorParentComponentBase` class
* [Update] `ElementAttributeAttribute` support null name for parameter name
## 0.1
Initial projects

