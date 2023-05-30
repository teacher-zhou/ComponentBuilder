namespace ComponentBuilder;

/// <summary>
/// 表示能满足执行的条件。
/// </summary>
public readonly struct Condition
{
    readonly Func<bool>? _execution = default;
    readonly bool? _valid = default;

    /// <summary>
    /// 初始化 <see cref="Condition"/> 类的新实例。
    /// </summary>
    /// <param name="valid">条件满足的结果。</param>
    public Condition(bool valid) => _valid = valid;

    /// <summary>
    /// 初始化 <see cref="Condition"/> 类的新实例。
    /// </summary>
    /// <param name="execution">条件满足的执行操作。</param>
    public Condition(Func<bool> execution) => _execution = execution;

    /// <summary>
    /// 获取条件满足的执行结果。
    /// </summary>
    public bool Result => _valid ?? _execution?.Invoke() ?? false;

    /// <summary>
    /// 执行条件的动作。
    /// </summary>
    /// <param name="condition">若是 <c>null</c> 则条件返回 <c>true</c>。</param>
    /// <param name="true">条件是 <c>true</c> 时要执行的操作。</param>
    /// <param name="false">条件是 <c>false</c> 时要执行的操作。</param>
    public static void Execute(Condition? condition, Action @true, Action? @false = default)
    {
        if ( Assert(condition) )
        {
            @true();
        }
        else
        {
            @false?.Invoke();
        }
    }

    /// <summary>
    /// 执行条件的动作并返回指定类型的结果。
    /// </summary>
    /// <typeparam name="TResult">返回类型。</typeparam>
    /// <param name="condition">若是 <c>null</c> 则条件返回 <c>true</c>。</param>
    /// <param name="true">条件是 <c>true</c> 时要执行的操作。</param>
    /// <param name="false">条件是 <c>false</c> 时要执行的操作。</param>
    public static TResult Execute<TResult>(Condition? condition, Func<TResult> @true, Func<TResult> @false) 
        => Assert(condition) ? @true() : @false();

    /// <summary>
    /// 断言指定的条件是否满足。
    /// </summary>
    /// <param name="condition">若是 <c>null</c> 则条件返回 <c>true</c>。</param>
    /// <returns>若 <paramref name="condition"/> 是 <c>null</c> 或 <see cref="Condition.Result"/> 是 <c>true</c>，则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
    public static bool Assert(Condition? condition)
        => condition is null || condition.Value.Result;

    /// <summary>
    /// 将 <c>bool</c> 值转换为 <see cref="Condition"/> 类。
    /// </summary>
    /// <param name="value">布尔值。</param>
    public static implicit operator Condition(bool value) => new(value);
    /// <summary>
    /// 将返回 bool 值的函数转换为 <see cref="Condition"/> 类。
    /// </summary>
    /// <param name="value">可返回布尔值的函数。</param>
    public static implicit operator Condition(Func<bool> value) => new(value);
}
