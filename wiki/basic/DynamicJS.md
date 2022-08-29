# DynamicJS
动态调用 JS 方法

在 `.razor` 中
```html
@inject IJSRuntime JS
```
在 `.cs` 中
```csharp
[Inject] IJSRuntime JS { get; set; }
```

注入 `IJSRuntime` 接口，调用 `Import` 方法

```csharp
var import = await JS.Import("/js/main.js"); //返回 dynamic 类型

//调用 js 方法名和相关数量的参数
import.alert("here is alert");
```
支持有返回值的调用
```csharp
import.getUser<User>(id);
```

**区分大小写，并且参数个数和返回值要匹配**