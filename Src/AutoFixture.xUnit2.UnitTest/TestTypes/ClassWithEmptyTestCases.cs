﻿using System.Collections;
using System.Collections.Generic;

namespace AutoFixture.Xunit2.UnitTest.TestTypes
{
    public class ClassWithEmptyTestCases : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { };
            yield return new object[] { };
            yield return new object[] { };
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}