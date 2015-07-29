using System.Globalization;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class InvariantCultureGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            var sut = new InvariantCultureGenerator();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullRequestWillReturnNoSpecimen()
        {
            var sut = new InvariantCultureGenerator();

            var actual = sut.Create(null, new DelegatingSpecimenContext());
            
            Assert.Equal(new NoSpecimen(), actual);
        }

        [Fact]
        public void CreateWithNullContextDoesNotThrow()
        {
            var sut = new InvariantCultureGenerator();
            sut.Create(new object(), null);
        }

        [Fact]
        public void CreateWithNonTypeRequestWillReturnNoSpecimen()
        {
            var sut = new InvariantCultureGenerator();
            var actual = sut.Create(new object(), new DelegatingSpecimenContext());

            Assert.Equal(new NoSpecimen(), actual);
        }

        [Fact]
        public void CreateWithNonCultureInfoTypeWillReturnNoSpecimen()
        {
            var sut = new InvariantCultureGenerator();
            var actual = sut.Create(typeof (object), new DelegatingSpecimenContext());

            Assert.Equal(new NoSpecimen(), actual);
        }

        [Fact]
        public void CreateWithCultureInfoRequestTypeReturnsInvariantCulture()
        {
            var sut = new InvariantCultureGenerator();
            var actual = sut.Create(typeof (CultureInfo), new DelegatingSpecimenContext());

            Assert.Equal(CultureInfo.InvariantCulture, actual);
        }
    }
}
