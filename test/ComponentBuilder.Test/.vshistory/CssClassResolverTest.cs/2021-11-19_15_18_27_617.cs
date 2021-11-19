using ComponentBuilder.Abstrations;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ComponentBuilder.Test
{
    public class CssClassResolverTest : ComponentBuilderTestBase
    {
        private readonly ICssClassResolver _resolver;
        public CssClassResolverTest()
        {
            _resolver = GetService<ICssClassResolver>();
        }

        [Fact]
        public void Given_Invoke_Resolve_Method_When_Input_Null_Value_Then_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _resolver.Resolve(null));
        }

        [Fact]
        public void Given_Component_For_Parameter_Mark_CssClass_When_Input_A_String_Then_Combile_Two_Of_Them()
        {
            var com = new ComponentWithStringParameter();
            com.Name = "abc";

            var result = _resolver.Resolve(com);
            result.Should().Be("cssabc");
        }

        [Fact]
        public void Given_Component_For_Enum_When_Parameter_Has_CssClassAttribute_Then_Get_The_Css_By_Enum_Member()
        {
            _resolver.Resolve(new ComponentWithEnumParameter
            {
                Color = ComponentWithEnumParameter.ColorType.Primary
            }).Should().Be("primary");

            _resolver.Resolve(new ComponentWithEnumParameter
            {
                Color = ComponentWithEnumParameter.ColorType.Light
            }).Should().Be("lt");


            _resolver.Resolve(new ComponentWithEnumParameter
            {
                BtnColor = ComponentWithEnumParameter.ColorType.Primary
            }).Should().Be("btn-primary");

            _resolver.Resolve(new ComponentWithEnumParameter
            {
                BtnColor = ComponentWithEnumParameter.ColorType.Light
            }).Should().Be("btn-lt");
        }
    }

    class ComponentWithStringParameter : BlazorComponentBase
    {
        [CssClass("css")] public string Name { get; set; }


    }

    class ComponentWithEnumParameter : BlazorComponentBase
    {
        internal enum ColorType
        {
            Primary,
            Secondary,
            Danger,
            [CssClass("lt")] Light
        }

        [CssClass] public ColorType? Color { get; set; }

        [CssClass("btn-")] public ColorType? BtnColor { get; set; }
    }

}
