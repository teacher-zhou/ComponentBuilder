using System.Reflection;

namespace ComponentBuilder;
/// <summary>
/// The extensions of collection.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Merge current key/value pairs with specified key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <param name="source">The source to merge.</param>
    /// <param name="values">The values to be merged.</param>
    /// <param name="replace"><c>true</c> replace with same key, otherwise <c>false</c>.</param>
    /// <returns>A new key/value pairs merged by two collections.</returns>
    /// <exception cref="System.ArgumentNullException">
    /// <paramref name="source"/>
    /// or
    /// <paramref name="values"/> is <c>null</c>。
    /// </exception>
    public static IEnumerable<KeyValuePair<TKey, TValue>> Merge<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, IEnumerable<KeyValuePair<TKey, TValue>> values, bool replace = true)
        where TKey : notnull
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (values is null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        var dic = new Dictionary<TKey, TValue>(source);
        foreach (var item in values)
        {
            if (dic.ContainsKey(item.Key))
            {
                if (replace)
                {
                    dic[item.Key] = item.Value;
                }
            }
            else
            {
                dic.Add(item.Key, item.Value);
            }
        }
        return dic;
    }

    /// <summary>
    /// Returns the key/values pairs from specified instance of <see cref="PropertyInfo"/> that defined <see cref="HtmlEventAttribute"/> attributes.
    /// </summary>
    internal static IEnumerable<KeyValuePair<string, object>> GetEventNameValue(this IEnumerable<PropertyInfo> properties, object instance)
    {
        return properties.Where(m => m.IsDefined(typeof(HtmlEventAttribute), false)).Select(m => new KeyValuePair<string, object>(m.GetCustomAttribute<HtmlEventAttribute>().Name, m.GetValue(instance)));
    }
}