using ComponentBuilder.Definitions;

namespace ComponentBuilder.Interceptors;

/// <summary>
/// Represents an interceptor to add ChildContent parameter into content of <see cref="RenderTreeBuilder"/>.
/// </summary>
internal class ChildContentInterceptor : ComponentInterceptorBase
{    
    /// <inheritdoc/>
    public override void InterceptOnContentBuilding(IBlazorComponent component, RenderTreeBuilder builder, int sequence)
    {
        if ( component is IHasChildContent content )
        {
            builder.AddContent(sequence, content.ChildContent);
        }
    }
}
