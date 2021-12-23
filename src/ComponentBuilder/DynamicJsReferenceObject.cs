using Microsoft.JSInterop;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace ComponentBuilder;

public sealed class DynamicJsReferenceObject : DynamicObject
{
    public IJSObjectReference Module { get; }

    public DynamicJsReferenceObject(IJSObjectReference module)
    {
        Module = module;
    }

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
