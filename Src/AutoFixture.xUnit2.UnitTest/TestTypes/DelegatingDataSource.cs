using System;
using System.Collections.Generic;
using AutoFixture.Xunit2.Internal;

namespace AutoFixture.Xunit2.UnitTest.TestTypes
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
