namespace ComponentBuilder.Enhancement;
/// <summary>
/// The extensions of collection.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Add or update specified key/value pairs in current ditionary witch action determined by same key. 
    /// <para>
    /// If <paramref name="replace"/> is <c>true</c> to update the value for same key, otherwise, add new value for this key.
    /// </para>
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <param name="source">The source of dictionary.</param>
    /// <param name="values">The values to update.</param>
    /// <param name="replace"><c>True</c> replace with same key, otherwise <c>false</c>.</param>
    /// <param name="allowNullValue"><c>True</c> to add or update if value is <c>null</c> for this key.</param>
    /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
    public static void AddOrUpdateRange<TKey, TValue>(this IDictionary<TKey, TValue> source, IEnumerable<KeyValuePair<TKey, TValue>> values, bool replace = true, bool allowNullValue = true)
    {
        if ( source is null )
        {
            throw new ArgumentNullException(nameof(source));
        }

        if ( values is null )
        {
            throw new ArgumentNullException(nameof(values));
        }

        foreach ( var item in values )
        {
            source.AddOrUpdate(item, replace, allowNullValue);
        }
    }

    /// <summary>
    /// Add or update specified key/value pairs in current ditionary witch action determined by same key. 
    /// <para>
    /// If <paramref name="replace"/> is <c>true</c> to update the value for same key, otherwise, add new value for this key.
    /// </para>
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <param name="source">The source of dictionary.</param>
    /// <param name="value">The value to update.</param>
    /// <param name="replace"><c>True</c> replace with same key, otherwise <c>false</c>.</param>
    /// <param name="allowNullValue"><c>True</c> to add or update if value is <c>null</c> for this key.</param>
    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, KeyValuePair<TKey, TValue> value, bool replace = true, bool allowNullValue = true)
    {
        if ( source is null )
        {
            throw new ArgumentNullException(nameof(source));
        }

        if ( source.ContainsKey(value.Key) )
        {
            if ( replace && (allowNullValue || value.Value is not null) )
            {
                source[value.Key] = value.Value;
            }
        }
        else
        {
            if ( allowNullValue || value.Value is not null )
            {
                source.Add(value.Key, value.Value);
            }
        }
    }

    /// <summary>
    /// Execute loop action from data source.
    /// </summary>
    /// <typeparam name="T">The data type.</typeparam>
    /// <param name="source">Data source to loop.</param>
    /// <param name="action">An action for each item in loop execution.</param>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        if ( source is null )
        {
            throw new ArgumentNullException(nameof(source));
        }

        if ( action is null )
        {
            throw new ArgumentNullException(nameof(action));
        }

        foreach ( var item in source )
        {
            action(item);
        }
    }
}