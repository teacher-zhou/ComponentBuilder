using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;

/// <summary>
/// The extension for input.
/// </summary>
public static class InputValueExtensions
{
    /// <summary>
    /// Executed when the value is changed by the given value.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="instance">The instance implemented <see cref="IHasValueBound{TValue}"/>.</param>
    /// <param name="value">The new value to change.</param>
    /// <param name="afterChanged">A callback function when the value of an input parameter changes.</param>
    /// <returns>Return <c>true</c> if the value is changed, otherwise return <c>false</c>.</returns>
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
    /// Obtains <see cref="FieldIdentifier"/> from the given binding value.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="instance">The instance implemented <see cref="IHasValueBound{TValue}"/>.</param>
    /// <returns><see cref="FieldIdentifier"/> or null.</returns>
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
    /// Format and return the value as a string.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="instance">The instance implemented <see cref="IHasValueBound{TValue}"/>.</param>
    /// <param name="formatHandler">The handler to format.</param>
    /// <returns>A string representation of a value.</returns>
    public static string? GetValueAsString<TValue>(this IHasValueBound<TValue?> instance, Func<TValue?, string?>? formatHandler = default)
    {
        formatHandler ??= (value) => value?.ToString();
        return formatHandler.Invoke(instance.Value);
    }

    /// <summary>
    /// Operation for type resolution of <typeparamref name="TValue"/> values.
    /// <para>
    /// Generally, by communicating with users (e.g. <c>&lt;InputText /></c> component) interacts to invoke this method of the input component.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="instance">The instance implemented <see cref="IHasValueBound{TValue}"/>.</param>
    /// <param name="value">The string value to be parsed.</param>
    /// <param name="validationErrorMessage">If the value cannot be resolved, a validation error message is provided.</param>
    /// <param name="parsingHandler">A handler that parses strings to type <typeparamref name="TValue"/>.</param>
    /// <param name="validationErrorHandler">A handler that returns an error message.</param>
    /// <returns>Return <c>true</c> if the value was successfully parsed, otherwise <c>false</c>.</returns>
    /// <exception cref="NotSupportedException">The current instance does not support the type <typeparamref name="TValue"/>.</exception>
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
    /// Change the value of the current input component to the specified <paramref name="value"/> and <see cref="OnValueChanged{TValue}(IHasValueBound{TValue}, TValue, Func{bool, Task?}?)"/> method will be called.
    /// <para>
    /// When value is changed，the <see cref="EditContext.NotifyFieldChanged(in FieldIdentifier)"/> method will be called，and the <see cref="EditContext.OnFieldChanged"/> event will be raised.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="instance">The instance implemented <see cref="IHasValueBound{TValue}"/>.</param>
    /// <param name="value">A new value to input.</param>
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
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="instance">The instance implemented <see cref="IHasValueBound{TValue}"/>.</param>
    /// <param name="value">A new value to input.</param>
    /// <returns>A parsable string.</returns>
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
    /// Create a callback for the changed value.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="instance">The instance implemented <see cref="IHasValueBound{TValue}"/>.</param>
    /// <returns>A callback with the <see cref="ChangeEventArgs"/> argument.</returns>
    public static EventCallback<ChangeEventArgs> CreateValueChangedCallback<TValue>(this IHasInputValue<TValue?> instance)
        => HtmlHelper.Callback.CreateBinder<string?>(instance, value => instance!.GetCurrentValueAsString(value), instance.GetValueAsString());


    /// <summary>
    /// Initializes the component that implements <see cref="IHasInputValue{TValue}"/>.
    /// <para>
    /// Please manually in <see cref="BlazorComponentBase.AfterSetParameters (ParameterView)" /> call in order to realize the form validation of initialization.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="instance">The instance implemented <see cref="IHasValueBound{TValue}"/>.</param>
    /// <param name="validateionStateChangedHandler">One for the trigger <see cref = "EditContext.OnValidationStateChanged" /> events entrusted processing program.</param>
    /// <exception cref="InvalidOperationException"><see cref="IHasInputValue{TValue}.ValueExpression"/> is required.</exception>
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
    /// Dispose the component witch implement <see cref="IHasInputValue{TValue}"/> component.
    /// <para>
    /// Please invoke manually when component is disposed.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="instance">The instance implemented <see cref="IHasValueBound{TValue}"/>.</param>
    /// <param name="validateionStateChangedHandler">One for the trigger <see cref = "EditContext.OnValidationStateChanged" /> events entrusted processing program.</param>
    public static void DisposeInputValue<TValue>(this IHasInputValue<TValue?> instance, EventHandler<ValidationStateChangedEventArgs>? validateionStateChangedHandler = default)
    {
        if ( instance.CascadedEditContext is not null )
        {
            instance.CascadedEditContext.OnValidationStateChanged -= validateionStateChangedHandler;
        }
    }
}
