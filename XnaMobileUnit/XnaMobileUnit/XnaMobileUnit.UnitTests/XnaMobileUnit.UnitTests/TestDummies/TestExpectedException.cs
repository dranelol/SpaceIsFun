using System;
using XnaMobileUnit.FrameWork;

namespace XnaMobileUnit.UnitTests.TestDummies
{
    public class TestExpectedException : XnaMobileUnit.FrameWork.TestFixture
    {
        public override void Context()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_pass_because_of_expected_exception()
        {
            throw new NullReferenceException("Testing");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_fail_because_of_different_exception_type()
        {
            throw new ArgumentNullException("Testing");
        }


        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Should_fail_because_of_expected_exception()
        {
        }
    }
}