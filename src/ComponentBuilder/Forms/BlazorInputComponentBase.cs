using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;

namespace ComponentBuilder.Forms;

/// <summary>
/// 表单输入组件的基类。这个基类自动集成了一个 <see cref="BlazorFormComponentBase{TForm}.EditContext"/> 参数。
/// 另外输入组件不在表单组件中，验证将不会触发，而不是抛出异常。
/// </summary>
/// <typeparam name="TValue">值得类型。</typeparam>
public abstract class BlazorInputComponentBase<TValue> : BlazorComponentBase, IHasTwoWayBinding<TValue>, IDisposable
{
    private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;
    private bool _hasInitializedParameters;
    private bool _previousParsingAttemptFailed;
    private ValidationMessageStore? _parsingValidationMessages;
    private Type? _nullableUnderlyingType;


    /// <summary>
    /// 初始化 <see cref="BlazorInputComponentBase{TValue}"/> 类的新实例。
    /// </summary>
    protected BlazorInputComponentBase()
    {
        _validationStateChangedHandler = OnValidateStateChanged;
    }

    /// <summary>
    /// 获取级联的 <see cref="EditContext"/> 参数，来自于表单组件。可能为 <c>null</c> 。
    /// </summary>
    [CascadingParameter] EditContext? CascadedEditContext { get; set; }
    /// <summary>
    /// 获取或设置输入的值。这应该与双向绑定一起使用。
    /// </summary>
    /// <example>
    /// @bind-Value="model.PropertyName"
    /// </example>
    [Parameter] public TValue? Value { get; set; }

    /// <summary>
    /// 获取或设置更新绑定值的回调。
    /// </summary>
    [Parameter] public EventCallback<TValue?> ValueChanged { get; set; }

    /// <summary>
    /// 获取或设置标识绑定值的表达式。
    /// </summary>
    [Parameter] public Expression<Func<TValue?>> ValueExpression { get; set; }


    private string? _displayName;
    /// <summary>
    /// 获取或设置此字段的显示名称。如果为 <c>null</c>, 将自动获取 <see cref="DisplayAttribute.Name"/> 的值。
    /// <para>通常，当输入值无法正确解析时，将使用此值生成错误消息。</para>
    /// </summary>
    [Parameter]
    public string? DisplayName
    {
        get
        {
            if (string.IsNullOrEmpty(_displayName))
            {
                return ValueExpression?.GetAttribute<TValue, DisplayAttribute>()?.Name;
            }
            return _displayName;
        }
        set => _displayName = value;
    }
    /// <summary>
    /// 获取或设置一个字符串，它向用户提供简短提示，说明字段中需要哪些类型的信息。
    /// </summary>
    [Parameter][HtmlAttribute] public string Placeholder { get; set; }

    /// <summary>
    /// 被关联的 <see cref="BlazorFormComponentBase{TForm}.EditContext"/>.
    /// </summary>
    protected EditContext EditContext { get; set; } = default!;

    /// <summary>
    /// Gets the <see cref="FieldIdentifier"/> for the bound value.
    /// </summary>
    protected internal FieldIdentifier FieldIdentifier { get; set; }

    /// <summary>
    /// 获取或设置输入的当前值。
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
                _ = ValueChanged.InvokeAsync(Value);
                EditContext?.NotifyFieldChanged(FieldIdentifier);
            }
        }
    }

    /// <summary>
    /// 获取或设置以字符串形式表示的输入的当前值。
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
    /// 触发双向绑定的 HTML 事件名称。默认是“onchange”。
    /// </summary>
    protected virtual string EventName => "onchange";

    /// <summary>
    /// 将值格式化为字符串。派生类可以重写此值以确定使用 <see cref="CurrentValueAsString"/> 的值。
    /// </summary>
    /// <param name="value">要格式化的值。</param>
    /// <returns>值的字符串表示形式。</returns>
    protected virtual string? FormatValueAsString(TValue? value)
        => value?.ToString();

    /// <summary>
    /// 用于对 <typeparamref name="TValue"/> 值进行类型解析的操作。 派生类可以重写该方法如何对 <see cref="CurrentValueAsString"/> 进行类型转换。
    /// </summary>
    /// <param name="value">要解析的字符串值。</param>
    /// <param name="result"><typeparamref name="TValue"/> 的实例。</param>
    /// <param name="validationErrorMessage">如果无法解析该值，则提供验证错误消息。</param>
    /// <returns>如果值可以解析，则为 <c>true</c>，否则为 <c>false</c> 。</returns>
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
    /// 获取一个CSS类字符串，该字符串指示正在编辑的字段的状态(默认为“modified”、“valid”和“invalid”)。
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
            AdditionalAttributes["aria-invalid"] = "true";
        }
        else if (hasAriaInvalidAttribute)
        {
            // No validation errors. Need to remove `aria-invalid` if it was rendered already

            if (AdditionalAttributes!.Count == 1)
            {
                // Only aria-invalid argument is present which we don't need any more
                AdditionalAttributes = null;
            }
            else
            {
                AdditionalAttributes.Remove("aria-invalid");
            }
        }
    }

    /// <summary>
    /// 为 <see cref="EventName"/> 添加属性，用于创建双响绑定的回调函数。
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

