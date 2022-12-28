using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterope;

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
        => this._callback = callback ?? throw new ArgumentNullException(nameof(callback));

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
/// <typeparam name="TEventArgs">The type of argument.</typeparam>
/// <typeparam name="TResult">The type of result.</typeparam>
public class CallbackFunc<TEventArgs, TResult> where TEventArgs : class
{
    private readonly Func<TEventArgs, TResult> _callback;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallbackFunc{TEventArgs,TResult}"/> class.
    /// </summary>
    /// <param name="callback">The callback invoked by javascript.</param>
    public CallbackFunc(Func<TEventArgs, TResult> callback)
    {
        this._callback = callback;
    }

    /// <summary>
    /// Invoked by JS.
    /// </summary>
    /// <param name="argument">An argument passes from JS.</param>
    /// <returns>The result with specified type.</returns>
    [JSInvokable]
    public TResult Invoke(TEventArgs argument) => _callback(argument);
}
