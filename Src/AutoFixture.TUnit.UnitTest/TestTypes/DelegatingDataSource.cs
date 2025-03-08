using System;
using System.Collections.Generic;
using AutoFixture.TUnit.Internal;

namespace AutoFixture.TUnit.UnitTest.TestTypes
{
    public class DelegatingDataSource : DataSource
    {
        public IEnumerable<object[]> TestData { get; set; } = Array.Empty<object[]>();

        protected override IEnumerable<object[]> GenerateDataSources()
        {
            return this.TestData;
        }
    }
}
