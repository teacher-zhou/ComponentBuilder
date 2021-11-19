using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder.Abstrations
{
    public class DefaultComponentBuilder : IComponentBuilder
    {
        public void BuildComponent<TComponent>(TComponent component, RenderTreeBuilder builder) where TComponent : BlazorComponentBase
        {
            builder.OpenElement(0, component.GetElementName());
            builder.AddAttribute(1, "class", component.GetCssClassString());
            builder.AddMultipleAttributes(10, component.AdditionalAttributes);
            AddChildContent(10, component, builder);
            builder.CloseElement();
        }

        private static void AddChildContent(int sequence, ComponentBase component, RenderTreeBuilder builder)
        {
            if (component is IHasChildContent content)
            {
                builder.AddAttribute(sequence, nameof(IHasChildContent.ChildContent), content.ChildContent);
            }
        }
    }
}
