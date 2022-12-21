using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterope;
/// <summary>
/// A factory to create callback for javascript function.
/// </summary>
public static class CallbackFactory
{
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
    public static DotNetObjectReference<CallbackAction> Create(Action callback)
    => DotNetObjectReference.Create(new CallbackAction(callback));

    /// <summary>
    /// Create a callback argument for javascript function.
    /// <para>
    /// Define a argument and call method <c>invokeMethodAsync("Invoke", args)</c> in js function.
    /// <code>
    /// function(dotNetHelper){
    ///     dotNetHelper.invokeMethodAsync("Invoke", args);
    /// }
    /// </code>
    /// </para>    
    /// </summary>
    /// <param name="callback">The action to invoke.</param>
    /// <returns>The instance of <see cref="DotNetObjectReference{TValue}"/> class.</returns>
    public static DotNetObjectReference<CallbackAction<TEventArgs>> Create<TEventArgs>(Action<TEventArgs> callback) where TEventArgs : class
    => DotNetObjectReference.Create(new CallbackAction<TEventArgs>(callback));


    /// <summary>
    /// Create a callback argument for javascript function.
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
    public static DotNetObjectReference<CallbackFunc<TResult>> Create<TResult>(Func<TResult> callback)
    => DotNetObjectReference.Create(new CallbackFunc<TResult>(callback));

    /// <summary>
    /// Create a callback argument for javascript function.
    /// <para>
    /// Define a argument and call method <c>invokeMethodAsync("Invoke", args)</c> in js function.
    /// <code>
    /// function(dotNetHelper){
    ///    let result = dotNetHelper.invokeMethodAsync("Invoke", args);
    /// }
    /// </code>
    /// </para>    
    /// </summary>
    /// <param name="callback">The action to invoke.</param>
    /// <returns>The instance of <see cref="DotNetObjectReference{TValue}"/> class.</returns>
    public static DotNetObjectReference<CallbackFunc<TEventArgs, TResult>> Create<TEventArgs, TResult>(Func<TEventArgs, TResult> callback) where TEventArgs : class
    => DotNetObjectReference.Create(new CallbackFunc<TEventArgs, TResult>(callback));
}
