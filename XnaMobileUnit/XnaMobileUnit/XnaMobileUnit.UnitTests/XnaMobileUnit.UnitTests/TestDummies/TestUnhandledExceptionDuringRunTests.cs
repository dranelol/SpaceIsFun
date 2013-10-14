using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaMobileUnit.FrameWork;

namespace XnaMobileUnit.UnitTests.TestDummies
{
    public class WhenUnhandledExceptionDuringContext : TestFixture
    {
        public override void Context()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void should_execute_event_after_failure_in_context()
        {
            Assert.IsTrue(true, "This method should pass even though context failed");
        }

        [TestMethod]
        public void should_fail_and_throw_unexpected_exception()
        {
            throw new Exception("Simulating failure due to failure in Context");
        }
    }

    public class WhenUnhandledExceptionDuringSetup : TestFixture
    {
        public override void Context()
        {

        }

        public override void Setup()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void should_pass_even_though_setup_failed()
        {
            Assert.IsTrue(true, "Should have passed");
        }

        [TestMethod]
        public void should_throw_unexpected_exception_and_fail()
        {
            throw new Exception("Exception simulated due failure in setup");
        }
    }

    public class WhenUnhandledExceptionDuringTearDown : TestFixture
    {
        public override void Context()
        {

        }

        public override void TearDown()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void should_execute_test_prior_to_exception()
        {
            Assert.IsTrue(true, "This method should pass even though tear down should fail");
        }
    }
}
