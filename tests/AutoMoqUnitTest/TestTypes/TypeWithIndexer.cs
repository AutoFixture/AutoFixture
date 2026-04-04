namespace AutoFixture.AutoMoq.UnitTest.TestTypes;

public class TypeWithIndexer
{
    private readonly int[] _array = new[] { -99, -99, -99 };

    public int this[int index]
    {
        get { return _array[index]; }
        set { _array[index] = value; }
    }
}