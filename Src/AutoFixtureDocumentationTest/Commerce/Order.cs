using System.Collections.Generic;

namespace Ploeh.AutoFixtureDocumentationTest.Commerce
{
    public class Order
    {
        public Order(uint id)
        {
            this.Id = id;
            this.OrderLines = new List<OrderLine>();
        }

        public Address BillingAddress { get; set; }

        public uint Id { get; private set; }

        public IList<OrderLine> OrderLines { get; private set; }

        public Address ShippingAddress { get; set; }
    }
}
