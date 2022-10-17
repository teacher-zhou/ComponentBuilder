using ComponentBuilder.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder
{
    public static class TreeRenderExtensions
    {
        public static BlazorTreeRender Begin(this RenderTreeBuilder builder, string elementName,int sequence=0)
        {
            var render = new BlazorTreeRender(builder);
            return render.Begin(elementName, sequence);
        }
        public static BlazorTreeRender Begin(this RenderTreeBuilder builder, Type componentType, int sequence = 0)
        {
            var render = new BlazorTreeRender(builder);
            return render.Begin(componentType, sequence);
        }

        public static BlazorTreeRender Begin<TComponent>(this RenderTreeBuilder builder, int sequence = 0) where TComponent : IComponent
        {
            var render = new BlazorTreeRender(builder);
            return render.Begin<TComponent>(sequence);
        }
    }
}
