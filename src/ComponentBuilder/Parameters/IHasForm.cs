namespace ComponentBuilder.Parameters;

/// <summary>
/// Provides a component support form and validations.
/// </summary>
public interface IHasForm : IHasEditContext, IHasChildContent<EditContext>
{
    /// <summary>
    /// Specifies the top-level model object for the form. An editing context will be constructed for the model.
    /// </summary>
    object? Model { get; set; }
    /// <summary>
    /// The callback that will be invoked when the form is submitted. If this parameter is used, it is up to you to trigger any validation manually, for example, by calling <see cref="EditContext.Validate"/> method.
    /// </summary>
    EventCallback<EditContext> OnSubmit { get; set; }
    /// <summary>
    ///  The callback function that will be called when the form is submitted, and then <see cref="EditContext"/> is judged to be valid.
    /// </summary>
    EventCallback<EditContext> OnValidSubmit { get; set; }
    /// <summary>
    ///  The callback function that will be called when the form is submitted, and then <see cref="EditContext"/> is judged to be invalid.
    /// </summary>
    EventCallback<EditContext> OnInvalidSubmit { get; set; }

    /// <summary>
    /// Gets a fixed edit context.
    /// </summary>
    EditContext? FixedEditContext { get; set; }
}
