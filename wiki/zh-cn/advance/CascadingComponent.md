## 级联组件
> 即父子组件，嵌套组件。

父组件继承 `BlazorParentComponentBase<TParentComponent>` 或 `BlazorParentComponentBase<TParentComponent, TChildComponent>`

> `BlazorParentComponentBase<TParentComponent, TChildComponent>` 会对子组件进行强验证，即子组件如果没有包含在相关父组件里使用，会抛出异常。

```cs

//父组件
public class ParentComponent : BlazorParentComponentBase<ParentComponent>
{
    //...
}


//级联组件强关联
public class ParentComponent : BlazorParentComponentBase<ParntComponent, ChildComponent>
{

}

```

子组件使用一般方式或继承 `BlazorChildComponentBase<TParentComponent>` 或`BlazorChildComponentBase<TParentComponent, TChildComponent>` 基类

* 常规子组件
    ```cs
    public class ChildComponent : BlazorComponentBase
    {
        //子组件通过 CascadingParemeter 接收父组件
        [CascadingParameter]ParentComponent Parent { get; set; }
    }
    ```

* 强关联父组件
    ```cs
    public class ChildComponent : BlazorChildComponentBase<ParentComponent>
    {
        protected override void OnInitialized()
        {
            // 使用 base.ParentComponent //属性获得级联的父组件对象
        }
    }
    ```


* 强关联父组件且可以操作子组件
    ```cs
    public class ParentComponent : BlazorParentComponentBase<ParntComponent, ChildComponent>
    {
        protected override void OnInitialized()
        {
            // 使用 base.ChildComponents 获得所有的子组件
        }
    }

    public class ChildComponent : BlazorChildComponentBase<ParentComponent, ChildComponent>
    {
        protected override void OnInitialized()
        {
            // 使用 base.ParentComponent //属性获得级联的父组件对象
        }
    }
    ```