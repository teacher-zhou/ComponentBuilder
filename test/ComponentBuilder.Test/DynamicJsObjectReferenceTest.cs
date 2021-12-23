using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var module = GetService<IJSObjectReference>();

            //TestContext.JSInterop.JSRuntime.Import("demo.js");

            dynamic dynamicObj = new DynamicJsReferenceObject(module);

            //dynamicObj.test();
            dynamicObj.test<string>();
            dynamicObj.test(1);
            dynamicObj.test<string>(2);
        }
    }
}
