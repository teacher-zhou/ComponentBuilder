namespace ComponentBuilder.Parameters;

/// <summary>
/// 提供组件具备级联参数。需要自行添加 <see cref="CascadingParameterAttribute"/> 特性。
/// </summary>
/// <typeparam name="TValue">级联参数的值类型。</typeparam>
public interface IHasCascadingParameter<TValue>
{
    /// <summary>
    /// 获取级联参数。
    /// </summary>
    TValue? CascadingValue { get; set; }
}
