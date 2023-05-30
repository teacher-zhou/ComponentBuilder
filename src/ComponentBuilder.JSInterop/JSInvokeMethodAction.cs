using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

/// <summary>
/// 表示 JS 的回调。
/// </summary>
public class JSInvokeMethodAction
{
    private readonly Action _callback;

    /// <summary>
    /// 初始化 <see cref="JSInvokeMethodAction"/> 类的新实例。
    /// </summary>
    /// <param name="callback">JS 要执行的 C# 回调代码。</param>
    public JSInvokeMethodAction(Action callback)
        => _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// 该方法会被 JS 执行。确保 JS 的代码是 <c>dotnetHelper.invokeMethodAsync("Invoke");</c> 的调用方式。
    /// </summary>
    [JSInvokable]
    public void Invoke() => _callback();
}

/// <summary>
/// 表示 JS 调用的方法具备 1 个输入参数。
/// </summary>
/// <typeparam name="T">参数类型。</typeparam>
public class JSInvokeMethodAction<T>
{
    private readonly Action<T> _callback;

    /// <summary>
    /// 初始化 <see cref="JSInvokeMethodAction{T}"/> 类的新实例。
    /// </summary>
    /// <param name="callback">JS 要执行的 C# 回调代码。</param>
    public JSInvokeMethodAction(Action<T> callback)
        => _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// 该方法会被 JS 执行。确保 JS 的代码是 <c>dotnetHelper.invokeMethodAsync("Invoke", arg);</c> 的调用方式。
    /// </summary>
    /// <param name="arg">JS 传入的参数。</param>
    [JSInvokable]
    public void Invoke(T arg) => _callback(arg);
}

/// <summary>
/// 表示 JS 调用的方法具备 2 个输入参数。
/// </summary>
/// <typeparam name="T1">参数1类型。</typeparam>
/// <typeparam name="T2">参数2类型。</typeparam>
public class JSInvokeMethodAction<T1, T2>
{
    private readonly Action<T1, T2> _callback;

    /// <summary>
    /// 初始化 <see cref="JSInvokeMethodAction{T1,T2}"/> 类的新实例。
    /// </summary>
    /// <param name="callback">JS 要执行的 C# 回调代码。</param>
    public JSInvokeMethodAction(Action<T1, T2> callback)
        => _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// 该方法会被 JS 执行。确保 JS 的代码是 <c>dotnetHelper.invokeMethodAsync("Invoke", arg1, arg2);</c> 的调用方式。
    /// </summary>
    /// <param name="arg1">JS 传入的参数1。</param>
    /// <param name="arg2">JS 传入的参数2。</param>
    [JSInvokable]
    public void Invoke(T1 arg1, T2 arg2) => _callback(arg1, arg2);
}


/// <summary>
/// 表示 JS 调用的方法具备 3 个输入参数。
/// </summary>
/// <typeparam name="T1">参数1类型。</typeparam>
/// <typeparam name="T2">参数2类型。</typeparam>
/// <typeparam name="T3">参数3类型。</typeparam>
public class JSInvokeMethodAction<T1, T2, T3>
{
    private readonly Action<T1, T2, T3> _callback;

    /// <summary>
    /// 初始化 <see cref="JSInvokeMethodAction{T1,T2,T3}"/> 类的新实例。
    /// </summary>
    /// <param name="callback">JS 要执行的 C# 回调代码。</param>
    public JSInvokeMethodAction(Action<T1, T2, T3> callback)
        => _callback = callback ?? throw new ArgumentNullException(nameof(callback));


    /// <summary>
    /// 该方法会被 JS 执行。确保 JS 的代码是 <c>dotnetHelper.invokeMethodAsync("Invoke", arg1, arg2, arg3);</c> 的调用方式。
    /// </summary>
    /// <param name="arg1">JS 传入的参数1。</param>
    /// <param name="arg2">JS 传入的参数2。</param>
    /// <param name="arg3">JS 传入的参数3。</param>
    [JSInvokable]
    public void Invoke(T1 arg1, T2 arg2, T3 arg3) => _callback(arg1, arg2, arg3);
}