// ****************************************************************
// Copyright 2008, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

using System;
using System.Linq;
using System.Reflection;
using NUnit.Core;
using NUnit.Core.Extensibility;

namespace Ploeh.AutoFixture.NUnit.org.Builders
{
    /// <summary>
    /// Class to build ether a parameterized or a normal NUnitTestMethod.
    /// There are four cases that the builder must deal with:
    ///   1. The method needs no params and none are provided
    ///   2. The method needs params and they are provided
    ///   3. The method needs no params but they are provided in error
    ///   4. The method needs params but they are not provided
    /// This could have been done using two different builders, but it
    /// turned out to be simpler to have just one. The BuildFrom method
    /// takes a different branch depending on whether any parameters are
    /// provided, but all four cases are dealt with in lower-level methods
    /// </summary>
    public class AutoTestCaseBuilder : ITestCaseBuilder2
    {
        #region ITestCaseBuilder Methods

        /// <summary>
        /// Determines if the method can be used to build an NUnit test
        /// test method of some kind. The method must normally be marked
        /// with an identifying attriute for this to be true. If the test
        /// config file sets AllowOldStyleTests to true, then any method beginning 
        /// "test..." (case-insensitive) is treated as a test unless 
        /// it is also marked as a setup or teardown method.
        /// 
        /// Note that this method does not check that the signature
        /// of the method for validity. If we did that here, any
        /// test methods with invalid signatures would be passed
        /// over in silence in the test run. Since we want such
        /// methods to be reported, the check for validity is made
        /// in BuildFrom rather than here.
        /// </summary>
        /// <param name="method">A MethodInfo for the method being used as a test method</param>
        /// <param name="suite">The test suite being built, to which the new test would be added</param>
        /// <returns>True if the builder can create a test case from this method</returns>
        public bool CanBuildFrom(MethodInfo method)
        {
            return Reflect.HasAttribute(method, AutoFixtureNUnitFramework.DataAttribute, false);
        }

        /// <summary>
        /// Build a Test from the provided MethodInfo. Depending on
        /// whether the method takes arguments and on the availability
        /// of test case data, this method may return a single test
        /// or a group of tests contained in a ParameterizedMethodSuite.
        /// </summary>
        /// <param name="method">The MethodInfo for which a test is to be built</param>
        /// <param name="suite">The test fixture being populated, or null</param>
        /// <returns>A Test representing one or more method invocations</returns>
        public Test BuildFrom(MethodInfo method)
        {
            return BuildFrom(method, null);
        }

        #region ITestCaseBuilder2 Members

        public bool CanBuildFrom(MethodInfo method, Test parentSuite)
        {
            return CanBuildFrom(method);
        }

        public Test BuildFrom(MethodInfo method, Test parentSuite)
        {
            if (CanBuildFrom(method))
                return BuildParameterizedMethodSuite(method, parentSuite);

            return null;
        }

        /// <summary>
        /// Builds a ParameterizedMetodSuite containing individual
        /// test cases for each set of parameters provided for
        /// this method.
        /// </summary>
        /// <param name="method">The MethodInfo for which a test is to be built</param>
        /// <returns>A ParameterizedMethodSuite populated with test cases</returns>
        public static Test BuildParameterizedMethodSuite(MethodInfo method, Test parentSuite)
        {
            ParameterizedMethodSuite methodSuite = new ParameterizedMethodSuite(method);
            NUnitFramework.ApplyCommonAttributes(method, methodSuite);

            if (parentSuite != null)
            {
                if (parentSuite.RunState == RunState.NotRunnable && methodSuite.RunState != RunState.NotRunnable)
                {
                    methodSuite.RunState = RunState.NotRunnable;
                    methodSuite.IgnoreReason = parentSuite.IgnoreReason;
                }

                if (parentSuite.RunState == RunState.Ignored && methodSuite.RunState != RunState.Ignored && methodSuite.RunState != RunState.NotRunnable)
                {
                    methodSuite.RunState = RunState.Ignored;
                    methodSuite.IgnoreReason = parentSuite.IgnoreReason;
                }
            }

            Attribute[] attrs = Reflect.GetAttributes(method, AutoFixtureNUnitFramework.DataAttribute, false);
            Type[] parameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();

            foreach (DataAttribute attr in attrs)
            {
                foreach (var arguments in attr.GetData(method, parameterTypes))
                {
                    ParameterSet parms = new ParameterSet();
                    parms.Arguments = arguments;
                    
                    TestMethod test = BuildSingleTestMethod(method, parentSuite, parms);
                    methodSuite.Add(test);
                }
            }

            return methodSuite;
        }

        /// <summary>
        /// Builds a single NUnitTestMethod, either as a child of the fixture 
        /// or as one of a set of test cases under a ParameterizedTestMethodSuite.
        /// </summary>
        /// <param name="method">The MethodInfo from which to construct the TestMethod</param>
        /// <param name="parms">The ParameterSet to be used, or null</param>
        /// <returns></returns>
        public static NUnitTestMethod BuildSingleTestMethod(MethodInfo method, Test parentSuite, ParameterSet parms)
        {
#if CLR_2_0 || CLR_4_0
            NUnitTestMethod testMethod = Reflect.IsAsyncMethod(method) ?
                new NUnitAsyncTestMethod(method) : new NUnitTestMethod(method);
#else
            NUnitTestMethod testMethod = new NUnitTestMethod(method);
#endif

            string prefix = method.ReflectedType.FullName;

            if (parentSuite != null)
            {
                prefix = parentSuite.TestName.FullName;
                testMethod.TestName.FullName = prefix + "." + testMethod.TestName.Name;
            }

            if (CheckTestMethodSignature(testMethod, parms))
            {
                if (parms == null)
                    NUnitFramework.ApplyCommonAttributes(method, testMethod);
                NUnitFramework.ApplyExpectedExceptionAttribute(method, testMethod);
            }

            if (parms != null)
            {
                // NOTE: After the call to CheckTestMethodSignature, the Method
                // property of testMethod may no longer be the same as the
                // original MethodInfo, so we reassign it here.
                method = testMethod.Method;

                if (parms.TestName != null)
                {
                    testMethod.TestName.Name = parms.TestName;
                    testMethod.TestName.FullName = prefix + "." + parms.TestName;
                }
                else if (parms.OriginalArguments != null)
                {
                    string name = MethodHelper.GetDisplayName(method, parms.OriginalArguments);
                    testMethod.TestName.Name = name;
                    testMethod.TestName.FullName = prefix + "." + name;
                }

                if (parms.Ignored)
                {
                    testMethod.RunState = RunState.Ignored;
                    testMethod.IgnoreReason = parms.IgnoreReason;
                }
                else if (parms.Explicit)
                {
                    testMethod.RunState = RunState.Explicit;
                }

                if (parms.ExpectedExceptionName != null)
                    testMethod.ExceptionProcessor = new ExpectedExceptionProcessor(testMethod, parms);

                foreach (string key in parms.Properties.Keys)
                    testMethod.Properties[key] = parms.Properties[key];

                // Description is stored in parms.Properties
                if (parms.Description != null)
                    testMethod.Description = parms.Description;
            }

            //if (testMethod.BuilderException != null && testMethod.RunState != RunState.NotRunnable)
            //{
            //    testMethod.RunState = RunState.NotRunnable;
            //    testMethod.IgnoreReason = testMethod.BuilderException.Message;
            //}

            if (parentSuite != null)
            {
                if (parentSuite.RunState == RunState.NotRunnable && testMethod.RunState != RunState.NotRunnable)
                {
                    testMethod.RunState = RunState.NotRunnable;
                    testMethod.IgnoreReason = parentSuite.IgnoreReason;
                }

                if (parentSuite.RunState == RunState.Ignored && testMethod.RunState != RunState.Ignored && testMethod.RunState != RunState.NotRunnable)
                {
                    testMethod.RunState = RunState.Ignored;
                    testMethod.IgnoreReason = parentSuite.IgnoreReason;
                }
            }

            return testMethod;
        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// Helper method that checks the signature of a TestMethod and
        /// any supplied parameters to determine if the test is valid.
        /// 
        /// Currently, NUnitTestMethods are required to be public, 
        /// non-abstract methods, either static or instance,
        /// returning void. They may take arguments but the values must
        /// be provided or the TestMethod is not considered runnable.
        /// 
        /// Methods not meeting these criteria will be marked as
        /// non-runnable and the method will return false in that case.
        /// </summary>
        /// <param name="testMethod">The TestMethod to be checked. If it
        /// is found to be non-runnable, it will be modified.</param>
        /// <param name="parms">Parameters to be used for this test, or null</param>
        /// <returns>True if the method signature is valid, false if not</returns>
        private static bool CheckTestMethodSignature(TestMethod testMethod, ParameterSet parms)
        {
            if (testMethod.Method.IsAbstract)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Method is abstract";
                return false;
            }

            if (!testMethod.Method.IsPublic)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Method is not public";
                return false;
            }

            ParameterInfo[] parameters = testMethod.Method.GetParameters();
            int argsNeeded = parameters.Length;

            object[] arglist = null;
            int argsProvided = 0;

            if (parms != null)
            {
                //TODO GJvR these internal fields are the only issue have posted a question / issue on https://answers.launchpad.net/nunitv2/+question/234577 waiting for a response.
                //testMethod.arguments = parms.Arguments;
                //testMethod.hasExpectedResult = parms.HasExpectedResult;
                //if (testMethod.hasExpectedResult)
                //    testMethod.expectedResult = parms.Result;
                testMethod.RunState = parms.RunState;
                testMethod.IgnoreReason = parms.IgnoreReason;
                testMethod.BuilderException = parms.ProviderException;

                arglist = parms.Arguments;

                if (arglist != null)
                    argsProvided = arglist.Length;

                if (testMethod.RunState != RunState.Runnable)
                    return false;
            }


#if CLR_2_0 || CLR_4_0
            bool isAsyncMethod = Reflect.IsAsyncMethod(testMethod.Method);
            bool hasMeaningfulReturnType = isAsyncMethod ? testMethod.Method.ReturnType.IsGenericType : testMethod.Method.ReturnType != typeof(void);
#else
			bool hasMeaningfulReturnType = testMethod.Method.ReturnType != typeof(void);
#endif

            if (hasMeaningfulReturnType && (parms == null || !parms.HasExpectedResult && parms.ExpectedExceptionName == null))
                return MarkAsNotRunnable(testMethod, "Test method has non-void return type, but no result is expected");
            if (!hasMeaningfulReturnType && parms != null && parms.HasExpectedResult)
                return MarkAsNotRunnable(testMethod, "Test method has void return type, but a result is expected");

            if (argsProvided > 0 && argsNeeded == 0)
                return MarkAsNotRunnable(testMethod, "Arguments provided for method not taking any");

            if (argsProvided == 0 && argsNeeded > 0)
                return MarkAsNotRunnable(testMethod, "No arguments were provided");

            if (argsProvided != argsNeeded)
            {
                testMethod.RunState = RunState.NotRunnable;
                testMethod.IgnoreReason = "Wrong number of arguments provided";
                return false;
            }

#if CLR_2_0 || CLR_4_0
            if (testMethod.Method.IsGenericMethodDefinition)
            {
                Type[] typeArguments = GetTypeArgumentsForMethod(testMethod.Method, arglist);
                foreach (object o in typeArguments)
                    if (o == null)
                    {
                        testMethod.RunState = RunState.NotRunnable;
                        testMethod.IgnoreReason = "Unable to determine type arguments for fixture";
                        return false;
                    }

                var x = new NUnitTestMethod(testMethod.Method.MakeGenericMethod(typeArguments));
                parameters = x.Method.GetParameters();

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (arglist[i].GetType() != parameters[i].ParameterType && arglist[i] is IConvertible)
                    {
                        try
                        {
                            arglist[i] = Convert.ChangeType(arglist[i], parameters[i].ParameterType);
                        }
                        catch (Exception)
                        {
                            // Do nothing - the incompatible argument will be reported below
                        }
                    }
                }
            }
#endif

            return true;
        }

        private static bool MarkAsNotRunnable(TestMethod testMethod, string reason)
        {
            testMethod.RunState = RunState.NotRunnable;
            testMethod.IgnoreReason = reason;
            return false;
        }

#if CLR_2_0 || CLR_4_0
        private static Type[] GetTypeArgumentsForMethod(MethodInfo method, object[] arglist)
        {
            Type[] typeParameters = method.GetGenericArguments();
            Type[] typeArguments = new Type[typeParameters.Length];
            ParameterInfo[] parameters = method.GetParameters();

            for (int typeIndex = 0; typeIndex < typeArguments.Length; typeIndex++)
            {
                Type typeParameter = typeParameters[typeIndex];

                for (int argIndex = 0; argIndex < parameters.Length; argIndex++)
                {
                    if (parameters[argIndex].ParameterType.Equals(typeParameter))
                        typeArguments[typeIndex] = TypeHelper.BestCommonType(
                            typeArguments[typeIndex],
                            arglist[argIndex].GetType());
                }
            }

            return typeArguments;
        }
#endif
        #endregion

        #endregion
    }
}
