global using Bunit;

namespace ComponentBuilder.Testing;

public abstract class TestBase
{
    protected TestContext TestContext { get; set; } = new TestContext();
}
