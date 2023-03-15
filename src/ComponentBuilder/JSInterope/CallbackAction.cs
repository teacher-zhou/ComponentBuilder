using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterope;

/// <summary>
/// Represents a callback invoked in javascript.
/// </summary>
public class CallbackAction
{
    private readonly Action _callback;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallbackAction"/> class.
    /// </summary>
    /// <param name="callback">The callback to invoke by js.</param>
    public CallbackAction(Action callback)
        => this._callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// Invoked by js.
    /// </summary>
    [JSInvokable]
    public void Invoke() => _callback();
}

/// <summary>
/// Represents a callback invoked in javascript with argument.
/// </summary>
/// <typeparam name="T">The type of argument.</typeparam>
public class CallbackAction<T>
{
    private readonly Action<T> _callback;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallbackAction"/> class.
    /// </summary>
    /// <param name="callback">The callback to invoke by js.</param>
    public CallbackAction(Action<T> callback)
        => this._callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// Invoked by js.
    /// </summary>
    /// <param name="arg">An argument passes from JS.</param>
    [JSInvokable]
    public void Invoke(T arg) => _callback(arg);
}

/// <summary>
/// Represents a callback invoked in javascript with argument.
/// </summary>
/// <typeparam name="T1">The type of argument1.</typeparam>
/// <typeparam name="T2">The type of argument2.</typeparam>
public class CallbackAction<T1, T2>
{
    private readonly Action<T1, T2> _callback;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallbackAction"/> class.
    /// </summary>
    /// <param name="callback">The callback to invoke by js.</param>
    public CallbackAction(Action<T1, T2> callback)
        => this._callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// Invoked by js.
    /// </summary>
    /// <param name="arg1">An argument1 passes from JS.</param>
    /// <param name="arg2">An argument2 passes from JS.</param>
    [JSInvokable]
    public void Invoke(T1 arg1, T2 arg2) => _callback(arg1, arg2);
}


/// <summary>
/// Represents a callback invoked in javascript with argument.
/// </summary>
/// <typeparam name="T1">The type of argument1.</typeparam>
/// <typeparam name="T2">The type of argument2.</typeparam>
/// <typeparam name="T3">The type of argument3.</typeparam>
public class CallbackAction<T1, T2,T3>
{
    private readonly Action<T1, T2, T3> _callback;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallbackAction"/> class.
    /// </summary>
    /// <param name="callback">The callback to invoke by js.</param>
    public CallbackAction(Action<T1, T2, T3> callback)
        => this._callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// Invoked by js.
    /// </summary>
    /// <param name="arg1">An argument1 passes from JS.</param>
    /// <param name="arg2">An argument2 passes from JS.</param>
    /// <param name="arg3">An argument3 passes from JS.</param>
    [JSInvokable]
    public void Invoke(T1 arg1, T2 arg2, T3 arg3) => _callback(arg1, arg2, arg3);
}