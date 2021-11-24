using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// Provides a component has child content parameter.
    /// </summary>
    public interface IHasChildContent
    {
        /// <summary>
        /// Render a segment of UI content.
        /// </summary>
        RenderFragment? ChildContent { get; set; }
    }
}
