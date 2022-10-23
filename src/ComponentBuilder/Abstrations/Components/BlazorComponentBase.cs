namespace ComponentBuilder;
public class BlazorComponentBase : BlazorAbstractComponentBase, IHasAdditionalCssClass, IHasAdditionalStyle, IHasCssClassUtility
{
    [Parameter] public ICssClassUtility CssClass { get; set; }
    [Parameter] public string? AdditionalCssClass { get; set; }
    [Parameter] public string? AdditionalStyle { get; set; }
}
