namespace AutoFixture.Xunit3.Internal
{
    /// <summary>
    ///     Encapsulates access to a field that provides test cases.
    /// </summary>
    [SuppressMessage("Design", "CA1010:Generic interface should also be implemented",
        Justification = "Type is not a collection.")]
    internal class FieldTestCaseSource : TestCaseSourceBase
    {
        /// <summary>
        ///     Creates an instance of type <see cref="FieldTestCaseSource" />.
        /// </summary>
        /// <param name="fieldInfo">The source field info.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="fieldInfo" /> is <see langword="null" />.</exception>
        public FieldTestCaseSource(FieldInfo fieldInfo)
        {
            this.FieldInfo = fieldInfo ?? throw new ArgumentNullException(nameof(fieldInfo));
        }

        /// <summary>
        ///     Gets the source field.
        /// </summary>
        public FieldInfo FieldInfo { get; }

        /// <summary>
        ///     Gets the enumerator for the test cases provided by the source member.
        /// </summary>
        /// <returns>Returns an enumerator of the test case sequence.</returns>
        /// <exception cref="InvalidCastException">Thrown when value provided by field is not a valid test case source.</exception>
        public override IEnumerator GetEnumerator()
        {
            var value = this.FieldInfo.GetValue(null);

            if (value is not IEnumerable enumerable)
            {
                throw new InvalidCastException("Member does not return an enumerable value.");
            }

            return enumerable.GetEnumerator();
        }
    }
}