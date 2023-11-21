using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

/// <summary>
/// JS callback action.
/// </summary>
/// <param name="callback">The callback of C# for js invoke.</param>
/// <exception cref="System.ArgumentNullException">callback</exception>
public class JSInvokeMethodAction(Action callback)
{
    private readonly Action _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// This method is executed by JS. 
    /// <para>
    /// To ensure the JS code is <c> dotnetHelper.InvokeMethodAsync("Invoke");</c>.
    /// </para>
    /// </summary>
    [JSInvokable]
    public void Invoke() => _callback();
}

/// <summary>
/// Indicates that the method called by JS has 1 input parameter.
/// </summary>
/// <typeparam name="T">Argument type.</typeparam>
/// <param name="callback">The callback of C# for js invoke.</param>
public class JSInvokeMethodAction<T>(Action<T> callback)
{
    private readonly Action<T> _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// This method is executed by JS. 
    /// <para>
    /// To ensure the JS code is <c> dotnetHelper.InvokeMethodAsync("Invoke", arg1);</c>.
    /// </para>
    /// </summary>
    [JSInvokable]
    public void Invoke(T arg) => _callback(arg);
}

/// <summary>
/// Indicates that the method called by JS has 2 input parameter.
/// </summary>
/// <typeparam name="T1">Argument1 type.</typeparam>
/// <typeparam name="T2">Argument2 type.</typeparam>
/// <param name="callback">The callback of C# for js invoke.</param>
public class JSInvokeMethodAction<T1, T2>(Action<T1, T2> callback)
{
    private readonly Action<T1, T2> _callback = callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>
    /// This method is executed by JS. 
    /// <para>
    /// To ensure the JS code is <c> dotnetHelper.InvokeMethodAsync("Invoke", arg1, arg2);</c>.
    /// </para>
    /// </summary>
    [JSInvokable]
    public void Invoke(T1 arg1, T2 arg2) => _callback(arg1, arg2);
}


/// <summary>
/// Indicates that the method called by JS has 3 input parameter.
/// </summary>
/// <typeparam name="T1">Argument1 type.</typeparam>
/// <typeparam name="T2">Argument2 type.</typeparam>
/// <typeparam name="T3">Argument2 type.</typeparam>
/// <param name="callback">The callback of C# for js invoke.</param>
public class JSInvokeMethodAction<T1, T2, T3>(Action<T1, T2, T3> callback)
{
    private readonly Action<T1, T2, T3> _callback = callback ?? throw new ArgumentNullException(nameof(callback));


    /// <summary>
    /// This method is executed by JS. 
    /// <para>
    /// To ensure the JS code is <c> dotnetHelper.InvokeMethodAsync("Invoke", arg1, arg2, arg3);</c>.
    /// </para>
    /// </summary>
    [JSInvokable]
    public void Invoke(T1 arg1, T2 arg2, T3 arg3) => _callback(arg1, arg2, arg3);
}