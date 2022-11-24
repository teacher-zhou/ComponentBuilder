using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ComponentBuilder.JSInterope;
/// <summary>
/// Represents a javascript builder using C# code.
/// </summary>
public sealed class ScriptBuilder : DynamicObject, IDisposable
{
    private readonly StringBuilder _script;


    /// <summary>
    /// Initializes a new instance of the <see cref="ScriptBuilder"/> class.
    /// </summary>
    public ScriptBuilder() => _script = new StringBuilder();


    /// <summary>
    /// 赋值，xx = 10
    /// </summary>
    /// <param name="binder"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        _script.AppendFormat($"{binder.Name} = {GetValue(value)}");

        return true;
    }

    /// <summary>
    /// 获取值， var x = window.abc;
    /// </summary>
    /// <param name="binder"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        Append(binder.Name);
        result = this;
        return true;
    }

    /// <summary>
    /// 调用函数，xx.add(a,b,c...)
    /// </summary>
    /// <param name="binder"></param>
    /// <param name="args"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        if (_script.Length > 0)
        {
            _script.Append('.');
        }
        var getArgs = args?.Select(value => GetValue(value));
        _script.Append($"{binder.Name}({string.Join(",", getArgs)})");
        result = this;
        return true;
    }
    /// <summary>
    /// 获取索引。obj.index[1]
    /// </summary>
    /// <param name="binder"></param>
    /// <param name="indexes"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object? result)
    {

        var isString = indexes[0] is string;
        if (indexes[0] is ScriptBuilder || indexes[0] is int || isString)
        {
            _script.Append('[');
            var value = isString ? GetValue(indexes[0]) : indexes[0].ToString();
            _script.Append(value);
            _script.Append(']');
        }
        else
        {
            throw new Exception("Unexpected indexer type");
        }
        result = this;
        return true;
    }

    /// <inheritdoc/>
    public override bool TryCreateInstance(CreateInstanceBinder binder, object?[]? args, [NotNullWhen(true)] out object? result)
    {
        Append(binder.CallInfo.ArgumentNames.Single());
        result = this;
        return true;
    }

    /// <summary>
    /// Append specified value.
    /// </summary>
    /// <param name="value">The value to append.</param>
    private void Append(object value)
    {
        if (_script.Length > 0)
        {
            _script.Append('.');
        }
        _script.Append(value);
    }

    /// <summary>
    /// Analyze specified value to string.
    /// </summary>
    /// <param name="value">The value to analyze.</param>
    /// <returns>javascript string or null.</returns>
    string? GetValue(object? value)
        => value switch
        {
            null => "null",
            string => string.Format("{0}{1}{0}", Regex.IsMatch(value!.ToString(), "[\r\n]") ? '`' : '\"', value!.ToString().Replace("'", "\u0027").Replace("\"", "\\u0022")),
            DateTime => "new Date(\u0022" + ((DateTime)value).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz") + "\u0022)",
            _ => new Func<string>(() =>
            {
                if (IsAnonymousType(value.GetType()))
                {
                    return JsonSerializer.Serialize(value);
                }
                return value?.ToString();
            }).Invoke()
        };

    /// <summary>
    /// Determines specified type is anonymouse object.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns><c>true</c> for anonymouse object, otherwise <c>false</c>.</returns>
    private bool IsAnonymousType(Type type) => type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length > 0 && type.FullName.Contains("AnonymousType");

    /// <inheritdoc/>
    public override string ToString() => _script.ToString();

    public void Dispose()
    {
        _script.Clear();
    }
}