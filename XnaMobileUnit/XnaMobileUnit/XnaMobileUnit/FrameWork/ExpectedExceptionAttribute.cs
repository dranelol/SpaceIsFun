using System;

namespace XnaMobileUnit.FrameWork
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