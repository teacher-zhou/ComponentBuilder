//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Rendering;

//namespace ComponentBuilder.Test;
//public class FragmentContentTest : TestBase
//{
//    public FragmentContentTest()
//    {

//    }

//    [Fact]
//    public void Given_A_FragmentContent_When_Set_HeadContent_As_String_Then_Display_The_String()
//    {
//        TestContext.RenderComponent<FragmentContentComponent>(p => p.Add(m => m.HeadContent, "hello"))
//            .MarkupMatches("<div>hello</div>");
//    }
//}


//class FragmentContentComponent : BlazorComponentBase
//{
//    [Parameter] public FragmentContent? HeadContent { get; set; } = "";

//    protected override void AddContent(RenderTreeBuilder builder, int sequence)
//    {
//        builder.AddContent(sequence, HeadContent?.Content);
//    }
//}
