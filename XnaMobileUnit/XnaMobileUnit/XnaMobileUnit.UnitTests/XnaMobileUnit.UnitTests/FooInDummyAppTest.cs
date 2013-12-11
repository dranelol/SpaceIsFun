using DummyGameApp;
using XnaMobileUnit.FrameWork;

namespace XnaMobileUnit.UnitTests
{
    public class FooInDummyAppTest : TestFixture
    {
        public override void Context()
        {
            
        }

        [TestMethod]
        public void should_pass_foo_bar_test()
        {
            Foo foo = new Foo();

            Assert.IsTrue(foo.Bar(true), "Should be true");
        }

        [TestMethod]
        public void should_fail_foo_bar_test()
        {
            Foo foo = new Foo();

            Assert.IsFalse(foo.Bar(true), "Should be false");
        }
    }
}
