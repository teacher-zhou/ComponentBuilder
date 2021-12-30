using ComponentBuilder.Abstrations;

namespace ComponentBuilder.Test
{
    public class CssClassResolverTest : TestBase
    {
        private readonly ICssClassAttributeResolver _resolver;
        public CssClassResolverTest()
        {
            _resolver = GetService<ICssClassAttributeResolver>();
        }

        [Fact]
        public void Given_Invoke_Resolve_Method_When_Input_Null_Value_Then_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _resolver.Resolve(null));
        }

        [Fact]
        public void Given_Component_For_Parameter_Mark_CssClass_When_Input_A_String_Then_Combile_Two_Of_Them()
        {
            var com = new ComponentWithStringParameter
            {
                Name = "abc",
                Block = true
            };

            var result = _resolver.Resolve(com);
            result.Should().Be("cssabc block");
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

        [Fact]
        public void Given_Component_Implement_ParameterInterface_When_Not_Use_CssClassAttribute_Then_Use_CssClass_From_Interface()
        {
            _resolver.Resolve(new InterfaceComponent { Active = true })
                .Should().Be("active");
        }

        [Fact]
        public void Given_Component_Implement_ParameterInterface_When_Use_CssClassAttribute_Then_Use_CssClass_From_Class()
        {
            _resolver.Resolve(new InterfaceComponent { Toggle = true })
                .Should().Be("toggle");

            _resolver.Resolve(new InterfaceComponent { Toggle = true, Active=true })
                .Should().Be("active toggle");
        }

        [Fact]
        public void Given_OrderedComponent_GetOrderedParameter_When_Parameter_Set_Then_Ordered()
        {
            _resolver.Resolve(new OrderedComponent { Active = true, Disabled = true })
                .Should().Be("hello disabled");
        }

        [Fact]
        public void Given_InterfaceClassComponent_When_Has_Interface_CssClassAttribute_Then_Use_Interface_CssClassAttribute()
        {
            _resolver.Resolve(new InterfaceClassComponent()).Should().Be("ui");
        }

        [Fact]
        public void Given_InterfaceClassComponent_When_Has_Interface_CssClassAttribute_But_Class_Has_CssClassAttribute_Then_Use_Class_CssClassAttribute()
        {
            _resolver.Resolve(new InterfaceClassOverrideComponent()).Should().Be("button");
        }
    }

    class ComponentWithStringParameter : BlazorComponentBase
    {
        [CssClass("css")] public string Name { get; set; }

        [CssClass("block")] public bool Block { get; set; }
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



    interface IActiveParameter
    {
        [CssClass("active")]bool Active { get; set; }
    }

    interface IToggleParameter
    {
        [CssClass("disabled")]bool Toggle { get; set; }
    }

    interface IDisableParameter
    {
        [CssClass("disabled",Order =100)]bool Disabled { get; set; }
    }

    [CssClass("ui")]
    interface IComponentUI
    {

    }

    class InterfaceComponent : BlazorComponentBase, IActiveParameter,IToggleParameter
    {
        public bool Active { get; set; }
        [CssClass("toggle")]public bool Toggle { get; set; }
    }

    
    class OrderedComponent : BlazorComponentBase, IActiveParameter, IDisableParameter
    {
        public bool Disabled { get; set; }
        [CssClass("hello", Order = 5)] public bool Active { get; set; }
    }

    class InterfaceClassComponent : BlazorComponentBase,IComponentUI
    {

    }

    [CssClass("button")]
    class InterfaceClassOverrideComponent : BlazorComponentBase, IComponentUI
    {

    }
}
