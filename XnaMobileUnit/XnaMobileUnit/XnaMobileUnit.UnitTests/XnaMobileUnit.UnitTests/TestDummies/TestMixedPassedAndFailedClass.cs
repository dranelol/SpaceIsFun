using XnaMobileUnit.FrameWork;

namespace XnaMobileUnit.UnitTests.TestDummies
{
    public class TestMixedPassedAndFailedClass : XnaMobileUnit.FrameWork.TestFixture
    {
        public override void Context()
        {

        }

        [TestMethod]
        public void should_fail_on_assert_IsFalse_with_true_argument()
        {
            Assert.IsFalse(true, "Assert.IsFalse(true)... should fail");
        }

        [TestMethod]
        public void should_fail_on_assert_IsTrue_with_false_argument()
        {
            Assert.IsTrue(false, "Assert.IsTrue(false)... should fail");
        }

        [TestMethod]
        public void should_pass_on_assert_are_equal_1_1_integers()
        {
            Assert.AreEqual(1, 1, "Assert.AreEqual(0, 1)... should pass");
        }

        [TestMethod]
        public void should_pass_on_assert_are_not_equal_0_1_integers()
        {
            Assert.AreNotEqual(0, 1, "Assert.AreNotEqual(0, 1)... should pass");
        }
    }
}