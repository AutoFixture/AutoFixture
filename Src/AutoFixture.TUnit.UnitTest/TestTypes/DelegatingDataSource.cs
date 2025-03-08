using System;
using System.Collections.Generic;
using AutoFixture.TUnit.Internal;

namespace AutoFixture.TUnit.UnitTest.TestTypes;

public class DelegatingDataSource : AutoFixtureDataSourceAttribute, IDataSource
{
    public IEnumerable<object[]> TestData { get; set; } = Array.Empty<object[]>();

    public override IEnumerable<object[]> GetData(DataGeneratorMetadata dataGeneratorMetadata)
    {
        return TestData;
    }
}