using Microsoft.JSInterop;
using System.Dynamic;
using System.Reflection;

namespace ComponentBuilder;

/// <summary>
/// A dynamic object for javascript reference.
/// </summary>
public sealed class DynamicJsReferenceObject : DynamicObject
{
    /// <summary>
    /// Gets the module imported from js.
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
