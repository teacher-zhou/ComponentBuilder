using ComponentBuilder;
using ComponentBuilder.Definitions;
using ComponentBuilder.Fluent;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace ComponentBuilder.Demo.ServerSide.Components;

/// <summary>
/// 表示可折叠的元素。
/// </summary>
public class Collapse : BlazorComponentBase,IHasChildContent
{

    /// <summary>
    /// 触发可折叠的内容。设置后会自动触发折叠/展开功能。
    /// <para>
    /// 外部组件可自行调用 <see cref="Toggle"/> 方法来控制折叠/展开功能。
    /// </para>
    /// </summary>
    [Parameter] public RenderFragment? TriggerContent { get; set; }
    /// <summary>
    /// 触发折叠/展开功能的 HTML 事件名称，默认 onclick。
    /// </summary>
    [Parameter] public string TriggerEvent { get; set; } = "onclick";
    /// <summary>
    /// 触发折叠/展开功能元素名称，默认 span。
    /// </summary>
    [Parameter] public string TriggerTagName { get; set; } = "span";
    /// <summary>
    /// 可折叠元素采用横向显示的动画。
    /// </summary>
    [Parameter] public bool Horizontal { get; set; }

    /// <summary>
    /// 设置为展开状态。
    /// </summary>
    [Parameter] public bool Active { get; set; }

    /// <summary>
    /// 设置一个回调函数，表示当可折叠元素显示时触发的事件。该事件具备一个 <see cref="bool"/> 参数，表示是否已经显示。
    /// </summary>
    [Parameter] public EventCallback<bool> OnShow { get; set; }
    /// <summary>
    /// 设置一个回调函数，表示当可折叠元素隐藏时触发的事件。该事件具备一个 <see cref="bool"/> 参数，表示是否已经隐藏。
    /// </summary>
    [Parameter] public EventCallback<bool> OnHide { get; set; }
    public RenderFragment? ChildContent { get; set; }

    private ElementReference? _triggerElement;
    private bool _isShow;
    private string id="collapse";
    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.Element(TriggerTagName, condition: TriggerContent is not null)
            .Callback(TriggerEvent, this, Toggle)
            .Content(TriggerContent)
            .Close();

        builder.CreateElement(10, "div", ChildContent, new
        {
            @class = HtmlHelper.Class.Append("collapse")
                                    .Append("collapse-horizontal", Horizontal)
                                    .Append("show", Active),
            id
        }
        //,        captureReference: element => _triggerElement = element
        );
    }

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["aria-expanded"] = _isShow.ToString().ToLower();
    }

    /// <summary>
    /// 将可折叠元素切换为显示或隐藏。
    /// </summary>
    public Task Toggle() => _isShow ? Hide() : Show();
    protected IJSObjectReference? BootstrapJS { get; private set; }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if ( firstRender )
        {
                BootstrapJS = await JS.Value.ImportJSAsync();
        }
    }
    protected async Task InvokeVoidAsync(string identifier, params object?[] args)
    {
        await BootstrapJS.InvokeVoidAsync(identifier, args);
    }
    /// <summary>
    /// 隐藏可折叠元素。
    /// </summary>
    public async Task Hide()
    {
        await OnHide.InvokeAsync(false);
        await InvokeVoidAsync("collapse.hide", id);
        await OnHide.InvokeAsync(true);
        _isShow = false;
    }

    /// <summary>
    /// 显示可折叠元素。
    /// </summary>
    public async Task Show()
    {
        await OnShow.InvokeAsync(false);
        await InvokeVoidAsync("collapse.show", id);
        await OnShow.InvokeAsync(true);
        _isShow = true;
    }
}

public static class JSInteropExtensions
{
    public static ValueTask<IJSObjectReference> ImportJSAsync(this IJSRuntime js)
        => js.InvokeAsync<IJSObjectReference>("import", "./demo.js");
}