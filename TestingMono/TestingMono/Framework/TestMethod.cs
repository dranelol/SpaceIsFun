using System;

namespace TestingMono.Framework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class TestMethodAttribute : Attribute
    {

    }
}
