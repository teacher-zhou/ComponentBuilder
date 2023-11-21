using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

/// <summary>
/// Represents the JS callback and returns a result.
/// </summary>
/// <typeparam name="TResult">The result type.</typeparam>
/// <param name="callback">The callback of C# for js invoke.</param>
public class JSInvokeMethodFunc<TResult>(Func<TResult> callback)
{
    private readonly Func<TResult> _callback = callback ?? throw new ArgumentNullException(nameof(callback));


    /// <summary>
    /// This method is executed by JS. 
    /// <para>
    /// Ensure JS code like <c>let result = dotnetHelper.invokeMethodAsync("Invoke");</c>.
    /// </para>
    /// </summary>
    /// <returns>The result value.</returns>
    [JSInvokable]
    public TResult Invoke() => _callback();
}


/// <summary>
/// Represents the JS callback and returns a result with 1 input parameter.
/// </summary>
/// <typeparam name="TResult">The result type.</typeparam>
/// <typeparam name="T">Argument type.</typeparam>
/// <param name="callback">The callback of C# for js invoke.</param>
public class JSInvokeMethodFunc<T, TResult>(Func<T, TResult> callback)
{
    private readonly Func<T, TResult> _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// This method is executed by JS. 
    /// <para>
    /// Ensure JS code like <c>let result = dotnetHelper.invokeMethodAsync("Invoke", arg1);</c>.
    /// </para>
    /// </summary>
    /// <returns>The result value.</returns>
    [JSInvokable]
    public TResult Invoke(T arg) => _callback(arg);
}


/// <summary>
/// Represents the JS callback and returns a result with 2 input parameter.
/// </summary>
/// <typeparam name="TResult">The result type.</typeparam>
/// <typeparam name="T1">Argument type.</typeparam>
/// <typeparam name="T2">Argument type.</typeparam>
/// <param name="callback">The callback of C# for js invoke.</param>
public class JSInvokeMethodFunc<T1, T2, TResult>(Func<T1, T2, TResult> callback)
{
    private readonly Func<T1, T2, TResult> _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// This method is executed by JS. 
    /// <para>
    /// Ensure JS code like <c>let result = dotnetHelper.invokeMethodAsync("Invoke", arg1, arg2);</c>.
    /// </para>
    /// </summary>
    /// <returns>The result value.</returns>
    [JSInvokable]
    public TResult Invoke(T1 arg1, T2 arg2) => _callback(arg1, arg2);
}


/// <summary>
/// Represents the JS callback and returns a result with 3 input parameter.
/// </summary>
/// <typeparam name="TResult">The result type.</typeparam>
/// <typeparam name="T1">Argument1 type.</typeparam>
/// <typeparam name="T2">Argument2 type.</typeparam>
/// <typeparam name="T3">Argument3 type.</typeparam>
/// <param name="callback">The callback of C# for js invoke.</param>
public class JSInvokeMethodFunc<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> callback)
{
    private readonly Func<T1, T2, T3, TResult> _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// This method is executed by JS. 
    /// <para>
    /// Ensure JS code like <c>let result = dotnetHelper.invokeMethodAsync("Invoke", arg1, arg2, arg3);</c>.
    /// </para>
    /// </summary>
    /// <returns>The result value.</returns>
    [JSInvokable]
    public TResult Invoke(T1 arg1, T2 arg2, T3 arg3) => _callback(arg1, arg2, arg3);
}
