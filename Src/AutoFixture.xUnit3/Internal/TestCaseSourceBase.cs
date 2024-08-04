namespace AutoFixture.Xunit3.Internal
{
    /// <summary>
    ///     The base class for test case sources.
    /// </summary>
    [SuppressMessage("Design", "CA1010:Generic interface should also be implemented",
        Justification = "The type is not a collection.")]
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix",
        Justification = "The type is not a collection.")]
    internal abstract class TestCaseSourceBase : ITestCaseSource, IEnumerable
    {
        /// <summary>
        ///     Gets the enumerator for the test cases provided by the source.
        /// </summary>
        /// <returns>Returns an enumerator of the test case sequence.</returns>
        /// <exception cref="InvalidCastException">Thrown when value provided by field is not a valid test case source.</exception>
        public abstract IEnumerator GetEnumerator();

        /// <summary>
        ///     Returns the test cases provided by the source.
        /// </summary>
        /// <param name="method">The target method for which to provide the arguments.</param>
        /// <returns>Returns a sequence of argument collections.</returns>
        public IEnumerable<IEnumerable<object>> GetTestCases(MethodInfo method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return GetTestCasesEnumerable();

            IEnumerable<IEnumerable<object>> GetTestCasesEnumerable()
            {
                var parameters = method.GetParameters();
                if (parameters.Length == 0)
                {
                    // If the method has no parameters, a single test run is enough.
                    yield return Array.Empty<object>();
                    yield break;
                }

                var enumerator = this.GetEnumerator();
                if (enumerator is null)
                {
                    throw new InvalidOperationException($"No data could be found for {method.Name}.");
                }

                while (enumerator.MoveNext())
                {
                    var value = enumerator.Current;

                    if (parameters[0].ParameterType.GetTypeInfo().IsInstanceOfType(value))
                    {
                        yield return new[] { value };
                        continue;
                    }

                    if (value is IEnumerable values)
                    {
                        yield return values.OfType<object>().Take(parameters.Length);
                        continue;
                    }

                    yield return Array.Empty<object>();
                }
            }
        }
    }
}