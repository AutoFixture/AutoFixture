using System.Diagnostics.CodeAnalysis;

namespace AutoFixture.TUnit.Internal
{
    /// <summary>
    /// The base class for test case sources.
    /// </summary>
    [SuppressMessage("Design", "CA1010:Generic interface should also be implemented",
        Justification = "The type is not a collection.")]
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix",
        Justification = "The type is not a collection.")]
    public abstract class DataSource : AutoFixtureDataSourceAttribute, IDataSource;
}