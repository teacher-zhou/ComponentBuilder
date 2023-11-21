using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

/// <summary>
/// Represents the window object in the DOM.
/// </summary>
public class Window : DomNode
{
    private readonly IJSRuntime _window;

    internal Window(IJSRuntime window, IJSObjectReference? customizeModule = null, IJSObjectReference? internalModule = null) : base(customizeModule, internalModule)
    {
        _window = window;
    }

    /// <summary>
    /// Displays a dialog box with an optional message and waits until the user cancels the dialog.
    /// </summary>
    /// <param name="message">The string displayed in the dialog box, or the object converted to a string and displayed.</param>
    public ValueTask Alert(string? message) => _window.InvokeVoidAsync("alert", message);
    /// <summary>
    /// Move the focus away from the window.
    /// </summary>
    public ValueTask Blur() => _window.InvokeVoidAsync("blur");
    /// <summary>
    /// Close the current window or the window calling it.
    /// </summary>
    public ValueTask Close()=>_window.InvokeVoidAsync("close");

    /// <summary>
    /// Displays a dialog box with an optional message and waits until the user confirms or cancels the dialog.
    /// </summary>
    /// <param name="message">The string to display in the confirmation dialog.</param>
    /// <returns>
    /// A Boolean value indicating whether to select OK (true) or Cancel (false). If the browser ignores the in-page dialog, the return value is always false.
    /// </returns>
    public ValueTask<bool> Confirm(string? message) => _window.InvokeAsync<bool>("confirm", message);

    /// <summary>
    /// Displays a dialog box with an optional message, prompts the user to enter some text, and waits for the user to submit text or cancel the dialog.
    /// </summary>
    /// <param name="message">The text string to display to the user. If nothing appears in the prompt window, you can omit it.</param>
    /// <param name="defaultValue">A string containing the default values displayed in the text input field.</param>
    /// <returns>A string containing the text entered by the user, or empty.</returns>
    public ValueTask<string?> Prompt(string? message, string? defaultValue = default)
        => _window.InvokeAsync<string?>("prompt", message, defaultValue);
}
