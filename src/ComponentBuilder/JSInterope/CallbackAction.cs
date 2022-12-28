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
/// <typeparam name="TEventArgs">The type of argument.</typeparam>
public class CallbackAction<TEventArgs> where TEventArgs : class
{
    private readonly Action<TEventArgs> _callback;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallbackAction"/> class.
    /// </summary>
    /// <param name="callback">The callback to invoke by js.</param>
    public CallbackAction(Action<TEventArgs> callback)
        => this._callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// Invoked by js.
    /// </summary>
    /// <param name="argument">An argument passes from JS.</param>
    [JSInvokable]
    public void Invoke(TEventArgs argument) => _callback(argument);
}