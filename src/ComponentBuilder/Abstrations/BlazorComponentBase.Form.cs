namespace ComponentBuilder;

/// <summary>
/// The partial of <see cref="BlazorComponentBase"/> class.
/// </summary>
partial class BlazorComponentBase
{

    #region Form

    EditContext? _fixedEditContext;
    private readonly Func<Task>? _handleSubmitDelegate;

    /// <summary>
    /// Only provides for component implemented from <see cref="IHasForm"/> and initialize the form parameters.
    /// </summary>
    protected void OnFormParameterSet()
    {
        if ( this is not IHasForm form )
        {
            return;
        }

        var _hasSetEditContextExplicitly = form.EditContext is not null;

        if ( _hasSetEditContextExplicitly && form.Model is not null )
        {
            throw new InvalidOperationException($"{GetType().Name} required a {nameof(form.Model)} " +
                $"paremeter, or a {nameof(form.EditContext)} parameter, but not both.");
        }
        else if ( !_hasSetEditContextExplicitly && form.Model is null )
        {
            throw new InvalidOperationException($"{GetType().Name} requires either a {nameof(form.Model)} parameter, or an {nameof(EditContext)} parameter, please provide one of these.");
        }

        if ( form.OnSubmit.HasDelegate && (form.OnValidSubmit.HasDelegate || form.OnInvalidSubmit.HasDelegate) )
        {
            throw new InvalidOperationException($"when supplying a {nameof(form.OnSubmit)} parameter to {GetType().Name}, do not also supply {nameof(form.OnValidSubmit)} or {nameof(form.OnInvalidSubmit)}.");
        }

        if ( form.EditContext is not null && form.Model is null )
        {
            _fixedEditContext = form.EditContext;
        }
        else if ( form.Model != null && form.Model != _fixedEditContext?.Model )
        {
            _fixedEditContext = new EditContext(form.Model!);
        }
    }

    /// <summary>
    /// Asynchorsouly submit current form component that implemented from <see cref="IHasForm"/> interface.
    /// </summary>
    /// <returns>A task contains validation result after task is completed.</returns>
    public async Task<bool> SubmitFormAsync()
    {
        if ( this is not IHasForm form )
        {
            return false;
        }

        if ( _fixedEditContext is null )
        {
            return false;
        }

        if ( form.OnSubmit.HasDelegate )
        {
            await form.OnSubmit.InvokeAsync(_fixedEditContext);
            return false;
        }

        bool isValid = _fixedEditContext.Validate();
        if ( isValid && form.OnValidSubmit.HasDelegate )
        {
            await form.OnValidSubmit.InvokeAsync(_fixedEditContext);
        }

        if ( !isValid && form.OnInvalidSubmit.HasDelegate )
        {
            await form.OnInvalidSubmit.InvokeAsync(_fixedEditContext);
        }
        return isValid;
    }

    #endregion
}
