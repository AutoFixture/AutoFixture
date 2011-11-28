using System;
using System.Linq;
using System.Reflection;
using FakeItEasy;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoFakeItEasy.UnitTest
{
    public class FakeItEasyMethodTest
    {
        [Fact]
        public void SutImplementsISpecimenBuilder()
        {
            // Exercise system
            var sut = new FakeItEasyMethod(typeof(FakeItEasyMethod), Enumerable.Empty<ParameterInfo>().ToArray());
            // Verify outcome
            Assert.IsAssignableFrom<IMethod>(sut);
            // Teardown
        }

        [Fact]
        public void ConstructorWithNullConstructorMethodThrows()
        {
            // Exercise system
            Assert.Throws<ArgumentNullException>(() =>
                new FakeItEasyMethod(null, Enumerable.Empty<ParameterInfo>().ToArray()));
        }

        [Fact]
        public void ConstructorWithNullParameterInfoArray()
        {
            // Exercise system
            Assert.Throws<ArgumentNullException>(() =>
                new FakeItEasyMethod(typeof(FakeItEasyMethod), null));
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(AbstractType))]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(IComparable<object>))]
        [InlineData(typeof(IComparable<string>))]
        [InlineData(typeof(IComparable<int>))]
        public void TargetTypeIsCorrect(Type t)
        {
            // Fixture setup
            var sut = new FakeItEasyMethod(t, new ParameterInfo[0]);
            // Exercise system
            Type result = sut.TargetType;
            // Verify outcome
            Assert.Equal(t, result);
            // Teardown
        }

        [Fact]
        public void ParametersIsCorrect()
        {
            // Fixture setup
            var parameters = Enumerable.Empty<ParameterInfo>();
            var sut = new FakeItEasyMethod(typeof(object), parameters.ToArray());
            // Exercise system
            var result = sut.Parameters;
            // Verify outcome
            Assert.Equal(parameters, result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(IInterface))]
        [InlineData(typeof(AbstractType))]
        public void InvokeReturnsCorrectResult(Type type)
        {
            // Fixture setup
            var sut = new FakeItEasyMethod(type, new ParameterInfo[] { });
            var expectedFake = typeof(Fake<>).MakeGenericType(type);
            // Exercise system
            var result = sut.Invoke(Enumerable.Empty<object>());
            // Verify outcome
            Assert.IsAssignableFrom(expectedFake, result);
            // Teardown
        }

        [Fact]
        public void InvokeOnTypeWithNonDefaultConstructorReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new FakeItEasyMethod(
                typeof(AbstractTypeWithNonDefaultConstructor<int>),
                new ParameterInfo[] { }
                );
            // Exercise system
            var result = sut.Invoke(new object[] { 1 });
            // Verify outcome
            var fake = Assert.IsAssignableFrom<Fake<AbstractTypeWithNonDefaultConstructor<int>>>(result);
            Assert.Equal(1, fake.FakedObject.Property);
            // Teardown
        }
    }
}
