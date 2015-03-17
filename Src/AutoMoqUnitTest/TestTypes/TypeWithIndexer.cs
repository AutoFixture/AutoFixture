using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public class TypeWithIndexer
    {
        private readonly int[] array = new[] {-99, -99, -99};

        public int this[int index]
        {
            get { return array[index]; }
            set { array[index] = value; }
        }
    }
}
