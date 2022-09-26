# ComponentBuilder
这是一个半自动化的 Blazor 组件框架，让你轻松地使用 `RenderTreeBuilder` 来创建属于自己的组件库。

> 为什么要使用 `RenderTreeBuilder` 来创建组件？RenderTreeBuilder 是 Blazor 组件的底层逻辑，可以编写复杂的逻辑代码来创建组件，比如动态 HTML 标记，动态 CSS 样式，动画效果。

## :sparkles: 亮点
* 基于 `RenderTreeBuilder` 编写组件
* 通过特性配置参数以满足 CSS 的动态应用
* 动态 CSS 类、样式、属性、事件、动画等
* 无限组件扩展，简化组件编写
* 满足具备业务逻辑的组件编写
* 动态 JS 模块函数的调用
* 强大 HtmlHelper 创建动态样式、事件等

## :rainbow: 创建组件
```csharp
[HtmlTag("button")]
[CssClass("btn")]
public class MyButton : BlazorComponentBase, IHasChildContent, IHasOnClick
{
	[Parameter][CssClass("active")]public bool Active { get; set; }
	
	[Parameter][CssClass("btn-")]public Color? Color { get; set; }

	[Parameter]public RenderFragment? ChildContent { get; set; }

	[Parameter][HtmlData("tooltip")]public string? Tooltip { get; set; }

	[Parameter][HtmlEvent("onclick")]public EventCallback<ClickEventArgs> OnClick { get; set; }
}

public enum Color
{
	Primary,
	Secondary,
	[CssClass("info")]Information,
}
```
```html
<!--razor-->
<MyButton Color="Color.Primary">Submit</MyButton>

<!--html-->
<button class="btn btn-primary">Submit</button>


<!--razor-->
<MyButton Active Tooltip="active button" Color="Color.Information">Active Button</MyButton>

<!--html-->
<button class="btn btn-info active" data-tooltip="active button">Active Button</button>
```

## :key: 动态 JS
```js
//保存在 app.js 中
export function display(){
	// 你的代码
}
```
```cs
[Inject]IJSRuntime JS { get; set; }

var js = await JS.Import("./app.js");
js.display();
```

## :large_blue_circle: 创建元素
```cs
protected override void BuildRenderTree(RenderTreeBuilder builder)
{
    builder.CreateElement(0, "div", ChildContent, new { @class = "my-class" });
}
```

## :large_orange_diamond: 创建组件
```cs
protected override void BuildRenderTree(RenderTreeBuilder builder)
{
    builder.CreateComponent<Button>(0, ChildContent, new { @class = "my-class" });
}
```
## :children_crossing: 父子组件
* 父组件
	```cs
	[ParentComponent]
	[HtmlTag("ul")]
	public class List : BlazorComponentBase, IHasChildContent
	{

	}
	```
* 子组件
	```cs
	[ChildComponent(typeof(List))] //强制关联到 List 组件
	[ChildComponent(typeof(Menu), Optional = true)] //不强制关联
	[HtmlTag("li")]
	public class ListItem : BlazorComponentBase
	{
		//一定要设置为 public
		[CascadingParameter]public List CascadingList { get; set; }

		// 如果是 Optional, 允许组件可为空类型
		[CascadingParameter]public Menu? CascadingMenu { get; set; }
	}
	```
## :six_pointed_star: HtmlHelper

* 在 .razor 文件中
    ```html
    <div class="@GetCssClass">
        ...
    </div>
    ```
    ```cs
    @code{
        string GetCssClass => HtmlHelper.CreateCssBuilder().Append("btn-primary").Append("active", Actived).ToString();
        
        [Parameter] public bool Actived { get; set; }
    }
    ```
* 在 `RenderTreeBuilder` 创建元素
    ```cs
    builder.CreateElement(0, "span", attributes: new { @class = HtmlHelper.CreateCssBuilder().Append("btn-primary").Append("active", Actived) });
    ```
* 创建动态事件
    ```cs
    protected override void BuildAttributes(IDictionary<string,object> attributes)
    {
        if(!Disabled)
        {
            attributes["onclick"] = HtmlHelper.CreateCallback<MouseEventArgs>(this, ()=> Clicked = true);
        }
    }
    ```
## :boom: 创建动态样式
在 `RenderTreeBuilder` 中
```cs
builder.CreateStyles(0, selector => {
    selector.AddStyle(".fade-in" , new { opacity = 1 })
            .AddStyle("#element", new { width = "120px", height = "80px", border_right="solid 1px #ccc"});
    selector.AddKeyFrames("FadeIn", k => {
        k.Add("from", new { width = "40px"}).Add("to", new { width = "150px"});
    })
});
```

生成样式：
```css
.fade-in {
    opacity:1;
}
#element {
    width:120px;
    height:80px;
    border-right:"solid 1px #ccc";
}
@keyframes FadeIn{
    from {
        width:40px;
    },
    to {
       width:150x; 
    }
}
```

**[查看文档](/wiki/readme.md)** 获取更多

## :computer: 支持环境
* .NET 6

## :blue_book: 安装使用

* 从 `Nuget.org` 安装
```bash
Install-Package ComponentBuilder
```

* 注册服务
```csharp
builder.Services.AddComponentBuilder();
```

## :link: 链接地址
* [问题反馈](/issues)
* [版本发布](/releases)
* [文档地址](/wiki/readme.md)