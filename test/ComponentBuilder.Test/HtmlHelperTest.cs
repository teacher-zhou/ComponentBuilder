using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder.Test
{
    public class HtmlHelperTest
    {
        public HtmlHelperTest()
        {

        }

        [Fact]
        public void Test_MergeAttribute()
        {
            HtmlHelper.MergeHtmlAttributes(new Dictionary<string, object>()
            {
                ["max"] = 10,
                ["min"] = 5
            }).Should().ContainKey("max").And.ContainValue(10)
                .And.ContainKey("min").And.ContainValue(5)
                ;


            HtmlHelper.MergeHtmlAttributes(new { @class = "class", data_toggle = "toggle" })
                .Should().ContainKey("class").And.ContainValue("class")
                .And.ContainKey("data-toggle").And.ContainValue("toggle");
        }

        [Fact]
        public void Test_GetCssClassByCondition()
        {
            HtmlHelper.CreateCssClass(true, "active").Should().Be("active");
            HtmlHelper.CreateCssClass(false, "show", "hide").Should().Be("hide");
            HtmlHelper.CreateCssClass(false, "active").Should().BeEmpty();
            HtmlHelper.CreateCssClass(true, "active", prepend: "btn").Should().Be("btn active");
            HtmlHelper.CreateCssClass(false, "show", "hide", "modal").Should().Be("modal hide");
        }

        [Fact]
        public void Test_CreateCssBuilder()
        {
            HtmlHelper.CreateCssBuilder().Append("active").Append("show")
                .ToString().Should().Be("active show");
        }

        [Fact]
        public void Test_CreateStyleBuilder()
        {
            HtmlHelper.CreateStyleBuilder().Append("display:block").ToString().Should().Be("display:block");
        }

    }
}
