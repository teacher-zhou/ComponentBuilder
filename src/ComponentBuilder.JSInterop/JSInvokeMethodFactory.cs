using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;
/// <summary>
/// A factory to create callback for javascript function.
/// </summary>
public static class JSInvokeMethodFactory
{
    #region CallbackAction
    /// <summary>
    /// Create a callback argument for javascript function.
    /// <para>
    /// Define a argument and call method <c>invokeMethodAsync("Invoke")</c> in js function.
    /// <code>
    /// function(dotNetHelper){
    ///     dotNetHelper.invokeMethodAsync("Invoke");
    /// }
    /// </code>
    /// </para>    
    /// </summary>
    /// <param name="callback">The action to invoke.</param>
    /// <returns>The instance of <see cref="DotNetObjectReference{TValue}"/> class.</returns>
    public static DotNetObjectReference<JSInvokeMethodAction> Create(Action callback)
    => DotNetObjectReference.Create(new JSInvokeMethodAction(callback));

    /// <summary>
    /// Create a callback argument for javascript function.
    /// <para>
    /// Define a argument and call method <c>invokeMethodAsync("Invoke", arg)</c> in js function.
    /// <code>
    /// function(dotNetHelper){
    ///     dotNetHelper.invokeMethodAsync("Invoke", arg);
    /// }
    /// </code>
    /// </para>    
    /// </summary>
    /// <param name="callback">The action to invoke.</param>
    /// <returns>The instance of <see cref="DotNetObjectReference{TValue}"/> class.</returns>
    public static DotNetObjectReference<CallbackAction<T>> Create<T>(Action<T> callback)
    => DotNetObjectReference.Create(new CallbackAction<T>(callback));

    /// <summary>
    /// Create a callback argument for javascript function.
    /// <para>
    /// Define a argument and call method <c>invokeMethodAsync("Invoke", arg1, arg2)</c> in js function.
    /// <code>
    /// function(dotNetHelper){
    ///     dotNetHelper.invokeMethodAsync("Invoke", arg1, arg2);
    /// }
    /// </code>
    /// </para>    
    /// </summary>
    /// <param name="callback">The action to invoke.</param>
    /// <returns>The instance of <see cref="DotNetObjectReference{TValue}"/> class.</returns>
    public static DotNetObjectReference<CallbackAction<T1, T2>> Create<T1, T2>(Action<T1, T2> callback)
    => DotNetObjectReference.Create(new CallbackAction<T1, T2>(callback));

    /// <summary>
    /// Create a callback argument for javascript function.
    /// <para>
    /// Define a argument and call method <c>invokeMethodAsync("Invoke", arg1, arg2, arg3)</c> in js function.
    /// <code>
    /// function(dotNetHelper){
    ///     dotNetHelper.invokeMethodAsync("Invoke", arg1, arg2, arg3);
    /// }
    /// </code>
    /// </para>    
    /// </summary>
    /// <param name="callback">The action to invoke.</param>
    /// <returns>The instance of <see cref="DotNetObjectReference{TValue}"/> class.</returns>
    public static DotNetObjectReference<CallbackAction<T1, T2, T3>> Create<T1, T2, T3>(Action<T1, T2, T3> callback)
    => DotNetObjectReference.Create(new CallbackAction<T1, T2, T3>(callback));
    #endregion

    #region CallbackFunc
    /// <summary>
    /// Create a callback argument for javascript function and return a result.
    /// <para>
    /// Define a argument and call method <c>invokeMethodAsync("Invoke")</c> in js function.
    /// <code>
    /// function(dotNetHelper){
    ///    let result = dotNetHelper.invokeMethodAsync("Invoke");
    /// }
    /// </code>
    /// </para>    
    /// </summary>
    /// <param name="callback">The action to invoke.</param>
    /// <returns>The instance of <see cref="DotNetObjectReference{TValue}"/> class.</returns>
    public static DotNetObjectReference<JSInvokeMethodFunc<TResult>> Create<TResult>(Func<TResult> callback)
    => DotNetObjectReference.Create(new JSInvokeMethodFunc<TResult>(callback));

    /// <summary>
    /// Create a callback argument for javascript function and return a result.
    /// <para>
    /// Define a argument and call method <c>invokeMethodAsync("Invoke", arg)</c> in js function.
    /// <code>
    /// function(dotNetHelper){
    ///    return dotNetHelper.invokeMethodAsync("Invoke", arg);
    /// }
    /// </code>
    /// </para>    
    /// </summary>
    /// <param name="callback">The action to invoke.</param>
    /// <returns>The instance of <see cref="DotNetObjectReference{TValue}"/> class.</returns>
    public static DotNetObjectReference<CallbackFunc<T1, TResult>> Create<T1, TResult>(Func<T1, TResult> callback)
    => DotNetObjectReference.Create(new CallbackFunc<T1, TResult>(callback));

    /// <summary>
    /// Create a callback argument for javascript function and return a result.
    /// <para>
    /// Define a argument and call method <c>invokeMethodAsync("Invoke", arg1, arg2)</c> in js function.
    /// <code>
    /// function(dotNetHelper){
    ///    return dotNetHelper.invokeMethodAsync("Invoke", arg1, arg3);
    /// }
    /// </code>
    /// </para>    
    /// </summary>
    /// <param name="callback">The action to invoke.</param>
    /// <returns>The instance of <see cref="DotNetObjectReference{TValue}"/> class.</returns>
    public static DotNetObjectReference<CallbackFunc<T1, T2, TResult>> Create<T1, T2, TResult>(Func<T1, T2, TResult> callback)
    => DotNetObjectReference.Create(new CallbackFunc<T1, T2, TResult>(callback));

    /// <summary>
    /// Create a callback argument for javascript function and return a result.
    /// <para>
    /// Define a argument and call method <c>invokeMethodAsync("Invoke", arg1, arg2, arg3)</c> in js function.
    /// <code>
    /// function(dotNetHelper){
    ///    return dotNetHelper.invokeMethodAsync("Invoke", arg1, arg3);
    /// }
    /// </code>
    /// </para>    
    /// </summary>
    /// <param name="callback">The action to invoke.</param>
    /// <returns>The instance of <see cref="DotNetObjectReference{TValue}"/> class.</returns>
    public static DotNetObjectReference<CallbackFunc<T1, T2, T3, TResult>> Create<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> callback)
    => DotNetObjectReference.Create(new CallbackFunc<T1, T2, T3, TResult>(callback));
    #endregion
}
