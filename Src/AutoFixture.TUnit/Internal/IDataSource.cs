using System;
using System.Collections.Generic;

namespace AutoFixture.TUnit.Internal
{
    /// <summary>
    /// Exposes the factory method for a sequence of test data.
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// Returns the test data provided by the source.
        /// </summary>
        /// <param name="dataGeneratorMetadata">The target method for which to provide the arguments.</param>
        /// <returns>Returns a sequence of argument collections.</returns>
        IEnumerable<Func<object[]>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata);
    }
}