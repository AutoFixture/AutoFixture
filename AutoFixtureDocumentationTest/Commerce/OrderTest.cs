using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureDocumentationTest.Commerce
{
    [TestClass]
    public class OrderTest
    {
        public OrderTest()
        {
        }

        [TestMethod]
        public void BuildOrderWithShippingAddressInDenmark()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var order = fixture.Build<Order>()
                .With(o => o.ShippingAddress, 
                    fixture.Build<Address>()
                    .With(a => a.Country, "Denmark")
                    .CreateAnonymous())
                .CreateAnonymous();
            // Verify outcome
            Assert.AreEqual<string>("Denmark", order.ShippingAddress.Country, "ShippingAddress");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousOrderAndThenSetShippingAddressInDenmark()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var order = fixture.CreateAnonymous<Order>();
            order.ShippingAddress.Country = "Denmark";
            // Verify outcome
            Assert.AreEqual<string>("Denmark", order.ShippingAddress.Country, "ShippingAddress");
            // Teardown
        }

        [TestMethod]
        public void BuildOrderWithManyOrderLines()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var order = fixture.Build<Order>()
                .Do(o => fixture.AddManyTo(o.OrderLines))
                .CreateAnonymous();
            // Verify outcome
            Assert.AreEqual<int>(fixture.RepeatCount, order.OrderLines.Count, "OrderLines count");
            // Teardown
        }

        [TestMethod]
        public void CreateAnonymousOrderAndThenAddOrderLines()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var order = fixture.CreateAnonymous<Order>();
            fixture.AddManyTo(order.OrderLines);
            Assert.AreEqual<int>(fixture.RepeatCount, order.OrderLines.Count, "OrderLines count");
            // Teardown
        }
    }
}
