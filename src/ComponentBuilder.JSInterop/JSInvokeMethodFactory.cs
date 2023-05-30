using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;
/// <summary>
/// 一个为 javascript 函数创建回调的工厂。
/// </summary>
public static class JSInvokeMethodFactory
{
    #region CallbackAction
    /// <summary>
    /// 为 javascript 函数创建一个回调。
    /// <code>
    /// function(dotNetHelper){
    ///     dotNetHelper.invokeMethodAsync("Invoke");
    /// }
    /// </code>
    /// </summary>
    /// <param name="callback">要执行的回调。</param>
    public static DotNetObjectReference<JSInvokeMethodAction> Create(Action callback)
    => DotNetObjectReference.Create(new JSInvokeMethodAction(callback));

    /// <summary>
    /// 为 javascript 函数创建一个具有1个参数的回调。
    /// <code>
    /// function(dotNetHelper){
    ///     dotNetHelper.invokeMethodAsync("Invoke", arg);
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T">参数类型。</typeparam>
    /// <param name="callback">要执行的回调。</param>
    public static DotNetObjectReference<JSInvokeMethodAction<T>> Create<T>(Action<T> callback)
    => DotNetObjectReference.Create(new JSInvokeMethodAction<T>(callback));

    /// <summary>
    /// 为 javascript 函数创建一个具有2个参数的回调。
    /// <code>
    /// function(dotNetHelper){
    ///     dotNetHelper.invokeMethodAsync("Invoke", arg1, arg2);
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T1">参数1的类型。</typeparam>
    /// <typeparam name="T2">参数2的类型。</typeparam>
    /// <param name="callback">要执行的回调。</param>
    public static DotNetObjectReference<JSInvokeMethodAction<T1, T2>> Create<T1, T2>(Action<T1, T2> callback)
    => DotNetObjectReference.Create(new JSInvokeMethodAction<T1, T2>(callback));

    /// <summary>
    /// 为 javascript 函数创建一个具有3个参数的回调。
    /// <code>
    /// function(dotNetHelper){
    ///     dotNetHelper.invokeMethodAsync("Invoke", arg1, arg2, arg3);
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T1">参数1的类型。</typeparam>
    /// <typeparam name="T2">参数2的类型。</typeparam>
    /// <typeparam name="T3">参数3的类型。</typeparam>
    /// <param name="callback">要执行的回调。</param>
    public static DotNetObjectReference<JSInvokeMethodAction<T1, T2, T3>> Create<T1, T2, T3>(Action<T1, T2, T3> callback)
    => DotNetObjectReference.Create(new JSInvokeMethodAction<T1, T2, T3>(callback));
    #endregion

    #region CallbackFunc
    /// <summary>
    /// 为 javascript 函数创建一个具有返回值的回调。
    /// <code>
    /// function(dotNetHelper){
    ///    let result = dotNetHelper.invokeMethodAsync("Invoke");
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="TResult">返回值类型。</typeparam>
    /// <param name="callback">要执行的回调。</param>
    public static DotNetObjectReference<JSInvokeMethodFunc<TResult>> Create<TResult>(Func<TResult> callback)
    => DotNetObjectReference.Create(new JSInvokeMethodFunc<TResult>(callback));

    /// <summary>
    /// 为 javascript 函数创建一个具有返回值并且有1个参数的回调。
    /// <code>
    /// function(dotNetHelper){
    ///    let result = dotNetHelper.invokeMethodAsync("Invoke", arg);
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T">参数类型。</typeparam>
    /// <typeparam name="TResult">返回值类型。</typeparam>
    /// <param name="callback">要执行的回调。</param>
    public static DotNetObjectReference<JSInvokeMethodFunc<T, TResult>> Create<T, TResult>(Func<T, TResult> callback)
    => DotNetObjectReference.Create(new JSInvokeMethodFunc<T, TResult>(callback));

    /// <summary>
    /// 为 javascript 函数创建一个具有返回值并且有2个参数的回调。
    /// <code>
    /// function(dotNetHelper){
    ///    let result = dotNetHelper.invokeMethodAsync("Invoke", arg1, arg2);
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T1">参数1的类型。</typeparam>
    /// <typeparam name="T2">参数2的类型。</typeparam>
    /// <typeparam name="TResult">返回值类型。</typeparam>
    /// <param name="callback">要执行的回调。</param>
    public static DotNetObjectReference<JSInvokeMethodFunc<T1, T2, TResult>> Create<T1, T2, TResult>(Func<T1, T2, TResult> callback)
    => DotNetObjectReference.Create(new JSInvokeMethodFunc<T1, T2, TResult>(callback));


    /// <summary>
    /// 为 javascript 函数创建一个具有返回值并且有3个参数的回调。
    /// <code>
    /// function(dotNetHelper){
    ///    let result = dotNetHelper.invokeMethodAsync("Invoke", arg1, arg2, arg3);
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="T1">参数1的类型。</typeparam>
    /// <typeparam name="T2">参数2的类型。</typeparam>
    /// <typeparam name="T3">参数3的类型。</typeparam>
    /// <typeparam name="TResult">返回值类型。</typeparam>
    /// <param name="callback">要执行的回调。</param>
    public static DotNetObjectReference<JSInvokeMethodFunc<T1, T2, T3, TResult>> Create<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> callback)
    => DotNetObjectReference.Create(new JSInvokeMethodFunc<T1, T2, T3, TResult>(callback));
    #endregion
}
