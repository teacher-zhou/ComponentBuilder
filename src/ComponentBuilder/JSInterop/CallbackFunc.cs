using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

/// <summary>
/// Represents a callback invoked by javascript with specified result.
/// </summary>
/// <typeparam name="TResult">The type of result.</typeparam>
public class CallbackFunc<TResult>
{
    private readonly Func<TResult> _callback;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallbackFunc{TResult}"/> class.
    /// </summary>
    /// <param name="callback">The callback invoked by javascript.</param>
    public CallbackFunc(Func<TResult> callback)
        => _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// Invoked by JS.
    /// </summary>
    /// <returns>The result with specified type.</returns>
    [JSInvokable]
    public TResult Invoke() => _callback();
}


/// <summary>
/// Represents a callback invoked by javascript with specified argument and spcified return type.
/// </summary>
/// <typeparam name="T">The type of argument.</typeparam>
/// <typeparam name="TResult">The type of result.</typeparam>
public class CallbackFunc<T, TResult>
{
    private readonly Func<T, TResult> _callback;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallbackFunc{TEventArgs,TResult}"/> class.
    /// </summary>
    /// <param name="callback">The callback invoked by javascript.</param>
    public CallbackFunc(Func<T, TResult> callback) => _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// Invoked by JS.
    /// </summary>
    /// <param name="argument">An argument passes from JS.</param>
    /// <returns>The result with specified type.</returns>
    [JSInvokable]
    public TResult Invoke(T argument) => _callback(argument);
}


/// <summary>
/// Represents a callback invoked by javascript with specified argument and spcified return type.
/// </summary>
/// <typeparam name="T1">The type of argument1.</typeparam>
/// <typeparam name="T2">The type of argument2.</typeparam>
/// <typeparam name="TResult">The type of result.</typeparam>
public class CallbackFunc<T1, T2, TResult>
{
    private readonly Func<T1, T2, TResult> _callback;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallbackFunc{TEventArgs,TResult}"/> class.
    /// </summary>
    /// <param name="callback">The callback invoked by javascript.</param>
    public CallbackFunc(Func<T1, T2, TResult> callback) => _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// Invoked by JS.
    /// </summary>
    /// <param name="arg1">An argument1 passes from JS.</param>
    /// <param name="arg2">An argument2 passes from JS.</param>
    /// <returns>The result with specified type.</returns>
    [JSInvokable]
    public TResult Invoke(T1 arg1, T2 arg2) => _callback(arg1, arg2);
}

/// <summary>
/// Represents a callback invoked by javascript with specified argument and spcified return type.
/// </summary>
/// <typeparam name="T1">The type of argument1.</typeparam>
/// <typeparam name="T2">The type of argument2.</typeparam>
/// <typeparam name="T3">The type of argument3.</typeparam>
/// <typeparam name="TResult">The type of result.</typeparam>
public class CallbackFunc<T1, T2, T3, TResult>
{
    private readonly Func<T1, T2, T3, TResult> _callback;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallbackFunc{TEventArgs,TResult}"/> class.
    /// </summary>
    /// <param name="callback">The callback invoked by javascript.</param>
    public CallbackFunc(Func<T1, T2, T3, TResult> callback) => _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// Invoked by JS.
    /// </summary>
    /// <param name="arg1">An argument1 passes from JS.</param>
    /// <param name="arg2">An argument2 passes from JS.</param>
    /// <param name="arg3">An argument3 passes from JS.</param>
    /// <returns>The result with specified type.</returns>
    [JSInvokable]
    public TResult Invoke(T1 arg1, T2 arg2, T3 arg3) => _callback(arg1, arg2, arg3);
}
