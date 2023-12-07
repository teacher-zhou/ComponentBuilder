namespace ComponentBuilder;
/// <summary>
/// The extensions to parse value.
/// </summary>
public static class ParseExtensions
{
    /// <summary>
    /// Parse any value to specified value type.
    /// </summary>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <param name="value">The value to parse.</param>
    /// <param name="defaultValue">Return default value if parse failed.</param>
    /// <param name="throwIfFailed"><c>true</c> to throw exception when parsing failed.</param>
    /// <returns></returns>
    public static TValue? Parse<TValue>(this object? value, TValue? defaultValue = default, bool throwIfFailed=default)
    {
        if (value is null)
        {
            return defaultValue;
        }

        var valueType = typeof(TValue);

        try
        {
            var nullableValueType = Nullable.GetUnderlyingType(valueType);
            if (nullableValueType is null)
            {
                return (TValue?)Convert.ChangeType(value, valueType);
            }

            return (TValue?)Convert.ChangeType(value, nullableValueType);
        }
        catch
        {
            if (throwIfFailed)
            {
                throw;
            }
            return defaultValue;
        }
    }
}
