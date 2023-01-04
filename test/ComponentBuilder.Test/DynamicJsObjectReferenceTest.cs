using ComponentBuilder.JSInterope;
using Microsoft.JSInterop;
using Moq;

namespace ComponentBuilder.Test
{
    public class DynamicJsObjectReferenceTest : TestBase
    {
        public DynamicJsObjectReferenceTest()
        {

        }

        [Fact]
        public void Test_Dynamic_Call()
        {

            var mock = new Mock<IJSObjectReference>();

            var module = mock.Object;

            //TestContext.JSInterop.JSRuntime.Import("demo.js");

            dynamic dynamicObj = new DynamicJsReferenceObject(module);

            //dynamicObj.test();
            dynamicObj.test<string>();
            dynamicObj.test(1);
            dynamicObj.test<string>(2);
        }
    }
}
