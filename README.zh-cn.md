# ComponentBuilder

轻松创建具有自动化功能的Blazor组件库，并同时支持 razor文件和 RenderTreeBuilder 两种方式。

[English](README.md) | [快速上手](./docs/readme.zh-CN.md) | [在线文档](https://playermaker.gitbook.io/componentbuilder/chinese/jian-jie)

![Latest Version](https://img.shields.io/github/v/release/AchievedOwner/ComponentBuilder) 

v4.x 支持
![.net6](https://img.shields.io/badge/.net-6-blue)
![.net7](https://img.shields.io/badge/.net-7-blue)

v5.x 支持
![.net8](https://img.shields.io/badge/.net-8-blue)

## :sparkles: 特性

* 属性优先，从参数中优雅地定义 CSS 
* 轻松通过属性与组件关联
* 可自定义CSS和组件的属性编码逻辑
* 同时支持 razor文件 或 `RenderTreeBuilder` 来创建组件
* 支持具有类似参数的组件的“预定义”
* 动态且简单地和 JS 进行交互
* 拦截器设计，为组件的生命周期赋予新的功能
* 渲染器管道模式，以识别动态渲染组件
* 丰富地 'RenderTreeBuilder' 扩展方法，代码片段轻松编写
* 用 Fluent API 轻松创建元素和组件

## :rainbow: 组件定义

* 在 `Button.razor`
```html
@inherits BlazorComponentBase

<button @attributes="@GetAttributes()">
    @ChildContent
</button>

@code{
    [CssClass("btn")]
    public Button()
    {
    }

    [Parameter][CssClass("active")]public bool Active { get; set; } 
	
	[Parameter][CssClass("btn-")]public Color? Color { get; set; } 

	[Parameter]public RenderFragment? ChildContent { get; set; } 

	[Parameter][HtmlData("tooltip")]public string? Tooltip { get; set; } 

	[Parameter][HtmlEvent("onclick")]public EventCallback<ClickEventArgs> OnClick { get; set; }

    [Parameter][HtmlAttribute]public string? Title { get; set; }
    
    public enum Color
    {
	    Primary,
	    Secondary,
	    [CssClass("info")]Information,
    }
}
```

* 在 `Button.cs` 类
```csharp
[HtmlTag("button")]
[CssClass("btn")]
public class Button : BlazorComponentBase, IHasChildContent, IHasOnClick
{
	[Parameter][CssClass("active")]public bool Active { get; set; } 
	
	[Parameter][CssClass("btn-")]public Color? Color { get; set; } 

	[Parameter]public RenderFragment? ChildContent { get; set; }

	[Parameter][HtmlData("tooltip")]public string? Tooltip { get; set; }

	[Parameter][HtmlEvent("onclick")]public EventCallback<ClickEventArgs> OnClick { get; set; 

    [Parameter][HtmlAttribute]public string? Title { get; set; }
}

public enum Color
{
	Primary,
	Secondary,
	[CssClass("info")]Information,
}
```
* 使用和对比
```html
<!--razor-->
<Button Color="Color.Primary">Submit</Button>
<!--html-->
<button class="btn btn-primary">Submit</button>

<!--razor-->
<Button Active Tooltip="active button" Color="Color.Information" Title="click me">Active Button</Button>
<!--html-->
<button class="btn btn-info active" data-tooltip="active button" title="click me">Active Button</button>
```


## :key: C# 和 Javascript 交互

* 导入模块
```js
//in app.js
export function display(){
 // ...your code
}
```

```csharp
[Inject]IJSRuntime JS { get; set; }

var js = await JS.Value.ImportAsync("./app.js");
js.display(); // same as function name
```

* 动态运行 JS 代码
```csharp
JS.Value.EvaluateAsync(window => {
    window.console.log("log")
});

JS.Value.EvaludateAsync(@"
    console.log(\"log\");
")
```

* 快速使用 JS 回调 C# 代码
```csharp
JS.Value.InvokeVoidAsync("myFunction", CallbackFactory.Create<string>(arg=> {
    //get arg from js
}));

JS.Value.InvokeVoidAsync("calculate", CallbackFactory.Create<int,int>((arg1,arg2)=> {
    //get value of arg1,arg2 from js
}))
```
```js
function myFunction(dotNetRef){
    dotNetRef.invokeMethodAsync("Invoke", "arg");
}

function calculate(dotNetRef){
    dotNetRef.invokeMethodAsync("Invoke", 1, 2);
}
```

## :information_source: 个性化 CSS/Style/Attributes
* Logical CSS
```csharp
protected override void BuildCssClass(ICssClassBuilder builder)
{
    if(builder.Contains("annotation-enter"))
    {
        builder.Remove("annotation-exist");
    }
    else
    {
        builder.Append("annotation-enter").Append("annotation-exist");
    }
}
```
* Logical Attributes
```csharp
protected override void BuildAttributes(IDictionary<string, object> attributes)
{
    attributes["onclick"] = HtmlHelper.Event.Create(this, ()=>{ ... });
    
    if(attrbutes.ContainKey("data-toggle"))
    {
        attributes["data-toggle"] = "collapse";
    }
}
```
## :palm_tree: Fluent API
```csharp

builder.Div()
        .Class("my-class")
        .Class("active", IsActive)
        .Class("text-block", !string.IsNullOrEmpty(Name))
        .Style($"font-size:{Size}px", Size.HasValue)
        .Content("hello world")
       .Close();

builder.Component<Button>()
        .Class("my-class")
        .Class("active", IsActive)
        .Class("text-block", !string.IsNullOrEmpty(Name))
        .Style((Size.HasValue, $"font-size:{Size}px"))
        .Content(ChildContent)
       .Close();

builder.Ul().ForEach("li", result => {
    result.attribute.Content($"content{result.index}");
});
```

## :children_crossing: 关联组件
### 在 .razor 文件
* `List.razor` 作为父组件
```html
<ul @attributes="@GetAttributes()">
    <CascadingValue Value="this">
        @ChildContent
    </CascadingValue>
</ul>
```

* `ListItem.razor` 作为子组件
```html
<li @attributes="@GetAttributes()">@ChildContent</li>

@code{
    [ChildComponent(typeof(List))]
    public ListItem()
    {
    }

    [CascadingParameter] public List CascadedList { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }
}
```

### 在 RenderTreeBuilder 中
* `List` 组件类
```csharp
[ParentComponent] //作为父组件
[HtmlTag("ul")]
public class List : BlazorComponentBase, IHasChildContent
{

}
```
* `ListItem` 组件类
```cs
[ChildComponent(typeof(List))] //强关联
[HtmlTag("li")]
public class ListItem : BlazorComponentBase, IHasChildContent
{
    [CascadingParameter]public List CascadedList { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }
}
```

### 组件的使用

```html
<List>
    <ListItem>...</ListItem>
</List>

<ListItem /> <!--ListItem 组件不在 List 组件中将抛出异常-->

```

## :six_pointed_star: HtmlHelper

* 在 `.razor` 中

```html
<div class="@GetCssClass"></div>

@code{
    string GetCssClass => HtmlHelper.Class.Append("btn-primary").Append("active", Actived).ToString();
}
```

* 应用于 RenderTreeBuilder 时

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

## :crossed_swords: 拦截器
您可以拦截组件的生命周期

* 定义拦截器
```csharp
public class LogInterceptor : ComponentInterceptorBase
{
    private readonly ILogger<LogInterceptor> _logger;
    public LogInterceptor(ILogger<LogInterceptor> logger)
    {
        _logger = logger;
    }

    //在 SetParameterAsync 方法中执行
    public override void InterceptSetParameters(IBlazorComponent component, ParameterView parameters)
    {
        foreach(var item in parameters)
        {
            _logger.LogDebug($"Key:{item.Name}, Value:{item.Value}");
        }
    }
}
```
* 注册拦截器
```csharp
builder.Services.AddComponentBuilder(configure => {
    configure.Interceptors.Add(new LogInterceptor());
})
```
![BlazorComponentBase Lifecycle](./asset/BlazorComponentBaseLifecycle.png)

## :recycle: 渲染器管道
识别特殊情况，以呈现指定的组件
```csharp
public class NavLinkComponentRender : IComponentRender
{
    public bool Render(IBlazorComponent component, RenderTreeBuilder builder)
    {
        if ( component is IHasNavLink navLink )
        {
            builder.OpenComponent<NavLink>(0);
            builder.AddAttribute(1, nameof(NavLink.Match), navLink.Match);
            builder.AddAttribute(2, nameof(NavLink.ActiveClass), navLink.ActiveCssClass);
            builder.AddAttribute(3, nameof(NavLink.ChildContent), navLink.ChildContent);
            builder.AddMultipleAttributes(4, component.GetAttributes());
            builder.CloseComponent();
            return false;
        }
        return true;
    }
}
```
* 注册渲染器
```csharp
builder.Services.AddComponentBuilder(configure => {
    configure.Renderers.Add(typeof(NavLinkComponentRenderer));
});
```

## :blue_book: 安装指南

* 从 `Nuget.org` 安装

```bash
Install-Package ComponentBuilder
```

* 注册服务

```csharp
builder.Services.AddComponentBuilder();
```


## :pencil: 组件库解决方案模板
使用 `ComponentBuilder.Templates` 生成组件库解决方案和在线演示站点
```bash
dotnet new install ComponentBuilder.Templates
dotnet new blazor-sln -n {YourRazorLibraryName}
```
更多信息见 [templates](./templates/readme.md)