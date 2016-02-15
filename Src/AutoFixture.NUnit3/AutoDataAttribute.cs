using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using Ploeh.AutoFixture.Kernel;
using ITestCaseData = NUnit.Framework.Interfaces.ITestCaseData;

namespace Ploeh.AutoFixture.NUnit3
{
    /// <summary>
    /// This attribute uses AutoFixture to generate values for unit test parameters. 
    /// This implementation is based on TestCaseAttribute of NUnit3
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AutoDataAttribute : NUnitAttribute, ITestBuilder, ITestCaseData, IImplyFixture
    {
        private readonly IFixture _fixture;

        /// <summary>
        /// Construct a <see cref="AutoDataAttribute"/>
        /// </summary>
        public AutoDataAttribute()
            : this(new Fixture())
        {
        }

        /// <summary>
        /// Construct a <see cref="AutoDataAttribute"/> with an <see cref="IFixture"/>
        /// </summary>
        protected AutoDataAttribute(IFixture fixture)
        {
            _fixture = fixture;
            this.RunState = RunState.Runnable;
            this.Properties = new PropertyBag();
        }

        /// <summary>
        ///     Construct one or more TestMethods from a given MethodInfo,
        ///     using available parameter data.
        /// </summary>
        /// <param name="method">The MethodInfo for which tests are to be constructed.</param>
        /// <param name="suite">The suite to which the tests will be added.</param>
        /// <returns>One or more TestMethods</returns>
        public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            var test = new NUnitTestCaseBuilder().BuildTestMethod(method, suite, this.GetParametersForMethod(method));

            yield return test;
        }

        private TestCaseParameters GetParametersForMethod(IMethodInfo method)
        {
            try
            {
                var specimenBuilder = new SpecimenContext(this._fixture);
                var parameterValues = method.GetParameters()
                    .Select(p => specimenBuilder.Resolve(p.ParameterType))
                    .ToArray();
                return new TestCaseParameters(parameterValues);
            }
            catch (Exception ex)
            {
                return new TestCaseParameters(ex);
            }
        }

        #region ITestData Members

        /// <summary>
        ///     Gets or sets the name of the test.
        /// </summary>
        /// <value>The name of the test.</value>
        public string TestName { get; set; }

        /// <summary>
        ///     Gets or sets the RunState of this test case.
        /// </summary>
        public RunState RunState { get; private set; }

        /// <summary>
        ///     Gets the list of arguments to a test case
        /// </summary>
        public object[] Arguments { get; private set; }

        /// <summary>
        ///     Gets the properties of the test case
        /// </summary>
        public IPropertyBag Properties { get; private set; }

        #endregion

        #region ITestCaseData Members

        /// <summary>
        ///     Gets or sets the expected result.
        /// </summary>
        /// <value>The result.</value>
        public object ExpectedResult
        {
            get
            {
                return this._expectedResult;
            }
            set
            {
                this._expectedResult = value;
                this.HasExpectedResult = true;
            }
        }

        private object _expectedResult;

        /// <summary>
        ///     Returns true if the expected result has been set
        /// </summary>
        public bool HasExpectedResult { get; private set; }

        #endregion

        #region Other Properties

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return this.Properties.Get(PropertyNames.Description) as string;
            }
            set
            {
                this.Properties.Set(PropertyNames.Description, value);
            }
        }

        /// <summary>
        ///     The author of this test
        /// </summary>
        public string Author
        {
            get
            {
                return this.Properties.Get(PropertyNames.Author) as string;
            }
            set
            {
                this.Properties.Set(PropertyNames.Author, value);
            }
        }

        /// <summary>
        ///     The type that this test is testing
        /// </summary>
        public Type TestOf
        {
            get
            {
                return this._testOf;
            }
            set
            {
                this._testOf = value;
                this.Properties.Set(PropertyNames.TestOf, value.FullName);
            }
        }

        private Type _testOf;

        /// <summary>
        ///     Gets or sets the reason for ignoring the test
        /// </summary>
        public string Ignore
        {
            get
            {
                return this.IgnoreReason;
            }
            set
            {
                this.IgnoreReason = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="NUnit.Framework.TestCaseAttribute" /> is explicit.
        /// </summary>
        /// <value>
        ///     <c>true</c> if explicit; otherwise, <c>false</c>.
        /// </value>
        public bool Explicit
        {
            get
            {
                return this.RunState == RunState.Explicit;
            }
            set
            {
                this.RunState = value ? RunState.Explicit : RunState.Runnable;
            }
        }

        /// <summary>
        ///     Gets or sets the reason for not running the test.
        /// </summary>
        /// <value>The reason.</value>
        public string Reason
        {
            get
            {
                return this.Properties.Get(PropertyNames.SkipReason) as string;
            }
            set
            {
                this.Properties.Set(PropertyNames.SkipReason, value);
            }
        }

        /// <summary>
        ///     Gets or sets the ignore reason. When set to a non-null
        ///     non-empty value, the test is marked as ignored.
        /// </summary>
        /// <value>The ignore reason.</value>
        public string IgnoreReason
        {
            get
            {
                return this.Reason;
            }
            set
            {
                this.RunState = RunState.Ignored;
                this.Reason = value;
            }
        }

#if !PORTABLE

        /// <summary>
        ///     Comma-delimited list of platforms to run the test for
        /// </summary>
        public string IncludePlatform { get; set; }

        /// <summary>
        ///     Comma-delimited list of platforms to not run the test for
        /// </summary>
        public string ExcludePlatform { get; set; }
#endif

        /// <summary>
        ///     Gets and sets the category for this test case.
        ///     May be a comma-separated list of categories.
        /// </summary>
        public string Category
        {
            get
            {
                return this.Properties.Get(PropertyNames.Category) as string;
            }
            set
            {
                foreach (var cat in value.Split(','))
                {
                    this.Properties.Add(PropertyNames.Category, cat);
                }
            }
        }

        #endregion
    }
}