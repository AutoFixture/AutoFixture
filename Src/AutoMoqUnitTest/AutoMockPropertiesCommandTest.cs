using System;
using System.Reflection;
using AutoFixture.AutoMoq.UnitTest.TestTypes;
using AutoFixture.Kernel;
using Moq;
using Xunit;

namespace AutoFixture.AutoMoq.UnitTest
{
    public class AutoMockPropertiesCommandTest
    {
        [Theory]
        [ClassData(typeof (ValidNonMockSpecimens))]
        public void ExecuteDoesNotThrowsWhenSpecimenIsValidNonMockSpecimen(object validNonMockSpecimen)
        {
            // Fixture setup
            var context = new Mock<ISpecimenContext>().Object;
            var sut = new AutoMockPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(validNonMockSpecimen, context)));
        }

        [Fact]
        public void ExecuteThrowsWhenContextIsNull()
        {
            // Fixture setup
            var specimen = new Mock<object>();
            var sut = new AutoMockPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => sut.Execute(specimen, null));
        }

        [Fact]
        public void IgnoresNonMockSpecimens()
        {
            // Fixture setup
            var specimen = new object();
            var context = new Mock<ISpecimenContext>().Object;
            var sut = new AutoMockPropertiesCommand();
            // Exercise system and verify outcome
            Assert.Null(Record.Exception(() => sut.Execute(specimen, context)));
        }

        [Fact]
        public void PopulatesPropertiesUsingContext()
        {
            // Fixture setup
            var specimen = new Mock<IInterfaceWithProperty>();

            const string frozenString = "a string";
            var contextStub = new Mock<ISpecimenContext>();
            contextStub.Setup(ctx => ctx.Resolve(specimen.Object.GetType().GetProperty("Property"))).Returns(frozenString);

            var sut = new AutoMockPropertiesCommand();
            // Exercise system
            sut.Execute(specimen, contextStub.Object);
            // Verify outcome
            specimen.VerifySet(s => s.Property = frozenString, Times.Once());
        }

        [Fact]
        public void PopulatesFields()
        {
            // Fixture setup
            var specimen = new Mock<TypeWithPublicField>();

            const string frozenString = "a string";
            var contextStub = new Mock<ISpecimenContext>();
            contextStub.Setup(ctx => ctx.Resolve(specimen.Object.GetType().GetField("Field"))).Returns(frozenString);

            var sut = new AutoMockPropertiesCommand();
            // Exercise system
            sut.Execute(specimen, contextStub.Object);
            // Verify outcome
            Assert.Equal(frozenString, specimen.Object.Field);
        }

        [Theory]
        [InlineData("__interceptors")]
        [InlineData("__target")]
        public void IgnoresProxyMembers(string proxyFieldName)
        {
            // Fixture setup
            var specimen = new Mock<IInterfaceWithoutMembers>();
            var proxyField = specimen.Object.GetType().GetField(proxyFieldName);
            var initialProxyFieldValue = proxyField?.GetValue(specimen.Object);

            var contextStub = new Mock<ISpecimenContext>();
            contextStub.Setup(ctx => ctx.Resolve(It.IsAny<FieldInfo>()))
                .Returns((FieldInfo fi) => fi.FieldType.IsArray ?
                    Array.CreateInstance(fi.FieldType.GetElementType(), 0) :
                    Activator.CreateInstance(fi.FieldType));

            var sut = new AutoMockPropertiesCommand();
            // Exercise system
            sut.Execute(specimen, contextStub.Object);
            // Verify outcome
            var finalProxyFieldValue = proxyField?.GetValue(specimen.Object);
            Assert.Same(initialProxyFieldValue, finalProxyFieldValue);
        }
    }
}
