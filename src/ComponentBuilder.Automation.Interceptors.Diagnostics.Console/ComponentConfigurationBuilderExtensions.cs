namespace ComponentBuilder.Interceptors;

/// <summary>
/// The extensions of <see cref="ComponentConfigurationBuilder"/> class.
/// </summary>
public static class ComponentConfigurationBuilderExtensions
{
    /// <summary>
    /// Add an interceptor for console lifecycle diagnostics.
    /// </summary>
    public static ComponentConfigurationBuilder AddConsoleDiagnostic(this ComponentConfigurationBuilder builder)
        => builder.AddInterceptor<ConsoleDiagnosticInterceptor>();
}