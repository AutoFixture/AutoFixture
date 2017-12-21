using AutoFixture;
using Xunit;

namespace AutoFixtureDocumentationTest.Commerce
{
    public class OrderTest
    {
        public OrderTest()
        {
        }

        [Fact]
        public void BuildOrderWithShippingAddressInDenmark()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var order = fixture.Build<Order>()
                .With(o => o.ShippingAddress, 
                    fixture.Build<Address>()
                    .With(a => a.Country, "Denmark")
                    .Create())
                .Create();
            // Assert
            Assert.Equal("Denmark", order.ShippingAddress.Country);
        }

        [Fact]
        public void CreateAnonymousOrderAndThenSetShippingAddressInDenmark()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var order = fixture.Create<Order>();
            order.ShippingAddress.Country = "Denmark";
            // Assert
            Assert.Equal("Denmark", order.ShippingAddress.Country);
        }

        [Fact]
        public void BuildOrderWithManyOrderLines()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var order = fixture.Build<Order>()
                .Do(o => fixture.AddManyTo(o.OrderLines))
                .Create();
            // Assert
            Assert.Equal<int>(fixture.RepeatCount, order.OrderLines.Count);
        }

        [Fact]
        public void CreateAnonymousOrderAndThenAddOrderLines()
        {
            // Arrange
            var fixture = new Fixture();
            // Act
            var order = fixture.Create<Order>();
            fixture.AddManyTo(order.OrderLines);
            Assert.Equal<int>(fixture.RepeatCount, order.OrderLines.Count);
        }
    }
}
