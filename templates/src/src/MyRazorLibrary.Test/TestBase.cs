using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace MyRazorLibrary.Test;
/// <summary>
/// Represents a base class for unit test.
/// </summary>
public abstract class TestBase
{
    private readonly ServiceProvider _provider;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestBase"/> class.
    /// </summary>
    protected TestBase()
    {
        TestContext = new TestContext();
        ConfigureServices(TestContext.Services);
        _provider = TestContext.Services.BuildServiceProvider();
    }

    /// <summary>
    /// Gets the test context.
    /// </summary>
    protected TestContext TestContext { get; private set; }

    /// <summary>
    /// Gets the service from DI.
    /// </summary>
    /// <returns>The service or <c>null</c>.</returns>
    protected TService? GetService<TService>() => _provider.GetService<TService>();

    /// <summary>
    /// Configure services to DI.
    /// </summary>
    /// <param name="services">The service collection.</param>
    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddComponentBuilder();

        //add your services here
    }
}
/// <summary>
/// Represents a base class for unit test of specified component.
/// </summary>
/// <typeparam name="TComponent">The type of component.</typeparam>
public abstract class TestBase<TComponent> : TestBase where TComponent : IComponent
{
    /// <summary>
    /// Render component with parameters.
    /// </summary>
    /// <param name="parameterBuilder">A delegate to provide parameter of component.</param>
    protected IRenderedComponent<TComponent> RenderComponent(Action<ComponentParameterCollectionBuilder<TComponent>> parameterBuilder) => TestContext.RenderComponent(parameterBuilder);

    /// <summary>
    /// Render component with parameters.
    /// </summary>
    /// <param name="parameters">Parameters to provide.</param>
    protected IRenderedComponent<TComponent> RenderComponent(params ComponentParameter[] parameters) => TestContext.RenderComponent<TComponent>(parameters);

}