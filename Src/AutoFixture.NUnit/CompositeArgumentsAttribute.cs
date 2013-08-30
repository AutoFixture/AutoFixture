using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixture.NUnit
{
    /// <summary>
    /// An implementation of ArgumentsAttribute that composes other ArgumentsAttribute instances.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    [CLSCompliant(false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "This attribute is the root of a potential attribute hierarchy.")]
    public class CompositeArgumentsAttribute : ArgumentsAttribute
    {
        private readonly IEnumerable<ArgumentsAttribute> _attributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeArgumentsAttribute"/> class.
        /// </summary>
        /// <param name="attributes">The attributes representing a data source for a data testcase.
        /// </param>
        public CompositeArgumentsAttribute(IEnumerable<ArgumentsAttribute> attributes)
            : this(attributes.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeArgumentsAttribute"/> class.
        /// </summary>
        /// <param name="attributes">The attributes representing a data source for a data testcase.
        /// </param>
        public CompositeArgumentsAttribute(params ArgumentsAttribute[] attributes)
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
        public IEnumerable<ArgumentsAttribute> Attributes
        {
            get { return _attributes; }
        }

        /// <summary>
        /// Returns the composition of data to be used to test the testcase. Favors the data returned
        /// by DataAttributes in ascending order. Data already returned is ignored on next
        /// ArgumentsAttribute returned data.
        /// </summary>
        /// <param name="method">The method that is being tested.</param>
        /// <param name="parameterTypes">The types of the parameters for the test method.</param>
        /// <returns>
        /// Returns the composition of the testcase arguments.
        /// </returns>
        /// <remarks>
        /// The number of test cases is set from the first ArgumentsAttribute testcase length.
        /// </remarks>
        public override IEnumerable<object[]> GetArguments(MethodInfo method, Type[] parameterTypes)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            if (parameterTypes == null)
            {
                throw new ArgumentNullException("parameterTypes");
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
                    var attributeData = attribute.GetArguments(method, parameterTypes).ToArray();

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
