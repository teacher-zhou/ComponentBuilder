using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder.Test
{
    public class HtmlTagAttributeTest:AutoTestBase
    {
        [Fact]
        public void Test_HtmlTag_OnClass()
        {
            GetComponent<ClassComponent>().Should().HaveTag("p");
        }

        [Fact]
        public void Test_HtmlTag_OnInterface_And_Class_Without_Definition()
        {
            GetComponent<Link>().Should().HaveTag("a");
        }

        [Fact]
        public void Test_HtmlTag_OnInterface_And_Class_Has_HtmlTag()
        {
            GetComponent<MyPara>().Should().HaveTag("p");
        }
    }

    [HtmlTag("p")]
    class ClassComponent : BlazorComponentBase
    {

    }
    [HtmlTag("a")]
    public interface ILink : IBlazorComponent
    {

    }

    class Link : BlazorComponentBase,ILink
    {

    }

    [HtmlTag("button")]
    public interface IButton
    {

    }
    [HtmlTag("p")]
    class MyPara : BlazorComponentBase, IButton { }
}
