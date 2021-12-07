using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace ComponentBuilder.Test
{
    public class ComponentAdditionalAttributeTest : TestBase
    {
        public ComponentAdditionalAttributeTest()
        {

        }

        [Fact]
        public void When_Has_Role_Attr_Then_Has_Role_Attribute()
        {
            TestContext.RenderComponent<AttributeComponent>()
                .Should().HaveAttribute("role", "alert");
        }

        [Fact]
        public void When_AddtionalAttributes_Is_Dic_Then_Has_AllAttributes()
        {
            TestContext.RenderComponent<AttributeComponent>(ComponentParameter.CreateParameter(nameof(AttributeComponent.AdditionalAttributes), new Dictionary<string, object>
            {
                ["max"] = 10,
                ["length"] = 25
            }))
                .Should().HaveAttribute("max", "10")
                .And.HaveAttribute("length", "25")
                ;
        }

        [Fact]
        public void When_AddtionalAttributes_Is_Object_Then_Has_AllAttributes()
        {
            TestContext.RenderComponent<AttributeComponent>(ComponentParameter.CreateParameter(nameof(AttributeComponent.AdditionalAttributes), new
            {
                max = 10,
                length = 25
            }))
                .Should().HaveAttribute("max", "10")
                .And.HaveAttribute("length", "25")
                ;
        }

        [Fact]
        public void When_Create_Uncaptured_Parameter_Then_Has_Attributes()
        {
            TestContext.RenderComponent<AttributeComponent>(builder =>
            {
                builder.AddUnmatched("max", "10").AddUnmatched("length", 25);
            })
                .Should().HaveAttribute("max", "10")
                .And.HaveAttribute("length", "25")
                ;
        }

        [Fact]
        public void Given_Class_Attr_Replace_Parameter_When_Block_Has_Value_And_Class_Attribute_Has_Value_Then_Should_Be_Class_Attribute()
        {
            TestContext.RenderComponent<AttributeComponent>(builder =>
            {
                builder.Add(m => m.Block, true);
                builder.AddUnmatched("class", "hello");
            })
                .Should().HaveAttribute("class", "hello")
                ;
        }
    }

    [ElementRole("alert")]
    class AttributeComponent : BlazorComponentBase
    {
        [Parameter] [CssClass("blocked")] public bool Block { get; set; }
    }
}

