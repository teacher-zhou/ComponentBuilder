namespace ComponentBuilder.Definitions;

/// <summary>
/// Provides components to support forms and validation.
/// </summary>
[HtmlTag("form")]
public interface IFormComponent : IHasEditContext, IHasChildContent<EditContext>
{
    /// <summary>
    /// Specifies the top-level model object for the form. An editing context is constructed for the model.
    /// </summary>
    object? Model { get; set; }
    /// <summary>
    /// The callback that will be invoked when the form is submitted. 
    /// </summary>
    EventCallback<FormEventArgs> OnSubmit { get; set; }

    /// <summary>
    /// Gets a bool value represents form is submitting.
    /// </summary>
    public bool IsSubmitting { get; set; }
}

/// <summary>
/// The arguments when submit the form.
/// </summary>
/// <param name="context">The edit context.</param>
/// <param name="valid">A bool value represents validation result of form.</param>
public class FormEventArgs(EditContext context,bool valid) : EventArgs
{
    /// <summary>
    /// Gets the instance of <see cref="EditContext"/> class.
    /// </summary>
    public EditContext EditContext => context;
    /// <summary>
    /// Gets a bool value represents form validation result.
    /// </summary>
    public bool Valid => valid;
}