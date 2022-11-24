namespace ComponentBuilder;
/// <summary>
/// The extensions of <see cref="EventCallbackFactory"/> class.
/// </summary>
public static class EventCallbackFactoryExtensions
{
    /// <summary>
    /// Creates <see cref="EventCallback"/> for the provided receiver and callback when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/> instance.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <param name="condition">The condition for satisifiction.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    public static EventCallback Create(this EventCallbackFactory factory, object receiver, Action callback, bool condition)
    {
        if (!condition)
        {
            return EventCallback.Empty;
        }
        return factory.Create(receiver, callback);
    }

    /// <summary>
    /// Creates <see cref="EventCallback"/> for the provided receiver and callback when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/> instance.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <param name="condition">The condition for satisifiction.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    public static EventCallback<TValue> Create<TValue>(this EventCallbackFactory factory, object receiver, Action<TValue> callback, bool condition)
    {
        if (!condition)
        {
            return EventCallback<TValue>.Empty;
        }
        return factory.Create(receiver, callback);
    }
    /// <summary>
    /// Creates <see cref="EventCallback"/> for the provided receiver and callback when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/> instance.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <param name="condition">The condition for satisifiction.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    public static EventCallback<TValue> Create<TValue>(this EventCallbackFactory factory, object receiver, Action callback, bool condition)
    {
        if (!condition)
        {
            return EventCallback<TValue>.Empty;
        }
        return factory.Create<TValue>(receiver, callback);
    }
    /// <summary>
    /// Creates <see cref="EventCallback"/> for the provided receiver and callback when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/> instance.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <param name="condition">The condition for satisifiction.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    public static EventCallback Create(this EventCallbackFactory factory, object receiver, Func<Task> callback, bool condition)
    {
        if (!condition)
        {
            return EventCallback.Empty;
        }
        return factory.Create(receiver, callback);
    }
    /// <summary>
    /// Creates <see cref="EventCallback"/> for the provided receiver and callback when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="factory">The <see cref="EventCallbackFactory"/> instance.</param>
    /// <param name="receiver">The event receiver.</param>
    /// <param name="callback">The event callback.</param>
    /// <param name="condition">The condition for satisifiction.</param>
    /// <returns>The <see cref="EventCallback"/>.</returns>
    public static EventCallback<TValue> Create<TValue>(this EventCallbackFactory factory, object receiver, Func<TValue, Task> callback, bool condition)
    {
        if (!condition)
        {
            return EventCallback<TValue>.Empty;
        }
        return factory.Create(receiver, callback);
    }
}
