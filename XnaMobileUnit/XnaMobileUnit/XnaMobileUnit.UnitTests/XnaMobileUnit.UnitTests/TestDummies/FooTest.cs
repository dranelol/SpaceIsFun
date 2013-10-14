using System;
using XnaMobileUnit.FrameWork;

namespace XnaMobileUnit.UnitTests.TestDummies
{


    public class Foo
    {
        public string Bar()
        {
            return "bar";
        }
    }

    public class FooTest : TestFixture
    {
        public override void Context()
        {
            
        }

        [TestMethod]
        public void Should_return_bar()
        {
            Foo foo = new Foo();
            Assert.AreEqual("bar", foo.Bar(), "Should have returned bar");
        }
    }
}
