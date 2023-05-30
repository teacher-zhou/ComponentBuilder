using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

/// <summary>
/// 表示 DOM 中的 window 对象。
/// </summary>
public class Window : DomNode
{
    private readonly IJSRuntime _window;

    internal Window(IJSRuntime window, IJSObjectReference? customizeModule = null, IJSObjectReference? internalModule = null) : base(customizeModule, internalModule)
    {
        _window = window;
    }

    /// <summary>
    /// 显示带有可选消息的对话框，并等待直到用户取消对话框。
    /// </summary>
    /// <param name="message">对话框中显示的字符串，或者是转换为字符串并显示的对象。</param>
    public ValueTask Alert(string? message)
        => _window.InvokeVoidAsync("alert", message);
    /// <summary>
    /// 将焦点从窗口移开。
    /// </summary>
    public ValueTask Blur() => _window.InvokeVoidAsync("blur");
    /// <summary>
    /// 关闭当前窗口或调用它的窗口。
    /// </summary>
    public ValueTask Close()=>_window.InvokeVoidAsync("close");

    /// <summary>
    /// 显示带有可选消息的对话框，并等待直到用户确认或取消对话框。
    /// </summary>
    /// <param name="message">要在确认对话框中显示的字符串。</param>
    /// <returns>
    /// 一个布尔值，指示选择OK (true)还是Cancel (false)。如果浏览器忽略页内对话框，则返回值始终为false。
    /// </returns>
    public ValueTask<bool> Confirm(string? message) => _window.InvokeAsync<bool>("confirm", message);

    /// <summary>
    /// 显示一个带有可选消息的对话框，提示用户输入一些文本，并等待用户提交文本或取消对话框。
    /// </summary>
    /// <param name="message">要显示给用户的文本字符串。如果提示窗口中没有显示任何内容，则可以省略。</param>
    /// <param name="defaultValue">包含在文本输入字段中显示的默认值的字符串。</param>
    /// <returns>包含用户输入的文本的字符串，或为空</returns>
    public ValueTask<string?> Prompt(string? message, string? defaultValue = default)
        => _window.InvokeAsync<string?>("prompt", message, defaultValue);
}
