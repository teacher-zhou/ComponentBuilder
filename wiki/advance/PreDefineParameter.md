## 预定义参数
> 通过接口的形式将参数的 CSS，HTML 属性、事件等进行预定义，使参数可以被重用。

**命名规范：IHasXXXXX** 用 IHas 作为命名前缀

### 重复应用 CSS

##### 组件类
```cs
[CssClass("nav")]
public interface IHasNav
{
}

public class Nav : BlazorComponentBase, IHasNav
{

}

public class NavBar : BlazorComponent, IHasNav
{

}

```
```html
<Nav />
<div class="nav"></div>

<NavBar />
<div class="nav"></div>
```
##### 参数
```cs
public interface IHasTextColor
{
    [CssClass("text-")]Color? TextColor { get; set; }
}

public interface IHasBgColor
{
    [CssClass("bg-")]Color? BgColor { get; set; }
}

public class Button : BlazorComponentBase, IHasTextColor
{
    [Parameter]public Color? TextColor { get; set; }
}

public class Paper : BlazorComponentBase, IHasTextColor, IHasBgColor
{
    [Parameter]public Color? TextColor { get; set; }
    [Parameter]public Color? BgColor { get; set; }
}
```
```html
<Button TextColor="Color.Primary"></Button>
<div class="primary"></div>

<Paper TextColor="Color.Primary"></Paper>
<div class="primary"></div>
```

### 重写预定义
> 在实现接口的组件类中，使用 `CssClass` 即重写接口的预定义

重写组件类
```cs
[CssClass("nav")]
public interface IHasNav
{
}

public class Nav : BlazorComponentBase, IHasNav
{

}

[CssClass("navbar")]
public class NavBar : BlazorComponent, IHasNav
{

}
```
```html
<Nav />
<div class="nav"></div>

<NavBar />
<div class="navbar"></div>
```

重写参数
```cs
public interface IHasTextColor
{
    [CssClass("text-")]Color? TextColor { get; set; }
}

public interface IHasBgColor
{
    [CssClass("bg-")]Color? BgColor { get; set; }
}

public class Button : BlazorComponentBase, IHasTextColor
{
    [Parameter][CssClass("btn-text-")]public Color? TextColor { get; set; }
}

public class Paper : BlazorComponentBase, IHasTextColor, IHasBgColor
{
    [Parameter]public Color? TextColor { get; set; }
    [Parameter]public Color? BgColor { get; set; }
}
```
```html
<Button TextColor="Color.Primary"></Button>
<div class="btn-text-primary"></div>

<Paper TextColor="Color.Primary"></Paper>
<div class="primary"></div>
```

#### 内置预定义参数
由系统内置预定义的参数
* IHasChildContent
* IHasChildContent&lt;TValue>
* IHasActive
* IHasOnActive
* IHasDisabled
* IHasOnDisabled
* IHasOnClick