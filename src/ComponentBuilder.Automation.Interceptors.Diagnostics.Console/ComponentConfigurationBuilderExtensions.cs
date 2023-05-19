namespace ComponentBuilder.Interceptors;

/// <summary>
/// The extensions of <see cref="ComponentConfigurationBuilder"/> class.
/// </summary>
public static class ComponentConfigurationBuilderExtensions
{
    /// <summary>
    /// 添加控制台生命周期诊断的拦截器功能。
    /// </summary>
    public static ComponentConfigurationBuilder AddConsoleDiagnostic(this ComponentConfigurationBuilder builder)
        => builder.AddInterceptor<ConsoleDiagnosticInterceptor>();
}