using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture.TUnit.UnitTest.TestTypes;

namespace AutoFixture.TUnit.UnitTest
{
    public class CompositeDataAttributeTest
    {
        [Test]
        public async Task SutIsDataAttribute()
        {
            // Arrange & Act
            var sut = new CompositeDataAttribute();

            // Assert
            await Assert.That(sut).IsAssignableFrom<NonTypedDataSourceGeneratorAttribute>();
        }

        [Test]
        public void InitializeWithNullArrayThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(
                () => new CompositeDataAttribute(null));
        }

        [Test]
        public async Task AttributesIsCorrectWhenInitializedWithArray()
        {
            // Arrange
            var a = () => { };
            var method = a.GetMethodInfo();

            var attributes = new NonTypedDataSourceGeneratorAttribute[]
            {
                new FakeDataAttribute(method, []),
                new FakeDataAttribute(method, []),
                new FakeDataAttribute(method, [])
            };

            var sut = new CompositeDataAttribute(attributes);
            // Act
            IEnumerable<NonTypedDataSourceGeneratorAttribute> result = sut.Attributes;
            // Assert
            await Assert.That(result).IsEquivalentTo(attributes);
        }

        [Test]
        public void InitializeWithNullEnumerableThrows()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(
                () => new CompositeDataAttribute((IReadOnlyCollection<NonTypedDataSourceGeneratorAttribute>)null));
        }

        [Test]
        public async Task AttributesIsCorrectWhenInitializedWithEnumerable()
        {
            // Arrange
            var a = () => { };
            var method = a.GetMethodInfo();

            var attributes = new NonTypedDataSourceGeneratorAttribute[]
            {
                new FakeDataAttribute(method, []),
                new FakeDataAttribute(method, []),
                new FakeDataAttribute(method, [])
            };

            var sut = new CompositeDataAttribute(attributes);
            // Act
            var result = sut.Attributes;
            // Assert
            await Assert.That(result).IsEquivalentTo(attributes);
        }

        [Test]
        public void GetDataWithNullMethodThrows()
        {
            // Arrange
            var sut = new CompositeDataAttribute();
            // Act & assert
            Assert.Throws<ArgumentNullException>(
                () => sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(null, null)));
        }

        [Test]
        public async Task GetDataOnMethodWithNoParametersReturnsNoTheory()
        {
            // Arrange
            Action a = () => { };
            var method = a.GetMethodInfo();

            var sut = new CompositeDataAttribute(
               new FakeDataAttribute(method, []),
               new FakeDataAttribute(method, []),
               new FakeDataAttribute(method, []));

            // Act
            var result = sut.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(method.DeclaringType, method.Name))
                .Select(x => x());

            // Assert
            await Assert.That(result).All().Satisfy(row => row.IsEmpty());
        }
    }
}