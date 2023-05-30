namespace ComponentBuilder;


/// <summary>
/// 应用于组件类，表示当前组件是一个子组件，会与标记了 <see cref="ParentComponentAttribute"/> 的父组件进行嵌套校验。可以有多个关联。
/// <para>
/// 若父组件标记了 <see cref="ParameterAttribute"/> 时，子组件可以通过 <see cref="CascadingParameterAttribute"/> 自动获得其级联参数的值，但该级联参数必须是 <c>public</c> 修饰符。
/// </para>
/// <para>示例代码：</para>
/// <code language="cs">
/// [ParentComponent]
/// public class ParentComponent : BlazorComponentBase { }
/// 
/// [ChildComponent(typeof(ParentComponent))]
/// public class ChildComponent : BlazorComponentBase 
/// {
///     [CascadingParameter]public ParentComponent Parent { get; set; }
/// }
/// </code>
/// <para>
/// 在 razor 组件中的使用：
/// <code language="html">
/// &lt;ParentComponent>
///     &lt;ChildComponent />
/// &lt;/ParentComponent>
/// </code>
/// </para>
/// <para>
/// 如果子组件不嵌套在指定的父组件中，则会引发异常：
/// <code language="html">
/// &lt;ChildComponent /> // 抛出异常
/// </code>
/// </para>
/// <para>
/// 设置 <see cref="ChildComponentAttribute.Optional"/> 为 <c>true</c> 将不引发嵌套校验异常，但父组件的级联参数的值可能为 <c>null</c>，因此需要增加可为空（?）操作符给级联参数提醒用户。
/// <code language="cs">
/// [ParentComponent]
/// public class ParentComponent : BlazorComponentBase { }
/// 
/// [ChildComponent(typeof(ParentComponent), Optional = true)]
/// public class ChildComponent : BlazorComponentBase 
/// {
///     [CascadingParameter]public ParentComponent? Parent { get; set; } //增加可为空操作符
/// }
/// </code>
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ChildComponentAttribute : Attribute
{
    /// <summary>
    /// 使用指定的组件类型初始化 <see cref="ChildComponentAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="componentType">父组件的类型。</param>
    public ChildComponentAttribute(Type componentType)
    {
        ComponentType = componentType ?? throw new ArgumentNullException(nameof(componentType));
    }

    /// <summary>
    /// 获取父组件的类型。
    /// </summary>
    public Type ComponentType { get; }

    /// <summary>
    /// 获取或设置一个布尔值，表示子组件可以不对父组件进行嵌套校验。
    /// </summary>
    public bool Optional { get; set; }
}

#if NET7_0_OR_GREATER


/// <summary>
/// 应用于组件类，表示当前组件是一个子组件，会与标记了 <see cref="ParentComponentAttribute"/> 的父组件进行嵌套校验。可以有多个关联。
/// <para>
/// 若父组件标记了 <see cref="ParameterAttribute"/> 时，子组件可以通过 <see cref="CascadingParameterAttribute"/> 自动获得其级联参数的值，但该级联参数必须是 <c>public</c> 修饰符。
/// </para>
/// <para>示例代码：</para>
/// <code language="cs">
/// [ParentComponent]
/// public class ParentComponent : BlazorComponentBase { }
/// 
/// [ChildComponent&lt;ParentComponent>()]
/// public class ChildComponent : BlazorComponentBase 
/// {
///     [CascadingParameter]public ParentComponent Parent { get; set; }
/// }
/// </code>
/// <para>
/// 在 razor 组件中的使用：
/// <code language="html">
/// &lt;ParentComponent>
///     &lt;ChildComponent />
/// &lt;/ParentComponent>
/// </code>
/// </para>
/// <para>
/// 如果子组件不嵌套在指定的父组件中，则会引发异常：
/// <code language="html">
/// &lt;ChildComponent /> // 抛出异常
/// </code>
/// </para>
/// <para>
/// 设置 <see cref="ChildComponentAttribute.Optional"/> 为 <c>true</c> 将不引发嵌套校验异常，但父组件的级联参数的值可能为 <c>null</c>，因此需要增加可为空（?）操作符给级联参数提醒用户。
/// <code language="cs">
/// [ParentComponent]
/// public class ParentComponent : BlazorComponentBase { }
/// 
/// [ChildComponent&lt;ParentComponent>(Optional = true)]
/// public class ChildComponent : BlazorComponentBase 
/// {
///     [CascadingParameter]public ParentComponent? Parent { get; set; } //增加可为空操作符
/// }
/// </code>
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ChildComponentAttribute<TComponent>: ChildComponentAttribute where TComponent:IComponent
{
    /// <summary>
    /// 初始化 <see cref="ChildComponentAttribute{TComponent}"/> 类的新实例。
    /// </summary>
    public ChildComponentAttribute():base(typeof(TComponent))
    {
    }
}
#endif