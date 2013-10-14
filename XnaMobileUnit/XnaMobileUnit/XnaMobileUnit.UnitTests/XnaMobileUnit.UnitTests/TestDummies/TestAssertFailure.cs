using XnaMobileUnit.FrameWork;

namespace XnaMobileUnit.UnitTests.TestDummies
{
    public class TestAssertFailure : XnaMobileUnit.FrameWork.TestFixture 
    {
        public override void Context()
        {
            
        }

        [TestMethod]
        public void should_fail_on_assert_IsFalse_with_true_argument()
        {
            Assert.IsFalse(true, "Assert.IsFalse(true)... if this failed then the test is valid");
        }

        [TestMethod]
        public void should_fail_on_assert_IsTrue_with_false_argument()
        {
            Assert.IsTrue(false, "Assert.IsTrue(false)... if this failed then the test is valid");
        }

        [TestMethod]
        public void should_fail_on_assert_are_equal_0_1__integers()
        {
            Assert.AreEqual(0, 1, "Assert.AreEqual(0, 1)... should fail");
        }

        [TestMethod]
        public void should_fail_on_assert_are_not_equal_0_0_integers()
        {
            Assert.AreNotEqual(0, 0, "Assert.AreNotEqual(0, 0)... should fail");
        }
    }
}
