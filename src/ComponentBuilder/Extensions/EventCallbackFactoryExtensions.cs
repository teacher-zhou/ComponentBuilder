namespace ComponentBuilder;
/// <summary>
/// The extensions of <see cref="EventCallbackFactory"/> class.
/// </summary>
public static class EventCallbackFactoryExtensions
{
    static EventCallback Execute(Condition? condition, Func<EventCallback> action)
        => Condition.Execute(condition, action, () => EventCallback.Empty);

    static EventCallback<TValue> Execute<TValue>(Condition? condition, Func<EventCallback<TValue>> action)
        => Condition.Execute(condition, action, () => EventCallback<TValue>.Empty);

    /// <summary>
    /// Creates <see cref="EventCallback"/> for the provided receiver and callback when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/> instance.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <param name="condition">The condition for satisifiction. <c>True</c> for <c>null</c> value.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    public static EventCallback Create(this EventCallbackFactory factory, object receiver, Action callback, Condition? condition = default) 
        => Execute(condition, () => factory.Create(receiver, callback));

    /// <summary>
    /// Creates <see cref="EventCallback"/> for the provided receiver and callback when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/> instance.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <param name="condition">The condition for satisifiction. <c>True</c> for <c>null</c> value.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    public static EventCallback<TValue> Create<TValue>(this EventCallbackFactory factory, object receiver, Action<TValue> callback, Condition? condition = default)
        => Execute(condition, () => factory.Create(receiver, callback));
    /// <summary>
    /// Creates <see cref="EventCallback"/> for the provided receiver and callback when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/> instance.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <param name="condition">The condition for satisifiction. <c>True</c> for <c>null</c> value.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    public static EventCallback<TValue> Create<TValue>(this EventCallbackFactory factory, object receiver, Action callback, Condition? condition = default)
        => Execute(condition, () => factory.Create<TValue>(receiver, callback));

    /// <summary>
    /// Creates <see cref="EventCallback"/> for the provided receiver and callback when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/> instance.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <param name="condition">The condition for satisifiction. <c>True</c> for <c>null</c> value.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    public static EventCallback Create(this EventCallbackFactory factory, object receiver, Func<Task> callback, Condition? condition = default)
        => Execute(condition, () => factory.Create(receiver, callback));
    /// <summary>
    /// Creates <see cref="EventCallback"/> for the provided receiver and callback when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/> instance.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <param name="condition">The condition for satisifiction. <c>True</c> for <c>null</c> value.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    public static EventCallback<TValue> Create<TValue>(this EventCallbackFactory factory, object receiver, Func<TValue, Task> callback, Condition? condition = default)
        => Execute(condition, () => factory.Create(receiver, callback));
}
