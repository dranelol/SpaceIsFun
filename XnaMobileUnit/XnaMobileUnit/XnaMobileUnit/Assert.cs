using System;

namespace XnaMobileUnit
{
    public class Assert
    {
        private string _className;
        private string _methodName;

        public event EventHandler<TestEventArgs> FailedTest;
        public event EventHandler<TestEventArgs> PassedTest;

        internal string ClassName
        {
            get { return _className; }
        }

        internal string MethodName
        {
            get { return _methodName; }
        }

        internal void SetClassName(string className)
        {
            _className = className;
        }

        internal void SetMethodName(string methodName)
        {
            _methodName = methodName;
        }

        public void IsTrue(bool test, string message)
        {
            if(!test)
            {
                FailedTest(this, new TestEventArgs {Message = message, Expectation = "Expected true but was false", TestClass = _className, TestMethod = _methodName});
            }
            else
            {
                PassedTest(this, new TestEventArgs{Message = message, TestClass = _className, TestMethod = _methodName});
            } 
        }
         
        public void IsFalse(bool test, string message)
        {
            if (test)
            {
                FailedTest(this, new TestEventArgs { Message = message, Expectation = "Expected true but was false", TestClass = _className, TestMethod = _methodName });
            }
            else
            {
                PassedTest(this, new TestEventArgs { Message = message, TestClass = _className, TestMethod = _methodName });
            }
        }

        public void AreEqual<T>(T expected, T actual, string message)
        {
            if (!expected.Equals(actual))
            {
                FailedTest(this, new TestEventArgs { Message = message, Expectation = string.Format("Expected {0} but was {1}", expected, actual), TestClass = _className, TestMethod = _methodName });
            }
            else
            {
                PassedTest(this, new TestEventArgs { Message = message, TestClass = _className, TestMethod = _methodName });
            } 
        }

        public void AreNotEqual<T>(T expected, T actual, string message)
        {
            if (expected.Equals(actual))
            {
                FailedTest(this, new TestEventArgs { Message = message, Expectation = string.Format("Expected {0} but was {1}", expected, actual), TestClass = _className, TestMethod = _methodName });
            }
            else
            {
                PassedTest(this, new TestEventArgs { Message = message, TestClass = _className, TestMethod = _methodName });
            }
        }

        public void IsNull(object nullObject, string message)
        {
            if (nullObject != null)
            {
                FailedTest(this, new TestEventArgs { Message = message, Expectation = string.Format("Expected null"), TestClass = _className, TestMethod = _methodName });
            }
            else
            {
                PassedTest(this, new TestEventArgs { Message = message, TestClass = _className, TestMethod = _methodName });
            }
        }
    }
}