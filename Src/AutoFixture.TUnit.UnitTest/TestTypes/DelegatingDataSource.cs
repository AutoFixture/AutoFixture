using System;
using System.Collections.Generic;

namespace AutoFixture.TUnit.UnitTest.TestTypes
{
    public class DelegatingDataSource : DataSource
    {
        public IEnumerable<object[]> TestData { get; set; } = Array.Empty<object[]>();

        protected override IEnumerable<object[]> GetData()
        {
            return this.TestData;
        }
    }
}
