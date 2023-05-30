namespace ComponentBuilder.Resolvers;
/// <summary>
/// The extensions of <see cref="ComponentConfigurationBuilder"/> class.
/// </summary>
public static class FluentClassComponentConfigurationBuilderExtensions
{
    /// <summary>
    /// 添加组件参数支持 <see cref="FluentClass.IFluentClassProvider"/> 构建丝滑 CSS 的解析器。
    /// </summary>
    public static ComponentConfigurationBuilder AddFluentClassResolver(this ComponentConfigurationBuilder builder)
    => builder.AddResolver<IParameterClassResolver, FluentCssClassResolver>();
}
