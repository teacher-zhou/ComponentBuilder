using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder.FluentRenderTree;

/// <summary>
/// 提供创建 HTML 元素的构造器。
/// </summary>
public interface IFluentOpenElementBuilder : IDisposable
{
    /// <summary>
    /// 创建 HTML 元素的开始标记。
    /// </summary>
    /// <param name="name">HTML 元素的名称。</param>
    /// <param name="sequence">一个表示源代码的序列。<c>null</c> 则系统会自动创建。</param>
    /// <returns><see cref="IFluentAttributeBuilder"/> 的实例。</returns>
    IFluentAttributeBuilder Element(string name, int? sequence = default);
}
