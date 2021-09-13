﻿using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace AutoFixture.NUnit3
{
    /// <summary>
    /// Utility used to create a <see cref="TestMethod"/> instance.
    /// </summary>
    public interface ITestMethodBuilder
    {
        /// <summary>
        /// Builds a <see cref="TestCaseParameters"/> from a method and the argument values.
        /// </summary>
        /// <param name="method">The <see cref="IMethodInfo"/> for which tests are to be constructed.</param>
        /// <param name="suite">The suite to which the tests will be added.</param>
        /// <param name="parameters">The argument values generated for the test case.</param>
        /// <param name="autoDataStartIndex">Index at which the automatically generated values start.</param>
        TestMethod Build(IMethodInfo method, Test suite, IEnumerable<TestParameterInfo> parameters, int autoDataStartIndex);
    }

    public class TestParameterInfo
    {
        public IParameterInfo Parameter { get; set; }

        public object Value { get; set; }

        public IEnumerable<IParameterCustomizationSource> CustomizationSources { get; set; }

        public bool HasValue { get; set; }
    }
}
