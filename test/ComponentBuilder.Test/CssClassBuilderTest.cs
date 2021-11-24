using ComponentBuilder.Abstrations;

namespace ComponentBuilder.Test
{
    public class CssClassBuilderTest : ComponentBuilderTestBase
    {
        private readonly ICssClassBuilder _builder;
        public CssClassBuilderTest()
        {
            _builder = GetService<ICssClassBuilder>();
        }

        [Fact]
        public void Given_Invoke_Append_When_Value_Is_Null_Then_No_Css_Class_Return()
        {
            Assert.Empty(_builder.Append(null).ToString());
        }

        [Fact]
        public void Given_Invoke_Append_When_Input_Value_Twice_Then_After_Build_Get_The_Result_Separated_For_Each_Item()
        {
            _builder.Append("first").Append("second").ToString()
                .Should().Be("first second");
        }

    }
}
