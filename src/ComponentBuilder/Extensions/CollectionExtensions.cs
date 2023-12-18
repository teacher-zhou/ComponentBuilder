namespace ComponentBuilder;
/// <summary>
/// The collection extensions.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Adds or updates the specified key/value pair determined by the same key in the current dictionary operation.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="values">The value to update</param>
    /// <param name="replace">If <paramref name="replace"/> is <c>true</c>, the value of the same key is updated, otherwise, a new value is added for the key.</param>
    /// <param name="allowNullValue">Whether to allow the <c>null</c> value to be added or updated.</param>
    /// <exception cref="ArgumentNullException"><paramref name="values"/> is null。</exception>
    public static void AddOrUpdateRange<TKey, TValue>(this IDictionary<TKey, TValue> source, IEnumerable<KeyValuePair<TKey, TValue>> values, bool replace = true, bool allowNullValue = true)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (values is null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        foreach (var item in values)
        {
            source.AddOrUpdate(item, replace, allowNullValue);
        }
    }

    /// <summary>
    /// Adds or updates the specified key/value pair determined by the same key in the current dictionary operation.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="value">The value to update</param>
    /// <param name="replace">If <paramref name="replace"/> is <c>true</c>, the value of the same key is updated, otherwise, a new value is added for the key.</param>
    /// <param name="allowNullValue">Whether to allow the <c>null</c> value to be added or updated.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null。</exception>
    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, KeyValuePair<TKey, TValue> value, bool replace = true, bool allowNullValue = true)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (source.ContainsKey(value.Key))
        {
            if (replace && (allowNullValue || value.Value is not null))
            {
                source[value.Key] = value.Value;
            }
        }
        else
        {
            if (allowNullValue || value.Value is not null)
            {
                source.Add(value.Key, value.Value);
            }
        }
    }

    /// <summary>
    /// Perform a loop operation from the data source.
    /// </summary>
    /// <typeparam name="T">The data type.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="action">The action for each item in the loop is performed.</param>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is null。</exception>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in source)
        {
            action(item);
        }
    }
}