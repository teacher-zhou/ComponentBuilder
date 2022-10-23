using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;

namespace ComponentBuilder;

/// <summary>
/// 提供具备双向绑定的输入组件基类。如果 <see cref="EditContext"/> 为 <c>null</c> 则不会引发异常。
/// </summary>
/// <typeparam name="TValue">要绑定的值。</typeparam>
public abstract class BlazorInputComponentBase<TValue> : BlazorAbstractComponentBase, IHasTwoWayBinding<TValue>, IDisposable
{
    private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;
    private bool _hasInitializedParameters;
    private bool _previousParsingAttemptFailed;
    private ValidationMessageStore? _parsingValidationMessages;
    private Type? _nullableUnderlyingType;


    /// <summary>
    /// Initializes a new instance of the <see cref="BlazorInputComponentBase{TValue}"/> class.
    /// </summary>
    protected BlazorInputComponentBase()
    {
        _validationStateChangedHandler = OnValidateStateChanged;
    }

    /// <summary>
    /// Gets the cascading value of <see cref="Microsoft.AspNetCore.Components.Forms.EditContext"/>.
    /// </summary>
    [CascadingParameter] EditContext? CascadedEditContext { get; set; }
    /// <summary>
    /// Gets or sets the value of the input.
    /// </summary>
    /// <example>
    /// @bind-Value="model.PropertyName"
    /// </example>
    [Parameter] public TValue? Value { get; set; }

    /// <summary>
    /// Gets or sets the callback to update the binding value.
    /// </summary>
    [Parameter] public EventCallback<TValue?>? ValueChanged { get; set; }

    /// <summary>
    /// Gets or sets an expression that identifies the binding value.
    /// </summary>
    [Parameter] public Expression<Func<TValue?>>? ValueExpression { get; set; }


    private string? _displayName;
    /// <summary>
    /// Gets or sets the display name of field. if <c>null</c>, it will get the value from <see cref="DisplayAttribute.Name"/>.
    /// <para>Typically, this value is used to generate an error message when the input value cannot be properly parsed.</para>
    /// </summary>
    [Parameter]
    public string? DisplayName
    {
        get
        {
            if (string.IsNullOrEmpty(_displayName))
            {
                return ValueExpression?.GetAttribute<TValue?, DisplayAttribute>()?.Name;
            }
            return _displayName;
        }
        set => _displayName = value;
    }
    /// <summary>
    /// Gets or sets a string that gives the user a brief hint as to what type of information is required in the field.
    /// </summary>
    [Parameter][HtmlAttribute] public string? Placeholder { get; set; }

    /// <summary>
    /// Get cascading value of <see cref="BlazorFormComponentBase{TForm}.EditContext"/> from parent component.
    /// </summary>
    protected EditContext? EditContext { get; set; } = default!;

    /// <summary>
    /// Gets the <see cref="FieldIdentifier"/> for the bound value.
    /// </summary>
    protected internal FieldIdentifier FieldIdentifier { get; set; }

    /// <summary>
    /// Gets or sets the current value entered.
    /// </summary>
    protected TValue? CurrentValue
    {
        get => Value;
        set
        {
            var hasChanged = !EqualityComparer<TValue>.Default.Equals(value, Value);
            if (hasChanged)
            {
                Value = value;
                _ = ValueChanged?.InvokeAsync(Value);
                EditContext?.NotifyFieldChanged(FieldIdentifier);
            }
        }
    }

    /// <summary>
    /// Gets or sets the current value of the input represented as a string.
    /// </summary>
    protected string? CurrentValueAsString
    {
        get => FormatValueAsString(CurrentValue);
        set
        {
            _parsingValidationMessages?.Clear();

            bool parsingFailed;

            if (_nullableUnderlyingType != null && string.IsNullOrEmpty(value))
            {
                // Assume if it's a nullable type, null/empty inputs should correspond to default(T)
                // Then all subclasses get nullable support almost automatically (they just have to
                // not reject Nullable<T> based on the type itself).
                parsingFailed = false;
                CurrentValue = default!;
            }
            else if (TryParseValueFromString(value, out var parsedValue, out var validationErrorMessage))
            {
                parsingFailed = false;
                CurrentValue = parsedValue!;
            }
            else
            {
                parsingFailed = true;

                // EditContext may be null if the input is not a child component of EditForm.
                if (EditContext is not null)
                {
                    _parsingValidationMessages ??= new ValidationMessageStore(EditContext);
                    _parsingValidationMessages.Add(FieldIdentifier, validationErrorMessage);

                    // Since we're not writing to CurrentValue, we'll need to notify about modification from here
                    EditContext.NotifyFieldChanged(FieldIdentifier);
                }
            }

            // We can skip the validation notification if we were previously valid and still are
            if (parsingFailed || _previousParsingAttemptFailed)
            {
                EditContext?.NotifyValidationStateChanged();
                _previousParsingAttemptFailed = parsingFailed;
            }
        }
    }

    /// <summary>
    /// Gets the name of the HTML event that triggered the bidirectional binding. The default value is 'onchange'.
    /// </summary>
    protected virtual string EventName => "onchange";

    /// <summary>
    /// Format the value as a string. Derived classes can override this value to determine the value that uses <see cref="CurrentValueAsString"/>.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <returns>A string representation of a value.</returns>
    protected virtual string? FormatValueAsString(TValue? value)
        => value?.ToString();

    /// <summary>
    /// The operation used for type resolution of <typeparamref name="TValue"/> values. Derived classes can override how this method casts <see cref="CurrentValueAsString"/>.
    /// </summary>
    /// <param name="value">The string value to parse.</param>
    /// <param name="result"><typeparamref name="TValue"/> value is parsed.</param>
    /// <param name="validationErrorMessage">If the value cannot be resolved, a validation error message is provided.</param>
    /// <returns><c>True</c> if the value can be parsed; otherwise, <c>false</c> .</returns>
    protected virtual bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        try
        {
            if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.CurrentCulture, out var parsedValue))
            {
                result = parsedValue;
                validationErrorMessage = null;
                return true;
            }
            else
            {
                result = default;
                validationErrorMessage = $"The {DisplayName ?? FieldIdentifier.FieldName} field is not valid.";
                return false;
            }
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(TValue)}'.", ex);
        }
    }

    /// <summary>
    /// Gets a CSS-class string indicating the state of the field being edited (defaults to "Modified", "valid", and "invalid").
    /// </summary>
    protected virtual string FieldStatusCssClass => EditContext?.FieldCssClass(FieldIdentifier) ?? string.Empty;

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        if (!_hasInitializedParameters)
        {
            // This is the first run
            // Could put this logic in OnInit, but its nice to avoid forcing people who override OnInit to call base.OnInit()

            if (ValueExpression == null)
            {
                throw new InvalidOperationException($"{GetType()} requires a value for the 'ValueExpression' " +
                    $"parameter. Normally this is provided automatically when using 'bind-Value'.");
            }

            FieldIdentifier = FieldIdentifier.Create(ValueExpression);

            if (CascadedEditContext != null)
            {
                EditContext = CascadedEditContext;
                EditContext.OnValidationStateChanged += _validationStateChangedHandler;
            }

            _nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(TValue));
            _hasInitializedParameters = true;
        }
        else if (CascadedEditContext != EditContext)
        {
            // Not the first run

            // We don't support changing EditContext because it's messy to be clearing up state and event
            // handlers for the previous one, and there's no strong use case. If a strong use case
            // emerges, we can consider changing this.
            throw new InvalidOperationException($"{GetType()} does not support changing the " +
                $"{nameof(EditContext)} dynamically.");
        }

        UpdateAdditionalValidationAttributes();

        // For derived components, retain the usual lifecycle with OnInit/OnParametersSet/etc.
        return base.SetParametersAsync(ParameterView.Empty);
    }


    private void OnValidateStateChanged(object? sender, ValidationStateChangedEventArgs eventArgs)
    {
        UpdateAdditionalValidationAttributes();

        StateHasChanged();
    }

    private void UpdateAdditionalValidationAttributes()
    {
        if (EditContext is null)
        {
            return;
        }

        var hasAriaInvalidAttribute = AdditionalAttributes != null && AdditionalAttributes.ContainsKey("aria-invalid");
        if (EditContext.GetValidationMessages(FieldIdentifier).Any())
        {
            if (hasAriaInvalidAttribute)
            {
                // Do not overwrite the attribute value
                return;
            }
            // To make the `Input` components accessible by default
            // we will automatically render the `aria-invalid` attribute when the validation fails
            // value must be "true" see https://www.w3.org/TR/wai-aria-1.1/#aria-invalid
            AdditionalAttributes!["aria-invalid"] = "true";
        }
        else if (hasAriaInvalidAttribute)
        {
            // No validation errors. Need to remove `aria-invalid` if it was rendered already

            if (AdditionalAttributes!.Count == 1)
            {
                // Only aria-invalid argument is present which we don't need any more
                //AdditionalAttributes = null;
            }
            else
            {
                AdditionalAttributes.Remove("aria-invalid");
            }
        }
    }

    /// <summary>
    /// Add property with <see cref="EventName"/> to create a callback function for the two-way binding.
    /// </summary>
    /// <param name="attributes"></param>
    protected virtual void AddValueChangedAttribute(IDictionary<string, object> attributes)
    {
        attributes[EventName] = HtmlHelper.CreateCallbackBinder(this, _value => CurrentValue = _value, CurrentValue);
    }

    /// <inheritdoc />
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        base.BuildAttributes(attributes);

        AddValueChangedAttribute(attributes);
    }


    /// <inheritdoc/>
    protected virtual void Dispose(bool disposing)
    {
    }

    void IDisposable.Dispose()
    {
        // When initialization in the SetParametersAsync method fails, the EditContext property can remain equal to null
        if (EditContext is not null)
        {
            EditContext.OnValidationStateChanged -= _validationStateChangedHandler;
        }

        Dispose(disposing: true);
    }
}

