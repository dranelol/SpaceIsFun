using System;

namespace XnaMobileUnit
{
    public class TestEventArgs : EventArgs
    {
        public string Message;
        public string TestMethod;
        public string TestClass;
        public string Expectation;
    }
}