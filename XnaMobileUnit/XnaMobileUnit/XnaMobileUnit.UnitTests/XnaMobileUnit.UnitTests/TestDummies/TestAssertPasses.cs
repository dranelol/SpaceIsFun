using XnaMobileUnit.FrameWork;

namespace XnaMobileUnit.UnitTests.TestDummies
{
    public class TestAssertPasses : XnaMobileUnit.FrameWork.TestFixture
    {
        public override void Context()
        {

        }

        [TestMethod]
        public void should_pass_on_assert_is_false_with_false_argument()
        {
            Assert.IsFalse(false, "Assert.IsFalse(false)... should pass");
        }

        [TestMethod]
        public void should_pass_on_assert_is_true_with_true_argument()
        {
            Assert.IsTrue(true, "Assert.IsTrue(true)... should pass");
        }

        [TestMethod]
        public void should_pass_on_assert_are_equal_1_1_integers()
        {
            Assert.AreEqual(1, 1, "Assert.AreEqual(1, 1)... should pass");
        }

        [TestMethod]
        public void should_pass_on_assert_are_equal_A_A_strings()
        {
            Assert.AreEqual("A", "A", "Assert.AreEqual(\"A\", \"A\")... should pass");
        }

        [TestMethod]
        public void should_pass_on_assert_are_not_equal_A_B_strings()
        {
            Assert.AreNotEqual("A", "B", "Assert.AreNotEqual(\"A\", \"B\")... should pass");
        }


        [TestMethod]
        public void should_pass_on_assert_are_not_equal_1_0_integers()
        {
            Assert.AreNotEqual(1, 0, "Assert.AreNotEqual(1, 0)... should pass");
        }
    }
}