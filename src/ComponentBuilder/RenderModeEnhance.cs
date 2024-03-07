namespace ComponentBuilder;

/// <summary>
/// The enhancement of render mode.
/// </summary>
public static class RenderModeEnhance
{
    /// <summary>
    /// Gets an <see cref="IComponentRenderMode"/> that represents rendering interactively on the server via Blazor Server hosting without server-side prerendering.
    /// </summary>
    public static IComponentRenderMode InteractiveServerWithoutPrerender => new InteractiveServerRenderMode(false);
    /// <summary>
    /// Gets an <see cref="IComponentRenderMode"/> that represents rendering interactively on the client via Blazor WebAssembly hosting without server-side prerendering.
    /// </summary>
    public static IComponentRenderMode InteractiveWebAssemblyWithoutPrerender => new InteractiveWebAssemblyRenderMode(false);
    /// <summary>
    /// Gets an <see cref="IComponentRenderMode"/> that means the render mode will be determined automatically based on a policy without server-side prerendering.
    /// </summary>
    public static IComponentRenderMode InteractiveAutoWithoutPrerender => new InteractiveAutoRenderMode(false);
}
