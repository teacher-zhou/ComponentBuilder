using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ComponentBuilder.Interceptors;
internal class DebugInterceptor : IComponentInterceptor
{
    private readonly ILogger<DebugInterceptor>? _logger;

    public DebugInterceptor(IServiceProvider serviceProvider)
    {
        this._logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger<DebugInterceptor>();
    }

    public int Order { get; }

    public void InterceptOnAfterRender(IBlazorComponent component, in bool firstRender)
    {
        WriteDebugMessage(component, nameof(InterceptOnAfterRender), $"FirstRender:{firstRender}");
    }

    public void InterceptOnBuildingContent(IBlazorComponent component, RenderTreeBuilder builder, int sequence)
    {
        WriteDebugMessage(component, nameof(InterceptOnBuildingContent));
    }

    public void InterceptOnDispose(IBlazorComponent component)
    {
        WriteDebugMessage(component, nameof(InterceptOnDispose), $"{new string('=', 100)}\n");
    }

    public void InterceptOnInitialized(IBlazorComponent component)
    {
        WriteDebugMessage(component, nameof(InterceptOnInitialized));
    }

    public void InterceptOnParameterSet(IBlazorComponent component)
    {
        WriteDebugMessage(component, nameof(InterceptOnParameterSet));
    }

    public void InterceptOnSetParameters(IBlazorComponent component, in ParameterView parameters)
    {
        WriteDebugMessage($"{new string('=', 100)}");
        WriteDebugMessage(component, nameof(InterceptOnSetParameters));
    }

    public void InterceptOnBuildingAttributes(IBlazorComponent component, IDictionary<string, object?> attributes)
    {
        WriteDebugMessage(component, nameof(InterceptOnBuildingAttributes), $"Attributes: {string.Join(", ", attributes.Select(m => $"{m.Key}: {m.Value}"))}");
    }

    void WriteDebugMessage(IBlazorComponent component, string lifecyle, string? content = default)
    {
        var format = $"【{DateTime.Now}】{component.GetType().Name} | {lifecyle} | {content}";

        WriteDebugMessage(format);
    }

    void WriteDebugMessage(string content)
    {
        _logger?.LogDebug(new EventId(GetHashCode()), content);
        Debug.WriteLine(content);
        Console.WriteLine(content);
    }

    public void InterceptOnAttributesBuilding(IBlazorComponent component, IDictionary<string, object?> attributes)
    {
        WriteDebugMessage(component, nameof(InterceptOnAttributesBuilding), $"Attributes: {string.Join(", ", attributes.Select(m => $"{m.Key}: {m.Value}"))}");
    }
}
