using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

/// <summary>
/// A window object in javascript.
/// </summary>
public class Window : DomNode
{
    private readonly IJSRuntime _window;

    internal Window(IJSRuntime window, IJSObjectReference? customizeModule = null, IJSObjectReference? internalModule = null) : base(customizeModule, internalModule)
    {
        _window = window;
    }

    /// <summary>
    /// Display a dialog with an optional message, and to wait until the user dismisses the dialog.
    /// </summary>
    /// <param name="message">A string you want to display in the alert dialog, or, alternatively, an object that is converted into a string and displayed.</param>
    /// <returns></returns>
    public ValueTask Alert(string? message)
        => _window.InvokeVoidAsync("alert", message);
    /// <summary>
    /// Shifts focus away from the window.
    /// </summary>
    public ValueTask Blur() => _window.InvokeVoidAsync("blur");
    /// <summary>
    /// Closes the current window, or the window on which it was called.
    /// </summary>
    public ValueTask Close()=>_window.InvokeVoidAsync("close");

    /// <summary>
    /// Display a dialog with an optional message, and to wait until the user either confirms or cancels the dialog.
    /// </summary>
    /// <param name="message">A string you want to display in the confirmation dialog.</param>
    /// <returns>
    /// A boolean indicating whether OK (true) or Cancel (false) was selected. If a browser is ignoring in-page dialogs, then the returned value is always false.
    /// </returns>
    public ValueTask<bool> Confirm(string? message) => _window.InvokeAsync<bool>("confirm", message);

    /// <summary>
    /// Display a dialog with an optional message prompting the user to input some text, and to wait until the user either submits the text or cancels the dialog.
    /// </summary>
    /// <param name="message">A string of text to display to the user. Can be omitted if there is nothing to show in the prompt window.</param>
    /// <param name="defaultValue">A string containing the default value displayed in the text input field.</param>
    /// <returns>A string containing the text entered by the user, or null</returns>
    public ValueTask<string?> Prompt(string? message, object? defaultValue = default)
        => _window.InvokeAsync<string?>("prompt", message, defaultValue);
}
