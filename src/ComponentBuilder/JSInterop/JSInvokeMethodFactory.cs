using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;
/// <summary>
/// A factory that creates callbacks for javascript functions.
/// </summary>
public static class JSInvokeCallbackFactory
{
    #region CallbackAction
    /// <summary>
    /// Create a callback for a javascript function.
    /// <code>
    /// function(dotNetHelper){
    ///     dotNetHelper.invokeMethodAsync("Invoke");
    /// }
    /// </code>
    /// </summary>
    /// <param name="callback">要执行的回调。</param>
    public static DotNetObjectReference<JSInvokeCallbackAction> Create(Action callback)
    => DotNetObjectReference.Create(new JSInvokeCallbackAction(callback));

    /// <summary>
    /// Create a callback for a javascript function.
    /// <code>
    /// function(dotNetHelper){
    ///     dotNetHelper.invokeMethodAsync("Invoke", arg);
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T">Argument type.</typeparam>
    /// <param name="callback">The callback.</param>
    public static DotNetObjectReference<JSInvokeCallbackAction<T>> Create<T>(Action<T> callback)
    => DotNetObjectReference.Create(new JSInvokeCallbackAction<T>(callback));

    /// <summary>
    /// Create a callback for a javascript function.
    /// <code>
    /// function(dotNetHelper){
    ///     dotNetHelper.invokeMethodAsync("Invoke", arg1, arg2);
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T1">Argument1 type.</typeparam>
    /// <typeparam name="T2">Argument2 type.</typeparam>
    /// <param name="callback">The callback.</param>
    public static DotNetObjectReference<JSInvokeCallbackAction<T1, T2>> Create<T1, T2>(Action<T1, T2> callback)
    => DotNetObjectReference.Create(new JSInvokeCallbackAction<T1, T2>(callback));

    /// <summary>
    /// Create a callback for a javascript function.
    /// <code>
    /// function(dotNetHelper){
    ///     dotNetHelper.invokeMethodAsync("Invoke", arg1, arg2, arg3);
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T1">Argument1 type.</typeparam>
    /// <typeparam name="T2">Argument2 type.</typeparam>
    /// <typeparam name="T3">Argument3 type.</typeparam>
    /// <param name="callback">The callback.</param>
    public static DotNetObjectReference<JSInvokeCallbackAction<T1, T2, T3>> Create<T1, T2, T3>(Action<T1, T2, T3> callback)
    => DotNetObjectReference.Create(new JSInvokeCallbackAction<T1, T2, T3>(callback));
    #endregion

    #region CallbackFunc
    /// <summary>
    /// Create a callback for a javascript function with a return value.
    /// <code>
    /// function(dotNetHelper){
    ///    let result = dotNetHelper.invokeMethodAsync("Invoke");
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="callback">The callback.</param>
    public static DotNetObjectReference<JSInvokeCallbackFunc<TResult>> Create<TResult>(Func<TResult> callback)
    => DotNetObjectReference.Create(new JSInvokeCallbackFunc<TResult>(callback));

    /// <summary>
    /// Create a callback for a javascript function with a return value.
    /// <code>
    /// function(dotNetHelper){
    ///    let result = dotNetHelper.invokeMethodAsync("Invoke", arg);
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T">Argument type.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="callback">The callback.</param>
    public static DotNetObjectReference<JSInvokeCallbackFunc<T, TResult>> Create<T, TResult>(Func<T, TResult> callback)
    => DotNetObjectReference.Create(new JSInvokeCallbackFunc<T, TResult>(callback));

    /// <summary>
    /// Create a callback for a javascript function with a return value.
    /// <code>
    /// function(dotNetHelper){
    ///    let result = dotNetHelper.invokeMethodAsync("Invoke", arg1, arg2);
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T1">Argument1 type.</typeparam>
    /// <typeparam name="T2">Argument2 type.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="callback">The callback.</param>
    public static DotNetObjectReference<JSInvokeCallbackFunc<T1, T2, TResult>> Create<T1, T2, TResult>(Func<T1, T2, TResult> callback)
    => DotNetObjectReference.Create(new JSInvokeCallbackFunc<T1, T2, TResult>(callback));


    /// <summary>
    /// Create a callback for a javascript function with a return value.
    /// <code>
    /// function(dotNetHelper){
    ///    let result = dotNetHelper.invokeMethodAsync("Invoke", arg1, arg2, arg3);
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T1">Argument1 type.</typeparam>
    /// <typeparam name="T2">Argument2 type.</typeparam>
    /// <typeparam name="T3">Argument3 type.</typeparam>
    /// <typeparam name="TResult">The type of return value.</typeparam>
    /// <param name="callback">The callback.</param>
    public static DotNetObjectReference<JSInvokeCallbackFunc<T1, T2, T3, TResult>> Create<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> callback)
    => DotNetObjectReference.Create(new JSInvokeCallbackFunc<T1, T2, T3, TResult>(callback));
    #endregion
}
