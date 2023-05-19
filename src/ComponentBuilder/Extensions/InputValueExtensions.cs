using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ComponentBuilder;

/// <summary>
/// 输入交互的扩展。
/// </summary>
public static class InputValueExtensions
{
    /// <summary>
    /// 当值被给定值改变时执行。
    /// </summary>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="instance">实现了 <see cref="IHasValueBound{TValue}"/> 接口的实例。</param>
    /// <param name="value">要更改的新值。</param>
    /// <param name="afterChanged">当输入参数的值改变时的回调函数。</param>
    /// <returns>如果值被改变，则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
    public static bool OnValueChanged<TValue>(this IHasValueBound<TValue?> instance, TValue? value, Func<bool, Task?>? afterChanged = default)
    {
        var hasChanged = !EqualityComparer<TValue?>.Default.Equals(value, instance.Value);
        if ( hasChanged )
        {
            instance.Value = value;
            instance.ValueChanged.InvokeAsync(value);
        }
        afterChanged?.Invoke(hasChanged);
        return hasChanged;
    }

    /// <summary>
    /// 从给定的绑定值获取 <see cref="FieldIdentifier"/>。
    /// </summary>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="instance">实现了 <see cref="IHasInputValue{TValue}"/> 接口的实例。</param>
    /// <returns><see cref="FieldIdentifier"/> 或 null.</returns>
    public static FieldIdentifier? GetFieldIdentifier<TValue>(this IHasInputValue<TValue?> instance)
    {
        if ( instance is null )
        {
            throw new ArgumentNullException(nameof(instance));
        }

        var expression = instance.ValueExpression;
        if ( expression == null )
        {
            return default;
        }
        return FieldIdentifier.Create(expression);
    }
    /// <summary>
    /// 格式化并以字符串形式返回值。
    /// </summary>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="instance">实现了 <see cref="IHasValueBound{TValue}"/> 接口的实例。</param>
    /// <param name="formatHandler">要格式化的处理程序。</param>
    /// <returns>值的字符串表示形式。</returns>
    public static string? GetValueAsString<TValue>(this IHasValueBound<TValue?> instance, Func<TValue?, string?>? formatHandler = default)
    {
        formatHandler ??= (value) => value?.ToString();
        return formatHandler.Invoke(instance.Value);
    }

    /// <summary>
    /// 用于 <typeparamref name="TValue"/> 值的类型解析的操作。
    /// <para>
    /// 通常，通过与用户(如 <c>&lt;InputText /></c> 组件)交互来调用输入组件的此方法。
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="instance">实现了 <see cref="IHasInputValue{TValue}"/> 接口的实例。</param>
    /// <param name="value">要解析的字符串值。</param>
    /// <param name="validationErrorMessage">如果无法解析该值，则会提供验证错误消息。</param>
    /// <param name="parsingHandler">解析字符串到 <typeparamref name="TValue"/> 类型的处理程序。 </param>
    /// <param name="validationErrorHandler">返回错误信息的处理程序。</param>
    /// <returns>如果值被成功解析，则返回 <c>true</c>，否则返回 <c>false</c> 。</returns>
    /// <exception cref="NotSupportedException">当前实例不支持 <typeparamref name="TValue"/> 的类型。</exception>
    public static bool TryParseValueFromString<TValue>([NotNull] this IHasInputValue<TValue?> instance,
                                                       string? value,
                                                       out string? validationErrorMessage,
                                                       Func<string?>? validationErrorHandler = default,
                                                       Func<(bool isParsed, TValue? parsedValue)>? parsingHandler = default)
    {
        parsingHandler ??= () =>
        {
            var isParsed = BindConverter.TryConvertTo<TValue?>(value, CultureInfo.CurrentCulture, out var parsedValue);
            return (isParsed, parsedValue);
        };

        validationErrorHandler ??= () => $"The {instance.GetFieldIdentifier()?.FieldName} field is not valid.";

        try
        {
            var (isParsed, parsedValue) = parsingHandler();
            if ( isParsed )
            {
                instance!.ChangeValue(parsedValue);
                validationErrorMessage = null;
                return true;
            }
            else
            {
                instance!.ChangeValue(default);
                validationErrorMessage = validationErrorHandler();
                return false;
            }
        }
        catch ( Exception ex )
        {
            throw new NotSupportedException($"{instance.GetType()} does not support the type '{typeof(TValue?)}'.", ex);
        }
    }

    /// <summary>
    /// 变更当前输入组件的值为指定的 <paramref name="value"/> 的值并且 <see cref="OnValueChanged{TValue}(IHasValueBound{TValue}, TValue, Func{bool, Task?}?)"/> 方法会随后执行。
    /// <para>
    /// 当值被改变后，<see cref="EditContext.NotifyFieldChanged(in FieldIdentifier)"/> 方法会被执行，并且事件 <see cref="EditContext.OnFieldChanged"/> 会被触发。
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="instance">实现了 <see cref="IHasInputValue{TValue}"/> 接口的实例。</param>
    /// <param name="value">输入一个新值。</param>
    public static void ChangeValue<TValue>(this IHasInputValue<TValue?> instance, TValue? value)
    {
        instance.OnValueChanged(value, changed =>
        {
            if ( changed )
            {
                var field = instance.GetFieldIdentifier();
                if ( field != null )
                {
                    instance.CascadedEditContext?.NotifyFieldChanged(field.Value);
                }
            }
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// 获取表示为字符串的输入的当前值。
    /// </summary>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="instance">实现了 <see cref="IHasInputValue{TValue}"/> 接口的实例。</param>
    /// <param name="value">输入一个新值。</param>
    /// <returns>可解析的字符串。</returns>
    public static string? GetCurrentValueAsString<TValue>(this IHasInputValue<TValue?> instance, string? value)
    {
        bool parsingFailed = false;

        if ( Nullable.GetUnderlyingType(typeof(TValue?)) != null && string.IsNullOrEmpty(value) )
        {
            // Assume if it's a nullable type, null/empty inputs should correspond to default(T)
            // Then all subclasses get nullable support almost automatically (they just have to
            // not reject Nullable<T> based on the type itself).
            parsingFailed = false;
            instance!.ChangeValue(default);
        }
        else if ( instance.TryParseValueFromString(value, out var validationErrorMessage) )
        {
            parsingFailed = false;
        }
        else
        {
            parsingFailed = true;

            if ( instance.CascadedEditContext is not null )
            {
                var field = instance.GetFieldIdentifier();
                if ( field is not null )
                {
                    var _parsingValidationMessages = new ValidationMessageStore(instance.CascadedEditContext);
                    _parsingValidationMessages.Add(field.Value, validationErrorMessage!);

                    // Since we're not writing to CurrentValue, we'll need to notify about modification from here
                    instance.CascadedEditContext.NotifyFieldChanged(field.Value);
                }
            }
        }
        if ( parsingFailed )
        {
            instance.CascadedEditContext?.NotifyValidationStateChanged();
        }
        return instance.GetValueAsString();
    }

    /// <summary>
    /// 为更改的值创建回调。
    /// </summary>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="instance">实现了 <see cref="IHasInputValue{TValue}"/> 接口的实例。</param>
    /// <returns>一个带有 <see cref="ChangeEventArgs"/> 参数的回调。</returns>
    public static EventCallback<ChangeEventArgs> CreateValueChangedCallback<TValue>(this IHasInputValue<TValue?> instance)
        => HtmlHelper.Instance.Callback().CreateBinder<string?>(instance, value => instance!.GetCurrentValueAsString(value), instance.GetValueAsString());


    /// <summary>
    /// 初始化实现 <see cref="IHasInputValue{TValue}"/> 的组件。
    /// <para>
    /// 可以手动在 <see cref="BlazorComponentBase.AfterSetParameters(ParameterView)"/> 进行调用以实现表单的验证初始化。
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="instance">实现了 <see cref="IHasInputValue{TValue}"/> 接口的实例。</param>
    /// <param name="validateionStateChangedHandler">一个用于引发 <see cref="EditContext.OnValidationStateChanged"/> 事件的委托处理程序。</param>
    /// <exception cref="InvalidOperationException"><see cref="IHasInputValue{TValue}.ValueExpression"/> 是必须的.</exception>
    public static void InitializeInputValue<TValue>(this IHasInputValue<TValue?> instance, EventHandler<ValidationStateChangedEventArgs>? validateionStateChangedHandler = default)
    {
        if ( instance.ValueExpression is null )
        {
            throw new InvalidOperationException($"{instance.GetType()} requires a value for the 'ValueExpression' " +
                $"parameter. Normally this is provided automatically when using 'bind-Value'.");
        }
        if ( instance.CascadedEditContext is not null )
        {
            validateionStateChangedHandler??= (sender, args) =>
            {
                if ( instance is IBlazorComponent refreshableComponent )
                {
                    refreshableComponent.NotifyStateChanged();
                }
            };

            instance.CascadedEditContext.OnValidationStateChanged += validateionStateChangedHandler;
        }
    }

    /// <summary>
    /// 释放组件。
    /// <para>
    /// 注意: 需要手动调用并释放 <see cref="EditContext"/> 实例。
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="instance">实现了 <see cref="IHasInputValue{TValue}"/> 接口的实例。</param>
    /// <param name="validateionStateChangedHandler">一个用于引发 <see cref="EditContext.OnValidationStateChanged"/> 事件的委托处理程序。</param>
    public static void DisposeInputValue<TValue>(this IHasInputValue<TValue?> instance, EventHandler<ValidationStateChangedEventArgs>? validateionStateChangedHandler = default)
    {
        if ( instance.CascadedEditContext is not null )
        {
            instance.CascadedEditContext.OnValidationStateChanged -= validateionStateChangedHandler;
        }
    }
}
