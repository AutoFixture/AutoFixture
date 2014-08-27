namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes
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
