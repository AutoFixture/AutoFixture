namespace Ploeh.AutoFixtureDocumentationTest.Commerce
{
    public class OrderLine
    {
        public OrderLine(Product product)
        {
            this.Product = product;
            this.Quantity = 1;
        }

        public Product Product { get; private set; }

        public uint Quantity { get; set; }
    }
}
