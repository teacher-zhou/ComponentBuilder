namespace ComponentBuilder.Automation.Interceptors;

/// <summary>
/// The extensions of <see cref="ComponentConfigurationBuilder"/> class.
/// </summary>
public static class ComponentConfigurationBuilderExtensions
{
    /// <summary>
    /// Add <see cref="ConsoleDiagnosticInterceptor"/> to intercetor of component lifecycle.
    /// </summary>
    public static ComponentConfigurationBuilder AddConsoleDiagnostic(this ComponentConfigurationBuilder builder)
        => builder.AddInterceptor<ConsoleDiagnosticInterceptor>();
}