﻿using System.Reflection;

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
    /// <param name="replace"><c>True</c> replace with same key, otherwise <c>false</c>.</param>
    /// <param name="allowNullValue"><c>True</c> to add or update if value is <c>null</c> for this key.</param>
    /// <returns>A new key/value pairs merged by two collections.</returns>
    /// <exception cref="System.ArgumentNullException">
    /// <paramref name="source"/>
    /// or
    /// <paramref name="values"/> is <c>null</c>。
    /// </exception>
    public static IEnumerable<KeyValuePair<TKey, TValue?>> Merge<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue?>> source, IEnumerable<KeyValuePair<TKey, TValue?>> values, bool replace = true, bool allowNullValue = true)
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

        var dic = new Dictionary<TKey, TValue?>(source);
        foreach (var item in values)
        {
            dic.AddOrUpdate(item, replace, allowNullValue);
        }
        return dic;
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
    /// <param name="values">The values to update.</param>
    /// <param name="replace"><c>True</c> replace with same key, otherwise <c>false</c>.</param>
    /// <param name="allowNullValue"><c>True</c> to add or update if value is <c>null</c> for this key.</param>
    /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
    public static void AddOrUpdateRange<TKey, TValue>(this IDictionary<TKey, TValue?> source, IEnumerable<KeyValuePair<TKey, TValue?>> values, bool replace = true, bool allowNullValue = true)
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
    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue?> source, KeyValuePair<TKey, TValue?> value, bool replace = true, bool allowNullValue = true)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (source.ContainsKey(value.Key))
        {
            if ( replace && (allowNullValue || value.Value is not null ))
            {
                source[value.Key] = value.Value;
            }
        }
        else
        {
            source.Add(value.Key, value.Value);
        }
    }

    public static bool TryAddOrConcat(this IDictionary<string, object?> source, string key, object? value, bool appendOrPrepend = true)
    {
        var exist = source.TryGetValue(key, out var existValue);

        if (exist)
        {
            source[key] = appendOrPrepend ? $"{existValue}{value}" : $"{value}{existValue}";
        }
        else
        {
            source[key] = value;
        }
        return exist;
    }
            if ( allowNullValue || value.Value is not null )
            {
                source.Add(value.Key, value.Value);
            }
        }
    }

    /// <summary>
    /// Returns the key/values pairs from specified instance of <see cref="PropertyInfo"/> that defined <see cref="HtmlEventAttribute"/> attributes.
    /// </summary>
    internal static IEnumerable<KeyValuePair<string, object?>> GetEventNameValue(this IEnumerable<PropertyInfo> properties, object instance)
    {
        return properties.Where(m => m.IsDefined(typeof(HtmlEventAttribute), false)).Select(m => new KeyValuePair<string, object?>(m.GetCustomAttribute<HtmlEventAttribute>()!.Name, m.GetValue(instance)));
    }
}