using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ComponentBuilder;
public static class InputValueExtensions
{


    /// <summary>
    /// Perform when value is changed by given value.
    /// </summary>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <param name="instance">the component instance.</param>
    /// <param name="value">A new value to change.</param>
    /// <param name="afterChanged">A callback function when value changed for input argument.</param>
    /// <returns><c>True</c> if value is changed, otherwise <c>false</c>.</returns>
    public static bool OnValueChanged<TValue>(this IHasValueBound<TValue> instance, TValue value, Func<bool, Task?>? afterChanged = default)
    {
        var hasChanged = !EqualityComparer<TValue>.Default.Equals(value, instance.Value);
        if ( hasChanged )
        {
            instance.Value = value;
            instance.ValueChanged.InvokeAsync(value);
        }
        afterChanged?.Invoke(hasChanged);
        return hasChanged;
    }

    /// <summary>
    /// Gets the <see cref="FieldIdentifier"/> from given bound value.
    /// </summary>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <param name="instance">the component instance.</param>
    /// <returns><see cref="FieldIdentifier"/> or null.</returns>
    public static FieldIdentifier? GetFieldIdentifier<TValue>(this IHasInputValue<TValue> instance)
    {
        var expression = instance.ValueExpression;
        if ( expression == null )
        {
            return default;
        }
        return FieldIdentifier.Create(expression);
    }
    /// <summary>
    /// Format the value as a string.
    /// </summary>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <param name="instance"></param>
    /// <param name="formatHandler">A handler to format.</param>
    /// <returns>A string representation of a value.</returns>
    public static string? FormatValueAsString<TValue>(this IHasValueBound<TValue> instance, Func<string?>? formatHandler = default)
    {
        formatHandler ??= () => instance.Value?.ToString();
        return formatHandler.Invoke();
    }

    /// <summary>
    /// The operation used for type resolution of <typeparamref name="TValue"/> values.
    /// <para>
    /// NORMALLY, invoke this method for input component by interaction with user like <c>&lt;InputText /> component</c>.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <param name="instance">the component instance.</param>
    /// <param name="value">The string value to parse.</param>
    /// <param name="validationErrorMessage">If the value cannot be resolved, a validation error message is provided.</param>
    /// <param name="parsingHandler">Handle to parse string to <typeparamref name="TValue"/>. </param>
    /// <param name="validationErrorHandler">Handle to return the validation error message.</param>
    /// <returns><c>True</c> if the value can be parsed; otherwise, <c>false</c> .</returns>
    /// <exception cref="NotSupportedException">Current instance dose not support the type of <typeparamref name="TValue"/>.</exception>
    public static bool TryParseValueFromString<TValue>([NotNull] this IHasInputValue<TValue> instance,
                                                       string? value,
                                                       out string? validationErrorMessage,
                                                       Func<string?>? validationErrorHandler = default,
                                                       Func<(bool isParsed, TValue? parsedValue)>? parsingHandler = default)
    {
        parsingHandler ??= () =>
        {
            var isParsed = BindConverter.TryConvertTo<TValue>(value, CultureInfo.CurrentCulture, out var parsedValue);
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
            throw new NotSupportedException($"{instance.GetType()} does not support the type '{typeof(TValue)}'.", ex);
        }
    }

    /// <summary>
    /// Change current value by specified <paramref name="value"/> and <see cref="OnValueChanged{TValue}(IHasValueBound{TValue}, TValue, Func{bool, Task?}?)"/> will be called.
    /// <para>
    /// The event <see cref="CascadedEditContext.OnFieldChanged"/> could be raised when value is changed.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The value of type.</typeparam>
    /// <param name="instance">the component instance.</param>
    /// <param name="value">A new value is input.</param>
    /// <returns>A <typeparamref name="TValue"/> representation of changed value.</returns>
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
    /// Gets the current value of the input represented as a string.
    /// </summary>
    /// <typeparam name="TValue">The value of type.</typeparam>
    /// <param name="instance">the component instance.</param>
    /// <param name="value">A new value is input.</param>
    /// <returns>The string value that can be parsed.</returns>
    public static string? GetCurrentValueAsString<TValue>(this IHasInputValue<TValue> instance, string? value)
    {
        bool parsingFailed = false;

        if ( Nullable.GetUnderlyingType(typeof(TValue)) != null && string.IsNullOrEmpty(value) )
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
        return instance.FormatValueAsString();
    }

    /// <summary>
    /// Create a callback for value changed.
    /// </summary>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <param name="instance">The component instance.</param>
    /// <returns>A callback with <see cref="ChangeEventArgs"/> argument.</returns>
    public static EventCallback<ChangeEventArgs> CreateValueChangedCallback<TValue>(this IHasInputValue<TValue> instance)
        => HtmlHelper.Event.CreateBinder<string?>(instance, value => instance!.GetCurrentValueAsString(value), instance.FormatValueAsString());


    /// <summary>
    /// Initialize the <see cref="IHasInputValue{TValue}"/> component.
    /// <para>
    /// NORMALLY, the method will be called in <see cref="ComponentBase.SetParametersAsync(ParameterView)"/> method by manually for automation binding validation.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <param name="instance">The component instance.</param>
    /// <param name="validateionStateChangedHandler">A handler to invoke when <see cref="EditContext.OnValidationStateChanged"/> event has raised.</param>
    /// <exception cref="InvalidOperationException"><see cref="IHasInputValue{TValue}.ValueExpression"/> is required.</exception>
    public static void InitializeInputValue<TValue>(this IHasInputValue<TValue> instance, EventHandler<ValidationStateChangedEventArgs>? validateionStateChangedHandler = default)
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
                if ( instance is IRazorComponent refreshableComponent )
                {
                    refreshableComponent.NotifyStateChanged();
                }
            };

            instance.CascadedEditContext.OnValidationStateChanged += validateionStateChangedHandler;
        }
    }

    /// <summary>
    /// Dispose input component.
    /// <para>
    /// NOTE: Invoke manually to release event of <see cref="CascadedEditContext"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="instance"></param>
    /// <param name="validateionStateChangedHandler">A handler to invoke when <see cref="CascadedEditContext.OnValidationStateChanged"/> event has raised.</param>
    public static void DisposeInputValue<TValue>(this IHasInputValue<TValue> instance, EventHandler<ValidationStateChangedEventArgs>? validateionStateChangedHandler = default)
    {
        if ( instance.CascadedEditContext is not null )
        {
            instance.CascadedEditContext.OnValidationStateChanged -= validateionStateChangedHandler;
        }
    }
}
