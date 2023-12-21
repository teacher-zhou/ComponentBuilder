namespace ComponentBuilder.Interceptors;

/// <summary>
/// Reprents an interceptor to resolve form attribute and build automation form.
/// </summary>
internal class FormComponentInterceptor : ComponentInterceptorBase
{
    /// <inheritdoc/>
    public override void InterceptOnAttributesBuilding(IBlazorComponent component, IDictionary<string, object?> attributes)
    {
        if (component is IFormComponent && !attributes.ContainsKey("onsubmit"))
        {
            attributes["onsubmit"] = HtmlHelper.Callback.Create(component, () => SubmitFormAsync(component));
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


        if (form.Model != null && form.EditContext is null)
        {
            form.EditContext = new EditContext(form.Model!);
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
            builder.CreateCascadingComponent(form.EditContext, 0, content =>
            {
                content.AddContent(0, form.ChildContent?.Invoke(form.EditContext!));

            }, isFixed: true);
        }
    }

    /// <summary>
    /// Asynchorsouly submit current form component that implemented from <see cref="IFormComponent"/> interface.
    /// </summary>
    /// <returns>A task contains validation result after task is completed.</returns>
    static async Task SubmitFormAsync(IBlazorComponent component)
    {
        if (component is not IFormComponent form)
        {
            return;
        }

        if (form.EditContext is null)
        {
            return;
        }
        form.IsSubmitting = true;
        bool isValid = form.EditContext.Validate();
        await form.OnSubmit.InvokeAsync(new(form.EditContext, isValid));
        form.IsSubmitting = false;
    }
}
