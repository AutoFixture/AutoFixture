namespace Ploeh.AutoFixtureDocumentationTest.Commerce
{
    public class Product
    {
        public Product(uint id)
        {
            this.Id = id;
        }

        public uint Id { get; private set; }

        public decimal Price { get; set; }

        public double Weight { get; set; }
    }
}
