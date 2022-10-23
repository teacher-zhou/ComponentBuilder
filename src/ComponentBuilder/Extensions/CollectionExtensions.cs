using System.Reflection;

namespace ComponentBuilder;
public static class CollectionExtensions
{
    /// <summary>
    /// 合并指定的键值对集合。
    /// </summary>
    /// <typeparam name="TKey">键的类型。</typeparam>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="source">源数据。</param>
    /// <param name="values">要合并的键值对集合。</param>
    /// <param name="replace">若为 <c>true</c> 则相同键会使用 <paramref name="values"/> 集合的值进行覆盖, 否则忽略。</param>
    /// <returns>合并后的新键值对集合。</returns>
    /// <exception cref="System.ArgumentNullException">
    /// <paramref name="source"/>
    /// 或
    /// <paramref name="values"/> 是 <c>null</c>。
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
    /// 从 <see cref="PropertyInfo"/> 集合中获取 <see cref="HtmlEventAttribute"/> 特性。
    /// </summary>
    /// <param name="properties"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    internal static IEnumerable<KeyValuePair<string, object>> GetEventNameValue(this IEnumerable<PropertyInfo> properties, object instance)
    {
        return properties.Where(m => m.IsDefined(typeof(HtmlEventAttribute), false)).Select(m => new KeyValuePair<string, object>(m.GetCustomAttribute<HtmlEventAttribute>().Name, m.GetValue(instance)));
    }
}