using ComponentBuilder;

using Markdig;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddComponentBuilder();
await builder.Build().RunAsync();

public static class Code
{
    public static MarkupString Create(string value)
    {
        var content = Markdown.ToHtml(value);
        return new MarkupString(content);
    }
    public const string BgRun = "background:#ccc";
}