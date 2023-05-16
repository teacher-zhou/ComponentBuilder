namespace ComponentBuilder.Automation.Interceptors;

/// <summary>
/// Reprents an interceptor to resolve form attribute and build automation form.
/// </summary>
internal class FormComponentInterceptor : ComponentInterceptorBase
{
    /// <inheritdoc/>
    public override void InterceptOnAttributesBuilding(IBlazorComponent component, IDictionary<string, object> attributes)
    {
        if (component is IFormComponent && !attributes.ContainsKey("onsubmit"))
        {
            attributes["onsubmit"] = HtmlHelper.CreateCallback().Create(component, () => SubmitFormAsync(component));
        }
    }

    /// <inheritdoc/>
    public override void InterceptOnParameterSet(IBlazorComponent component)
    {
        if (component is not IFormComponent form )
        {
            return;
        }

        var _hasSetEditContextExplicitly = form.EditContext is not null;

        if (_hasSetEditContextExplicitly && form.Model is not null)
        {
            throw new InvalidOperationException($"{GetType().Name} required a {nameof(form.Model)} " +
                $"paremeter, or a {nameof(form.EditContext)} parameter, but not both.");
        }
        else if (!_hasSetEditContextExplicitly && form.Model is null)
        {
            throw new InvalidOperationException($"{GetType().Name} requires either a {nameof(form.Model)} parameter, or an {nameof(EditContext)} parameter, please provide one of these.");
        }

        if (form.OnSubmit.HasDelegate && (form.OnValidSubmit.HasDelegate || form.OnInvalidSubmit.HasDelegate))
        {
            throw new InvalidOperationException($"when supplying a {nameof(form.OnSubmit)} parameter to {GetType().Name}, do not also supply {nameof(form.OnValidSubmit)} or {nameof(form.OnInvalidSubmit)}.");
        }

        if (form.EditContext is not null && form.Model is null)
        {
            form.FixedEditContext = form.EditContext;
        }
        else if (form.Model != null && form.Model != form.FixedEditContext?.Model)
        {
            form.FixedEditContext = new EditContext(form.Model!);
        }
    }

    /// <summary>
    /// Create cascading parameter for <see cref="EditContext"/> when component implemented from <see cref="IFormComponent"/> interface.
    /// </summary>
    /// <param name="component"><inheritdoc/></param>
    /// <param name="builder"><inheritdoc/></param>
    /// <param name="sequence"><inheritdoc/></param>
    public override void InterceptOnContentBuilding(IBlazorComponent component, RenderTreeBuilder builder, int sequence)
    {
        if (component is IFormComponent form )
        {
            builder.CreateCascadingComponent(form.FixedEditContext, 0, content =>
            {
                content.AddContent(0, form.ChildContent?.Invoke(form.FixedEditContext!));

            }, isFixed: true);
        }
    }

    /// <summary>
    /// Asynchorsouly submit current form component that implemented from <see cref="IFormComponent"/> interface.
    /// </summary>
    /// <returns>A task contains validation result after task is completed.</returns>
    static async Task<bool> SubmitFormAsync(IBlazorComponent component)
    {
        if (component is not IFormComponent form)
        {
            return false;
        }

        if (form.FixedEditContext is null)
        {
            return false;
        }

        if (form.OnSubmit.HasDelegate)
        {
            await form.OnSubmit.InvokeAsync(form.FixedEditContext);
            return false;
        }

        bool isValid = form.FixedEditContext.Validate();
        if (isValid && form.OnValidSubmit.HasDelegate)
        {
            await form.OnValidSubmit.InvokeAsync(form.FixedEditContext);
        }

        if (!isValid && form.OnInvalidSubmit.HasDelegate)
        {
            await form.OnInvalidSubmit.InvokeAsync(form.FixedEditContext);
        }
        return isValid;
    }
}
