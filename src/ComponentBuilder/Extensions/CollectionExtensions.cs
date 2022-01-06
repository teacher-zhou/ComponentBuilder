using System.Linq;
using System.Reflection;

namespace ComponentBuilder;

public static class CollectionExtensions
{
    /// <summary>
    /// Merge the specified values to current source.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="second">The collection to merge.</param>
    /// <param name="replace"><c>true</c> to replace value with same key, otherwise ignore.</param>
    /// <returns>The merged collection from source.</returns>
    /// <exception cref="System.ArgumentNullException">
    /// source
    /// or
    /// values
    /// </exception>
    public static IEnumerable<KeyValuePair<TKey, TValue>> Merge<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, IEnumerable<KeyValuePair<TKey, TValue>> second, bool replace = true)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (second is null)
        {
            throw new ArgumentNullException(nameof(second));
        }

        var dic = new Dictionary<TKey, TValue>(source);
        foreach (var item in second)
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




    internal static IEnumerable<KeyValuePair<string, object>> GetEventNameValue(this IEnumerable<PropertyInfo> properties,object instance)
    {
        return properties.Where(m => m.IsDefined(typeof(HtmlEventAttribute), false)).Select(m => new KeyValuePair<string, object>(m.GetCustomAttribute<HtmlEventAttribute>().Name, m.GetValue(instance)));
    }
}