namespace ComponentBuilder.Test
{
    public class ComponentBuilderExtensionsTest : AutoTestBase
    {
        public ComponentBuilderExtensionsTest()
        {

        }

        [Fact]
        public void TestEnumCssClassAttribute()
        {
            TestEnum.Class1.GetCssClassAttribute().Should().Be("class1");
            TestEnum.Class2.GetCssClassAttribute().Should().Be("myclass");
            PrefixEnum.Value1.GetCssClassAttribute().Should().Be("prefix-value1");
            PrefixEnum.Value2.GetCssClassAttribute().Should().Be("prefix-value2");
        }
        [Fact]
        public void TestMergeReplace()
        {
            var dic1 = new Dictionary<string, string>
            {
                ["name"] = "admin"
            };

            var dic2 = new Dictionary<string, string>
            {
                ["name"] = "test",
                ["age"] = "12"
            };

            dic1.AddOrUpdateRange(dic2);
            dic1.Should().ContainKey("age").And.Contain(m => m.Key == "name" && m.Value == "test");
        }
        [Fact]
        public void TestMergeIgnoreDuplicate()
        {
            var dic1 = new Dictionary<string, object>
            {
                ["name"] = "admin"
            };

            var dic2 = new Dictionary<string, object>
            {
                ["name"] = "test",
                ["age"] = "12"
            };

            dic1.AddOrUpdateRange(dic2, false);
            dic1.Should().ContainKey("age").And.Contain(m => m.Key == "name" && m.Value == "admin");
        }

        enum TestEnum
        {
            Class1,
            [CssClass("myclass")] Class2,
        }

        [CssClass("prefix-")]
        enum PrefixEnum
        {
            Value1,
            Value2
        }
    }
}
