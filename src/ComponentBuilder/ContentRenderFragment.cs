namespace ComponentBuilder;

/// <summary>
/// 表示具备任意内容的 UI 渲染片段。
/// <para>
/// 支持 <see cref="string"/>、<see cref="RenderFragment"/> 和 <see cref="MarkupString"/> 类型。
/// </para>
/// </summary>
public sealed class ContentRenderFragment : OneOfBase<string?, RenderFragment?, MarkupString?>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContentRenderFragment"/> class.
    /// </summary>
    /// <param name="input">The input.</param>
    public ContentRenderFragment(OneOf<string?, RenderFragment?, MarkupString?> input) : base(input)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentRenderFragment"/> class.
    /// </summary>
    /// <param name="content">The content.</param>
    public ContentRenderFragment(RenderFragment? content):this(OneOf<string?, RenderFragment?, MarkupString?>.FromT1(content)) { }

    /// <summary>
    /// 对字符串的隐式转换。
    /// </summary>
    /// <param name="value">要转换的字符串。</param>
    public static implicit operator ContentRenderFragment(string? value)
        => new(value);
    /// <summary>
    /// 对 <see cref="RenderFragment"/> 的隐式转换。
    /// </summary>
    /// <param name="value">要转换的渲染片段。</param>
    public static implicit operator ContentRenderFragment(RenderFragment? value)
        => new(value);
    /// <summary>
    /// 对 <see cref="MarkupString"/> 的隐式转换。
    /// </summary>
    /// <param name="value">要转换的文本标记字符串。</param>
    public static implicit operator ContentRenderFragment(MarkupString? value)
        => new(value);

    /// <summary>
    /// 追加指定 <see cref="RenderTreeBuilder"/> 实例的内容。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    public void AddContent(RenderTreeBuilder builder,int sequence)
        => Switch(value => builder.AddContent(sequence, value??String.Empty),
                  value => builder.AddContent(sequence, value),
                  value => builder.AddContent(sequence, value));
}

