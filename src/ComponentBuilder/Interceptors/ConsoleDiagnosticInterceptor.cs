using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ComponentBuilder.Interceptors;
/// <summary>
/// Provides interceptors for debugging life cycle.
/// </summary>
internal class ConsoleDiagnosticInterceptor : IComponentInterceptor
{
    private readonly ILogger<ConsoleDiagnosticInterceptor>? _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleDiagnosticInterceptor"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    public ConsoleDiagnosticInterceptor(IServiceProvider serviceProvider)
    {
        this._logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger<ConsoleDiagnosticInterceptor>();
        WriteDebugMessage(@"
********************** Console Diagnostic ************************
");
    }

    /// <inheritdoc/>
    public int Order { get; }

    /// <inheritdoc/>
    public void InterceptOnAfterRender(IBlazorComponent component, in bool firstRender)
    {
        WriteDebugMessage(component, nameof(InterceptOnAfterRender), $"FirstRender:{firstRender}");
    }

    /// <inheritdoc/>
    public void InterceptOnContentBuilding(IBlazorComponent component, RenderTreeBuilder builder, int sequence)
    {
        WriteDebugMessage(component, nameof(InterceptOnContentBuilding));
    }

    /// <inheritdoc/>
    public void InterceptOnDisposing(IBlazorComponent component)
    {
        WriteDebugMessage(component, nameof(InterceptOnDisposing));
    }

    /// <inheritdoc/>
    public void InterceptOnInitialized(IBlazorComponent component)
    {
        WriteDebugMessage(component, nameof(InterceptOnInitialized));
    }

    /// <inheritdoc/>
    public void InterceptOnParameterSet(IBlazorComponent component)
    {
        WriteDebugMessage(component, nameof(InterceptOnParameterSet));
    }

    /// <inheritdoc/>
    public void InterceptOnSetParameters(IBlazorComponent component, in ParameterView parameters)
    {
        WriteDebugMessage(component, nameof(InterceptOnSetParameters));
    }

    /// <inheritdoc/>
    public void InterceptOnBuildingAttributes(IBlazorComponent component, IDictionary<string, object?> attributes)
    {
        WriteDebugMessage(component, nameof(InterceptOnBuildingAttributes), $"Attributes: {string.Join(", ", attributes.Select(m => $"{m.Key}: {m.Value}"))}");
    }

    /// <inheritdoc/>
    void WriteDebugMessage(IBlazorComponent component, string lifecyle, string? content = default)
    {
        var format = $"【{DateTime.Now}】{component.GetType().Name} | {lifecyle} | {content} | ChildComponents:{component.ChildComponents.Count}";

        WriteDebugMessage(format);
    }

    /// <inheritdoc/>
    void WriteDebugMessage(string? content)
    {
        _logger?.LogDebug(new EventId(GetHashCode()), content);
        Debug.WriteLine(content);
        Console.WriteLine(content);
    }

    /// <inheritdoc/>
    public void InterceptOnAttributesBuilding(IBlazorComponent component, IDictionary<string, object?> attributes)
    {
        WriteDebugMessage(component, nameof(InterceptOnAttributesBuilding), $"Attributes: {string.Join(", ", attributes.Select(m => $"{m.Key}: {m.Value}"))}");
    }
}
