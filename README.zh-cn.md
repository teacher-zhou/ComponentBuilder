# ComponentBuilder
一个通过 `RenderTreeBuilder` 创建 Blazor 组件的自动化框架。

## :sparkles: 特点

* 它是一个框架，而不是一个组件库
* 根据属性定义自动构建组件
* 强大的' RenderTreeBuilder '扩展
* 支持模块化JS动态导入和调用
* 任何HTML元素布局的灵活性
* 编写逻辑来呈现不同的组件
* 具象思维的挑战

## :rainbow: 定义组件

```csharp
[HtmlTag("button")] // 元素名称
[CssClass("btn")] // 元素固定的 class
public class MyButton : BlazorComponentBase, IHasChildContent, IHasOnClick
{
	[Parameter][CssClass("active")]public bool Active { get; set; } // true 时追加 active 的 CSS
	
	[Parameter][CssClass("btn-")]public Color? Color { get; set; } // 和枚举项的 CSS 合并成一个

	[Parameter]public RenderFragment? ChildContent { get; set; } // 支持子内容

	[Parameter][HtmlData("tooltip")]public string? Tooltip { get; set; } // 生成 data-tooltip 属性

	[Parameter][HtmlEvent("onclick")]public EventCallback<ClickEventArgs> OnClick { get; set; } //自动注册一个 onclick 事件给元素

        [Parameter][HtmlAttribute]public string? Title { get; set; } //生成 title 属性
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
<MyButton Active Tooltip="active button" Color="Color.Information" Title="click me">Active Button</MyButton>
<!--html-->
<button class="btn btn-info active" data-tooltip="active button" title="click me">Active Button</button>
```

## :key: JS 引入和调用

```js
//in app.js
export function display(){
 // ...your code
}
```

```cs
[Inject]IJSRuntime JS { get; set; }

var js = await JS.Value.ImportAsync("./app.js");
js.display(); // 和函数名一样
```

## :large_blue_circle: 创建元素

```cs
protected override void BuildRenderTree(RenderTreeBuilder builder)
{
    builder.Open("div")
            .Class("my-class", (IsActive, "active"), (!string.IsNullOrEmpty(Name), "text-block"))
            .Style((Size.HasValue, $"font-size:{Size}px"))
            .Content("hello world")
           .Close();
}
```

## :large_orange_diamond: 创建组件

```cs
protected override void BuildRenderTree(RenderTreeBuilder builder)
{
    builder.Open<Button>()
            .Class("my-class", (IsActive, "active"), (!string.IsNullOrEmpty(Name), "text-block"))
            .Style((Size.HasValue, $"font-size:{Size}px"))
            .Content(ChildContent)
           .Close();
}
```

## :children_crossing: 父子组件关联

* 父组件

```cs
[ParentComponent] //be cascading parameter for this component
[HtmlTag("ul")]
public class List : BlazorComponentBase, IHasChildContent
{

}
```

* 子组件

```cs
[ChildComponent(typeof(List))] //Strong association with List
[ChildComponent(typeof(Menu), Optional = true)] //Soft association
[HtmlTag("li")]
public class ListItem : BlazorComponentBase
{        
        // Required
    [CascadingParameter]public List CascadingList { get; set; }

    // Optional, maybe null
    [CascadingParameter]public Menu? CascadingMenu { get; set; }
}
```
* 用法

```html
<List>
    <ListItem>...</ListItem>
</List>

<ListItem /> <!--不在父组件 List 中，抛出异常-->

<Menu>
    <ListItem>...</ListItem>
</Menu>
```

## :six_pointed_star: HtmlHelper

* 在 `.razor` 文件

```html
<div class="@GetCssClass">
    ...
</div>
```

```cs
@code{
    string GetCssClass => HtmlHelper.Class.Append("btn-primary").Append("active", Actived).ToString();
        
    [Parameter] public bool Actived { get; set; }
}
```

* 动态元素的属性

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

* 逻辑代码的支持

  * 构建 CSS

    ```cs
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        if(User.Identity.IsAuthenticated)
        {
            builder.Append("user-plus");
        }
    }
    ```

  * 构建 style

    ```cs
    protected override void BuildStlye(IStyleBuilder builder)
    {
        if(IsAdmin)
        {
            builder.Append("display:block");
        }
    }
    ```

  * 构建属性

    ```cs
    protected override void BuildAttributes(IDictionary<string,object> attributes)
    {
        if(!Disabled)
        {
            attributes["onclick"] = HtmlHelper.Event.Create<MouseEventArgs>(this, ()=> Clicked = true);
        }
    }
    ```

## :boom: 动态样式

```cs
builder.CreateStyleRegion(0, selector => {
    selector.AddStyle(".fade-in" , 
                        new { 
                            opacity = 1 
                        })
            .AddStyle("#element", 
                        new { 
                            width = "120px", 
                            height = "80px", 
                            border_right="solid 1px #ccc"
                        });

    selector.AddKeyFrames("FadeIn", k => {
        k.Add("from", 
                new { 
                    width = "40px"，
                    height = "150px"
                })
        .Add("to", 
                new { 
                    width = "150px",
                    height = "30px"
                });
    })
});
```

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
        height:150px;
    },
    to {
       width:150x;
       height:30px 
    }
}
```

## :computer: 环境

* .NET 6

## :blue_book: 安装

* `Nuget.org` 安装

```bash
Install-Package ComponentBuilder
```

* 注册服务
  
```csharp
builder.Services.AddComponentBuilder();
```

## :link: 链接
* [问题反馈](https://github.com/AchievedOwner/ComponentBuilder/issues)
* [版本发布](https://github.com/AchievedOwner/ComponentBuilder/releases)
* [参考文档](https://github.com/AchievedOwner/ComponentBuilder/wiki)