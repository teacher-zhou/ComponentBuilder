using ComponentBuilder.JSInterope;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ComponentBuilder.Demo.ServerSide.Pages;
partial class CallbackPage
{
    [Inject] IJSRuntime JS { get; set; }

    string CallbackAction { get; set; }

    async Task ClickAction()
    {
        var demoJS = await JS.ImportAsync("./demo.js");

        demoJS.action(CallbackFactory.Create(() =>
        {
            CallbackAction = "Action is invoked";
            StateHasChanged();
        }));
    }

    async Task ClickFunc()
    {
        var demoJS = await JS.ImportAsync("./demo.js");

        demoJS.func(CallbackFactory.Create(() =>
        {
            return "Func is invoked";
        }));
    }
}
