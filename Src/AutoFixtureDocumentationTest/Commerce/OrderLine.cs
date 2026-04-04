namespace AutoFixtureDocumentationTest.Commerce;

public class OrderLine
{
    public OrderLine(Product product)
    {
        Product = product;
        Quantity = 1;
    }

    public Product Product { get; private set; }

    public uint Quantity { get; set; }
}