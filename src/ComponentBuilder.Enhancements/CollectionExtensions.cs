namespace ComponentBuilder;
/// <summary>
/// 集合的扩展。
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// 在当前字典操作中添加或更新由相同键确定的指定键/值对。
    /// <para>
    /// 如果 <paramref name="replace"/> 为 <c>true</c>，则更新同一键的值，否则，为该键添加新值。
    /// </para>
    /// </summary>
    /// <typeparam name="TKey">键的类型。</typeparam>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="source">字典对象的源。</param>
    /// <param name="values">要更新的值。</param>
    /// <param name="replace">如果 <paramref name="replace"/> 为 <c>true</c>，则更新同一键的值，否则，为该键添加新值。</param>
    /// <param name="allowNullValue">是否允许 <c>null</c> 值进行添加或更新操作。</param>
    /// <exception cref="ArgumentNullException"><paramref name="values"/> 是 null。</exception>
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
    /// 在当前字典操作中添加或更新由相同键确定的指定键/值对。
    /// <para>
    /// 如果 <paramref name="replace"/> 为 <c>true</c>，则更新同一键的值，否则，为该键添加新值。
    /// </para>
    /// </summary>
    /// <typeparam name="TKey">键的类型。</typeparam>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="source">字典对象的源。</param>
    /// <param name="value">要更新的值。</param>
    /// <param name="replace">如果 <paramref name="replace"/> 为 <c>true</c>，则更新同一键的值，否则，为该键添加新值。</param>
    /// <param name="allowNullValue">是否允许 <c>null</c> 值进行添加或更新操作。</param>
    /// <exception cref="ArgumentNullException"><paramref name="values"/> 是 null。</exception>
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
    /// 从数据源执行循环操作。
    /// </summary>
    /// <typeparam name="T">数据类型。</typeparam>
    /// <param name="source">要循环的数据源。</param>
    /// <param name="action">循环执行中每个项的动作。</param>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> 是 null。</exception>
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