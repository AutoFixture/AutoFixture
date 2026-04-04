namespace AutoFixtureDocumentationTest.Commerce;

public class Product
{
    public Product(uint id)
    {
        Id = id;
    }

    public uint Id { get; private set; }

    public decimal Price { get; set; }

    public double Weight { get; set; }
}