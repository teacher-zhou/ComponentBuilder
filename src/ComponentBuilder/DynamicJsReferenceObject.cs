using System.Dynamic;
using System.Reflection;
using Microsoft.JSInterop;

namespace ComponentBuilder;

/// <summary>
/// 动态 javascript 引用对象。
/// </summary>
public sealed class DynamicJsReferenceObject : DynamicObject
{
    /// <summary>
    /// 获取 JS 引用模块。
    /// </summary>
    public IJSObjectReference Module { get; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="module"></param>
    public DynamicJsReferenceObject(IJSObjectReference module)
    {
        Module = module;
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="binder"></param>
    /// <param name="args"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        var csharpBinder = binder.GetType().GetInterface("Microsoft.CSharp.RuntimeBinder.ICSharpInvokeOrInvokeMemberBinder");
        var typeArgs =
            csharpBinder!.GetProperty("TypeArguments")?.GetValue(binder, null) as IList<Type> ??
            Array.Empty<Type>();

        var jsObjectReferenceType = typeof(IJSObjectReference);

        MethodInfo methodInfo;

        if (typeArgs.Any())
        {
            var method = jsObjectReferenceType
                .GetMethods()
                .First(x => x.Name.Contains(nameof(Module.InvokeAsync)));

            // only support one generic
            methodInfo = method.MakeGenericMethod(typeArgs.First());
            result = methodInfo.Invoke(Module, new object[] { binder.Name, args });
        }
        else
        {
            methodInfo = typeof(JSObjectReferenceExtensions).GetMethods().First(m => m.Name.Contains(nameof(JSObjectReferenceExtensions.InvokeVoidAsync)));
            result = methodInfo.Invoke(null, new object[] { Module, binder.Name, args });
        }

        //result = task;
        return true;
    }
}
