using XnaMobileUnit.FrameWork;

namespace XnaMobileUnit.UnitTests.TestDummies
{
    public class TestOnlyValidMethodsAreCalled : XnaMobileUnit.FrameWork.TestFixture
    {
        public override void Context()
        {

        }

        [TestMethod]
        private void should_not_executed_this_method_because_it_is_private()
        {
            Assert.IsFalse(true, "Assert.IsFalse(true) should fail");
        }

        [TestMethod]
        protected void should_not_execute_this_method_because_it_is_protected()
        {
            Assert.IsTrue(false, "This method is protected and should not be called");
        }

        public void should_not_executed_this_method_because_it_is_not_decorated_with_testmethod_attribute()
        {
            Assert.AreEqual(0, 1, "This method is not decorated with TestMethodAttribute and should not be called");
        }

        [TestMethod]
        public void should_fail_this_method_to_prove_only_valid_test_methods_execute()
        {
            Assert.AreNotEqual(0, 0, "This method is valid an should be called");
        }
    }
}