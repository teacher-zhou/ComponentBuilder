global using Bunit;

using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Test;

public abstract class TestBase
{
    protected TestContext TestContext { get; set; } = new TestContext();


    protected IRenderedComponent<TComponent> GetComponent<TComponent>(params ComponentParameter[] parameters) where TComponent : IComponent
        => TestContext.RenderComponent<TComponent>(parameters);

    protected IRenderedComponent<TComponent> GetComponent<TComponent>(Action<ComponentParameterCollectionBuilder<TComponent>> buildAction) where TComponent : IComponent
        => TestContext.RenderComponent(buildAction);
}
