using System;

namespace TestingMono
{
    public class TestEventArgs : EventArgs
    {
        public string Message;
        public string TestMethod;
        public string TestClass;
        public string Expectation;
    }
}
