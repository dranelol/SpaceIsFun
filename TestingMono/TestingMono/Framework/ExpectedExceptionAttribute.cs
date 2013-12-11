using System;

namespace TestingMono.Framework
{
    public class ExpectedExceptionAttribute : Attribute
    {
        public Type ExceptionType;

        public ExpectedExceptionAttribute(Type exceptionType)
        {
            ExceptionType = exceptionType;
        }
    }
}
