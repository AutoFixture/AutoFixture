﻿using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Commerce
{
    public class OrderTest
    {
        public OrderTest()
        {
        }

        [Fact]
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
            Assert.Equal<string>("Denmark", order.ShippingAddress.Country);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOrderAndThenSetShippingAddressInDenmark()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var order = fixture.Create<Order>();
            order.ShippingAddress.Country = "Denmark";
            // Verify outcome
            Assert.Equal<string>("Denmark", order.ShippingAddress.Country);
            // Teardown
        }

        [Fact]
        public void BuildOrderWithManyOrderLines()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var order = fixture.Build<Order>()
                .Do(o => fixture.AddManyTo(o.OrderLines))
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<int>(fixture.RepeatCount, order.OrderLines.Count);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOrderAndThenAddOrderLines()
        {
            // Fixture setup
            var fixture = new Fixture();
            // Exercise system
            var order = fixture.Create<Order>();
            fixture.AddManyTo(order.OrderLines);
            Assert.Equal<int>(fixture.RepeatCount, order.OrderLines.Count);
            // Teardown
        }
    }
}
