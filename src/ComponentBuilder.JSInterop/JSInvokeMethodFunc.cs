using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

/// <summary>
/// 表示 JS 的回调并返回一个结果。
/// </summary>
/// <typeparam name="TResult">结果的类型。</typeparam>
public class JSInvokeMethodFunc<TResult>
{
    private readonly Func<TResult> _callback;

    /// <summary>
    /// 初始化 <see cref="JSInvokeMethodFunc{TResult}"/> 类的新实例。
    /// </summary>
    /// <param name="callback">JS 要执行的 C# 回调代码。</param>
    public JSInvokeMethodFunc(Func<TResult> callback)
        => _callback = callback ?? throw new ArgumentNullException(nameof(callback));


    /// <summary>
    /// 该方法会被 JS 执行。确保 JS 的代码是 <c>let result = dotnetHelper.invokeMethodAsync("Invoke");</c> 的调用方式。
    /// </summary>
    /// <returns>指定类型的返回值。</returns>
    [JSInvokable]
    public TResult Invoke() => _callback();
}


/// <summary>
/// 表示 JS 调用的方法具备 1 个输入参数并返回一个结果。
/// </summary>
/// <typeparam name="T">参数类型。</typeparam>
/// <typeparam name="TResult">结果的类型。</typeparam>
public class JSInvokeMethodFunc<T, TResult>
{
    private readonly Func<T, TResult> _callback;

    /// <summary>
    /// 初始化 <see cref="JSInvokeMethodFunc{T, TResult}"/> 类的新实例。
    /// </summary>
    /// <param name="callback">JS 要执行的 C# 回调代码。</param>
    public JSInvokeMethodFunc(Func<T, TResult> callback) => _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// 该方法会被 JS 执行。确保 JS 的代码是 <c>let result = dotnetHelper.invokeMethodAsync("Invoke", arg);</c> 的调用方式。
    /// </summary>
    /// <param name="arg">JS 传入的参数。</param>
    /// <returns>指定类型的返回值。</returns>
    [JSInvokable]
    public TResult Invoke(T arg) => _callback(arg);
}


/// <summary>
/// 表示 JS 调用的方法具备 2 个输入参数并返回一个结果。
/// </summary>
/// <typeparam name="T1">参数1类型。</typeparam>
/// <typeparam name="T2">参数2类型。</typeparam>
/// <typeparam name="TResult">结果的类型。</typeparam>
public class JSInvokeMethodFunc<T1, T2, TResult>
{
    private readonly Func<T1, T2, TResult> _callback;

    /// <summary>
    /// 初始化 <see cref="JSInvokeMethodFunc{T1, T2, TResult}"/> 类的新实例。
    /// </summary>
    /// <param name="callback">JS 要执行的 C# 回调代码。</param>
    public JSInvokeMethodFunc(Func<T1, T2, TResult> callback) => _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// 该方法会被 JS 执行。确保 JS 的代码是 <c>let result = dotnetHelper.invokeMethodAsync("Invoke", arg1, arg2);</c> 的调用方式。
    /// </summary>
    /// <param name="arg1">JS 传入的参数1。</param>
    /// <param name="arg2">JS 传入的参数2。</param>
    /// <returns>指定类型的返回值。</returns>
    [JSInvokable]
    public TResult Invoke(T1 arg1, T2 arg2) => _callback(arg1, arg2);
}

/// <summary>
/// 表示 JS 调用的方法具备 3 个输入参数并返回一个结果。
/// </summary>
/// <typeparam name="T1">参数1类型。</typeparam>
/// <typeparam name="T2">参数2类型。</typeparam>
/// <typeparam name="T3">参数3类型。</typeparam>
/// <typeparam name="TResult">结果的类型。</typeparam>
public class JSInvokeMethodFunc<T1, T2, T3, TResult>
{
    private readonly Func<T1, T2, T3, TResult> _callback;

    /// <summary>
    /// 初始化 <see cref="JSInvokeMethodFunc{T1, T2, T3, TResult}"/> 类的新实例。
    /// </summary>
    /// <param name="callback">JS 要执行的 C# 回调代码。</param>
    public JSInvokeMethodFunc(Func<T1, T2, T3, TResult> callback) => _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// 该方法会被 JS 执行。确保 JS 的代码是 <c>let result = dotnetHelper.invokeMethodAsync("Invoke", arg1, arg2, arg3);</c> 的调用方式。
    /// </summary>
    /// <param name="arg1">JS 传入的参数1。</param>
    /// <param name="arg2">JS 传入的参数2。</param>
    /// <param name="arg3">JS 传入的参数3。</param>
    /// <returns>指定类型的返回值。</returns>
    [JSInvokable]
    public TResult Invoke(T1 arg1, T2 arg2, T3 arg3) => _callback(arg1, arg2, arg3);
}
