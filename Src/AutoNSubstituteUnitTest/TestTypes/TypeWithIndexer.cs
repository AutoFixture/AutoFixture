namespace AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public abstract class TypeWithIndexer
    {
        private readonly int[] array = new[] {-99, -99, -99};

        public int this[int index]
        {
            get { return this.array[index]; }
            set { this.array[index] = value; }
        }
    }
}
