using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace ComponentBuilder.Automation.Test;
public class FluentCssClassTest:TestBase
{
    [Fact]
    public void Test_FluentClass()
    {
        TestContext.RenderComponent<FluentComponent>(m => m.Add(p => p.Size,
            Class.Fluent.Small))
            .Should().HaveClass("small");
    }

    [Fact]
    public void Test_FluentClasProvider_Generate_Class()
    {
        TestContext.RenderComponent<FluentComponent>(m => m.Add(p => p.Size, 
            Class.Fluent.Small.Tablet))
            .Should().HaveClass("small-t");
    }

    [Fact]
    public void Test_Same_Key()
    {
        TestContext.RenderComponent<FluentComponent>(m => m.Add(p => p.Size,
            Class.Fluent.Small.Tablet.Small.Mobile))
            .Should().HaveClass("small-t").And.HaveClass("small-m");
    }
}

class FluentComponent : BlazorComponentBase
{
    [Parameter]public IFluentSize Size { get; set; }
}

class Class
{
    public static IFluentSize Fluent => new TestFluentProvider();
}

interface ISize: IFluentClassProvider
{
    IBreakPoint Small { get; }
    IBreakPoint Medium { get; }
    IBreakPoint Big { get; }
}

interface ISizeWithBreakPoint:ISize,IBreakPoint
{

}

interface IBreakPoint: IFluentClassProvider
{
    ISize Mobile { get; }
    ISize Tablet { get;}
}
interface IFluentSize : ISizeWithBreakPoint, IFluentClassProvider
{

}
class TestFluentProvider : FluentClassProvider<string,string?>, IFluentSize,IBreakPoint
{
    public ISize Mobile => WithBreakPoint("m");
    public ISize Tablet => WithBreakPoint("t");
    public IBreakPoint Small => WithSize("small");
    public IBreakPoint Medium => WithSize("medium");
    public IBreakPoint Big => WithSize("big");

    IBreakPoint WithSize(string size)
    {
        ChangeKey(size);
        return this;
    }

    ISize WithBreakPoint(string breakPoint)
    {
        AddRule(breakPoint);
        return this;
    }

    protected override string? Format(string key, string? value)
    {
        return $"{key}-{value}";
    }

    protected override string? Format(string key)
    {
        return key;
    }
}
