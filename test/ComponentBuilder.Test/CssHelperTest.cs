using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder.Test
{
    public class CssHelperTest
    {
        public CssHelperTest()
        {

        }

        [Fact]
        public void Test_MergeAttribute()
        {
            CssHelper.MergeAttributes(new Dictionary<string, object>()
            {
                ["max"] = 10,
                ["min"] = 5
            }).Should().ContainKey("max").And.ContainValue(10)
                .And.ContainKey("min").And.ContainValue(5)
                ;


            CssHelper.MergeAttributes(new { @class = "class", data_toggle = "toggle" })
                .Should().ContainKey("class").And.ContainValue("class")
                .And.ContainKey("data-toggle").And.ContainValue("toggle");
        }

        [Fact]
        public void Test_GetCssClassByCondition()
        {
            CssHelper.GetCssClass(true, "active").Should().Be("active");
            CssHelper.GetCssClass(false, "show", "hide").Should().Be("hide");
            CssHelper.GetCssClass(false, "active").Should().BeEmpty();
            CssHelper.GetCssClass(true, "active", prepend: "btn").Should().Be("btn active");
            CssHelper.GetCssClass(false, "show", "hide", "modal").Should().Be("modal hide");
        }

        [Fact]
        public void Test_CreateBuilder()
        {
            CssHelper.CreateBuilder().Append("active").Append("show")
                .ToString().Should().Be("active show");
        }
    }
}
