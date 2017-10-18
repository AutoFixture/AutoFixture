namespace AutoFixture.AutoMoq.UnitTest.TestTypes
{
    public class TypeWithIndexer
    {
        private readonly int[] array = new[] {-99, -99, -99};

        public int this[int index]
        {
            get { return this.array[index]; }
            set { this.array[index] = value; }
        }
    }
}
