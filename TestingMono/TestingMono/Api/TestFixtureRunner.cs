using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestingMono.Framework;
using Microsoft.Xna.Framework;

namespace TestingMono.Api
{
    public class TestFixtureRunner
    {
        private IList<TestFixture> _testFixtures = new List<TestFixture>();
        private TimeSpan _testDuration;
        private Game game;

        public IList<TestFixture> TestFixtures
        {
            get { return _testFixtures; }
        }

        public TestFixtureRunner(Game game)
        {
            this.game = game;
        }

        public List<TestLine> GetTestExecutionTree()
        {
            List<TestLine> testExecutionTree = new List<TestLine>();

            int testsFailed = 0;
            int testsPassed = 0;

            foreach (var testFixture in TestFixtures)
            {
                testExecutionTree.Add(new TestLine { TestClass = testFixture.ClassName, Message = string.Format("{0} TestFixture {1}", testFixture.ClassName, DidFixturePass(testFixture) ? "Passed" : "Failed"), Passed = DidFixturePass(testFixture) });

                foreach (var testPassed in testFixture.TestResults.PassedTests)
                {
                    testExecutionTree.Add(new TestLine { TestClass = testFixture.ClassName, TestMethod = testPassed.Value.TestMethod, Passed = true });
                    testsPassed++;
                }

                foreach (var testFailed in testFixture.TestResults.FailedTests)
                {
                    testExecutionTree.Add(new TestLine
                    {
                        TestClass = testFailed.Value.TestClass,
                        TestMethod = testFailed.Value.TestMethod,
                        Message = string.Format("{0} Failed: {1}", testFailed.Value.TestMethod, testFailed.Value.Message),
                        FailedReason = testFailed.Value.Expectation,
                        Passed = false
                    });
                    testsFailed++;
                }
            }

            testExecutionTree.Insert(0, new TestLine
            {
                Passed = true,
                Message = string.Format("Test took {0}{1}{2} to execute",
                                              GetMinutes(
                                                  (int)_testDuration.TotalMinutes),
                                              GetSeconds((int)_testDuration.Seconds),
                                              GetMilliseconds(
                                                  (int)_testDuration.Milliseconds))
            });

            testExecutionTree.Insert(1, new TestLine
            {
                Passed = true,
                Message = string.Format("{0} tests passed", testsPassed)
            });

            testExecutionTree.Insert(2, new TestLine
            {
                Passed = true,
                Message = string.Format("{0} tests failed", testsFailed)
            });

            return testExecutionTree;
        }

        private bool DidFixturePass(TestFixture testFixture)
        {
            if (testFixture.TestResults.FailedTests.Count > 0)
                return false;

            return true;
        }

        private string GetMinutes(int totalMinutes)
        {
            string minutes = string.Empty;

            if (totalMinutes > 0)
            {
                minutes = string.Format(" {0} minutes ", totalMinutes);
            }

            return minutes;
        }

        private string GetSeconds(int totalSeconds)
        {
            string seconds = string.Empty;

            if (totalSeconds > 0)
            {
                seconds = string.Format(" {0} seconds ", totalSeconds);
            }

            return seconds;
        }

        private string GetMilliseconds(int totalMilliseconds)
        {
            string milliSeconds = string.Empty;

            if (totalMilliseconds > 0)
            {
                milliSeconds = string.Format(" {0} miliseconds ", totalMilliseconds);
            }

            return milliSeconds;
        }


        public void RunTests(Assembly assembly)
        {
            DateTime started = DateTime.Now;

            foreach (TestFixture testFixture in GetTestFixtures(assembly))
            {
                _testFixtures.Add(testFixture);
                CallTestFixtureContext(testFixture);

                foreach (MethodInfo methodInfo in GetTestMethods(testFixture))
                {

                    CallTestSetup(testFixture, methodInfo);

                    Type exceptionType = null;

                    try
                    {
                        exceptionType = GetExpectedExceptionType(methodInfo, exceptionType);
                        CallTestMethod(testFixture, methodInfo);
                        FailTestOnExpectedExceptionNotThrown(testFixture, exceptionType);
                    }
                    catch (Exception exception)
                    {
                        HandleExceptionDuringTestMethod(testFixture, methodInfo, exceptionType, exception);
                    }

                    CallTestTeardown(testFixture);
                }
            }

            DateTime finished = DateTime.Now;

            _testDuration = finished - started;
        }

        private void CallTestMethod(TestFixture testFixture, MethodInfo methodInfo)
        {
            testFixture.SetClassName(testFixture.GetType().Name);
            testFixture.SetMethodName(methodInfo.Name);
            methodInfo.Invoke(testFixture, null);
        }

        private void CallTestTeardown(TestFixture testFixture)
        {
            try
            {
                testFixture.TearDown();
            }
            catch (Exception exception)
            {
                SetFixtureToFailed("TearDown", testFixture, exception);
            }
        }

        private void HandleExceptionDuringTestMethod(TestFixture testFixture, MethodInfo methodInfo, Type exceptionType, Exception exception)
        {
            if ((exceptionType != null && exception.InnerException != null && exception.InnerException.GetType() != exceptionType))
            {
                testFixture.Failed(string.Format("Unexpected exception expected {0} but was {1}... {2}", exceptionType.Name, exception.InnerException.GetType().Name, exception.Message));
            }
            else if (exceptionType != null && exception.InnerException != null && exception.InnerException.GetType() == exceptionType)
            {
                testFixture.Passed(methodInfo.Name);
            }
            else
            {
                testFixture.Failed(string.Format("Unexpected {0}... {1}", exception.GetType().Name, exception.Message));
            }
        }

        private void FailTestOnExpectedExceptionNotThrown(TestFixture testFixture, Type exceptionType)
        {
            if (exceptionType != null)
                testFixture.Failed("Expected exception of type " + exceptionType.ToString());
        }

        private Type GetExpectedExceptionType(MethodInfo methodInfo, Type exceptionType)
        {
            object[] testMethodAttributes =
                methodInfo.GetCustomAttributes(typeof(ExpectedExceptionAttribute), true);

            if (testMethodAttributes.Count() > 0)
            {
                exceptionType = ((ExpectedExceptionAttribute)testMethodAttributes[0]).ExceptionType;
            }
            return exceptionType;
        }

        private void CallTestFixtureContext(TestFixture testFixture)
        {
            try
            {
                testFixture.Context();
            }
            catch (Exception exception)
            {
                SetFixtureToFailed("Context", testFixture, exception);
            }
        }

        private void CallTestSetup(TestFixture testFixture, MethodInfo methodInfo)
        {
            try
            {
                testFixture.Setup();
            }
            catch (Exception exception)
            {
                testFixture.SetClassName(testFixture.GetType().Name);
                testFixture.SetMethodName(methodInfo.Name);
                SetFixtureToFailed("Setup", testFixture, exception);
            }
        }

        private void SetFixtureToFailed(string methodName, TestFixture testFixture, Exception exception)
        {
            testFixture.SetClassName(testFixture.GetType().Name);
            testFixture.SetMethodName(methodName);
            testFixture.Failed("An exception occured during " + methodName + "..." + exception.Message);
        }

        private IEnumerable<MethodInfo> GetTestMethods(TestFixture testFixture)
        {
            Type t = testFixture.GetType();
            List<MethodInfo> _testMethods = new List<MethodInfo>();

            foreach (MethodInfo methodInfo in t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                object[] testMethodAttributes =
                    methodInfo.GetCustomAttributes(typeof(TestMethodAttribute), true);

                if (testMethodAttributes.Count() > 0)
                {
                    _testMethods.Add(methodInfo);
                }
            }

            return _testMethods;
        }

        private IEnumerable<TestFixture> GetTestFixtures(Assembly assembly)
        {
            List<TestFixture> testFixtures = new List<TestFixture>();

            foreach (Type type in assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(TestFixture))))
            {
                object instance = Activator.CreateInstance(type) as TestFixture;
                

                if (instance != null)
                    testFixtures.Add((TestFixture)instance);
            }

            foreach (TestFixture test in testFixtures)
            {
                test.setGame(game);
            }

            return testFixtures;
        }
    }
}
