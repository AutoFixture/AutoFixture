namespace AutoFixture.Xunit3.Internal
{
    /// <summary>
    ///     Exposes the factory method for a sequence of test cases.
    /// </summary>
    internal interface ITestCaseSource
    {
        /// <summary>
        ///     Returns the test cases provided by the source.
        /// </summary>
        /// <param name="method">The target method for which to provide the arguments.</param>
        /// <returns>Returns a sequence of argument collections.</returns>
        IEnumerable<IEnumerable<object>> GetTestCases(MethodInfo method);
    }
}