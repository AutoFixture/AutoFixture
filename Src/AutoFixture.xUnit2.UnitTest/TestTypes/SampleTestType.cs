using System.Collections.Generic;
using TestTypeFoundation;

namespace AutoFixture.Xunit2.UnitTest.TestTypes
{
    public class SampleTestType
    {
        public void TestMethodWithoutParameters()
        {
        }

        public void TestMethodWithSingleParameter(string value)
        {
        }

        public void TestMethodWithMultipleParameters(string a, int b, double c)
        {
        }

        public void TestMethodWithReferenceTypeParameter(string a, int b, PropertyHolder<string> c)
        {
        }

        public void TestMethodWithRecordTypeParameter(string a, int b, RecordType<string> c)
        {
        }

        public void TestMethodWithCollectionParameter(string a, int b, IEnumerable<int> c)
        {
        }

        public void TestMethodWithCustomizedParameter([Frozen] string a, int b, PropertyHolder<string> c)
        {
        }

        public void TestMethodWithMultipleCustomizations([Frozen] string a, [Frozen] int b, [Greedy][Frozen] PropertyHolder<string> c)
        {
        }
    }
}
