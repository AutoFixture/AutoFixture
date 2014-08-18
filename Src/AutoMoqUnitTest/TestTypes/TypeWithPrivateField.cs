using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public class TypeWithPrivateField
    {
        private string field = "";

        public string GetPrivateField()
        {
            return field;
        }
    }
}
