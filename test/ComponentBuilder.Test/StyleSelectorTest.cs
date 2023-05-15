using ComponentBuilder.Automation.Builder;

namespace ComponentBuilder.Automation.Test
{
    public class StyleSelectorTest
    {
        [Fact]
        public void Test_Output_Style()
        {
            var selectors = new StyleSelector();
            var style = selectors.AddStyle(".css", new { width = "100px", height = "100px" })
                .ToString();

            Assert.Equal(".css { width:100px; height:100px; }", style);
        }

        [Fact]
        public void Test_KeyFrame_Style()
        {
            var selectors = new StyleSelector();
            var keyframes = selectors.AddKeyFrames("transition", configure =>
            {
                configure.Add("from", new { width = "10px", transform = "rotate(180)" })
                .Add("to", new { width = "1000px" });
            }).ToString();
            Assert.Equal("@keyframes transition { from { width:10px; transform:rotate(180); } to { width:1000px; } }", keyframes);
        }
    }
}
