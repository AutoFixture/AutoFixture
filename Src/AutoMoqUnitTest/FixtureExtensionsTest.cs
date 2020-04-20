using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Kernel;
using Moq;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class FixtureExtensionsTest
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(false, true)]
        public void AddAutoMoqCustomizesFixture(bool configureMembers, bool generateDelegates)
        {
            var fixture = Mock.Of<IFixture>();

            fixture.AddAutoMoq(configureMembers: configureMembers, generateDelegates: generateDelegates);

            Mock.Get(fixture).Verify(p => p.Customize(It.Is<AutoMoqCustomization>(c => c.ConfigureMembers == configureMembers && c.GenerateDelegates == generateDelegates)));
        }

        [Fact]
        public void AddAutoMoqCustomizesFixtureWithGivenRelay()
        {
            var fixture = Mock.Of<IFixture>();

            var relay = Mock.Of<ISpecimenBuilder>();

            fixture.AddAutoMoq(relay: relay);

            Mock.Get(fixture).Verify(p => p.Customize(It.Is<AutoMoqCustomization>(c => ReferenceEquals(relay, c.Relay))));
        }

        [Fact]
        public void AddAutoMoqReturnsSameFixture()
        {
            var fixture = Mock.Of<IFixture>();

            var result = fixture.AddAutoMoq();

            Assert.Same(fixture, result);
        }

        [Fact]
        public void AddAutoMoqThrowsIfFixtureIsNull()
        {
            IFixture fixture = null;

            Assert.Throws<ArgumentNullException>(() => fixture.AddAutoMoq());
        }
    }
}
