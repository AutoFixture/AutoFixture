using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixture.NUnit
{
    /// <summary>
    /// An implementation of TestCaseDataAttribute that composes other TestCaseDataAttribute instances.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    [CLSCompliant(false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "This attribute is the root of a potential attribute hierarchy.")]
    public class CompositeTestCaseDataAttribute : TestCaseDataAttribute
    {
        private readonly IEnumerable<TestCaseDataAttribute> _attributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeTestCaseDataAttribute"/> class.
        /// </summary>
        /// <param name="attributes">The attributes representing a data source for a testcase.
        /// </param>
        public CompositeTestCaseDataAttribute(IEnumerable<TestCaseDataAttribute> attributes)
            : this(attributes.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeTestCaseDataAttribute"/> class.
        /// </summary>
        /// <param name="attributes">The attributes representing a data source for a testcase.
        /// </param>
        public CompositeTestCaseDataAttribute(params TestCaseDataAttribute[] attributes)
        {
            if (attributes == null)
            {
                throw new ArgumentNullException("attributes");
            }

            _attributes = attributes;
        }

        /// <summary>
        /// Gets the attributes supplied through one of the constructors.
        /// </summary>
        public IEnumerable<TestCaseDataAttribute> Attributes
        {
            get { return _attributes; }
        }

        /// <summary>
        /// Returns the composition of arguments to be used to test the testcase. Favors the arguments returned
        /// by TestCaseDataAttributes in ascending order. 
        /// </summary>
        /// <param name="method">The method that is being tested.</param>
        /// <returns>
        /// Returns the composition of the testcase arguments.
        /// </returns>
        public override IEnumerable<object[]> GetArguments(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            int numberOfParameters = method.GetParameters().Length;
            if (numberOfParameters <= 0)
                yield break;

            int numberOfIterations = 0;
            int iteration = 0;
            var foundData = new List<List<object>>();

            do
            {
                foreach (var attribute in _attributes)
                {
                    var attributeData = attribute.GetArguments(method).ToArray();

                    if (attributeData.Length <= iteration)
                    {
                        // No data found for this position.
                        break;
                    }

                    if (numberOfIterations == 0)
                    {
                        numberOfIterations = attributeData.Length;

                        for (int n = 0; n < numberOfIterations; n++)
                        {
                            foundData.Add(new List<object>());
                        }

                        if (foundData.Count == 0)
                        {
                            yield break;
                        }
                    }

                    var testcase = attributeData[iteration];

                    int remaining = numberOfParameters - foundData[iteration].Count;
                    if (remaining == numberOfParameters)
                    {
                        if (testcase.Length == numberOfParameters)
                        {
                            foundData[iteration].AddRange(testcase);
                            break;
                        }

                        if (testcase.Length > numberOfParameters)
                        {
                            foundData[iteration].AddRange(testcase.Take(numberOfParameters));
                            break;
                        }
                    }

                    if (remaining > testcase.Length)
                    {
                        foundData[iteration].AddRange(testcase);
                    }
                    else
                    {
                        int found = foundData[iteration].Count;
                        foundData[iteration].AddRange(testcase.Skip(found).Take(remaining));
                    }
                }

                if (foundData[iteration].Count == numberOfParameters)
                {
                    yield return foundData[iteration].ToArray();
                }
                else
                {
                    throw new InvalidOperationException(
                          string.Format(
                              CultureInfo.CurrentCulture,
                              "Expected {0} parameters, got {1} parameters",
                              numberOfParameters, foundData[iteration].Count
                              )
                          );
                }
            } while (++iteration < numberOfIterations);
        }
    }
}
