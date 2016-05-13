namespace Ploeh.AutoFixtureUnitTest
{
    using System;
    using System.Linq.Expressions;
    using AutoFixture;
    using AutoFixture.Kernel;
    using Kernel;
    using Xunit;
    using Xunit.Extensions;

    public class LambdaExpressionGeneratorTest
    {
        [Fact]
        public void SutIsISpecimenBuilder()
        {
            var sut = new LambdaExpressionGenerator();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNonTypeRequestReturnsNoSpecimen()
        {
            var nonTypeRequest = new object();
            var dummyContainer = new DelegatingSpecimenContext();
            var sut = new LambdaExpressionGenerator();

            var result = sut.Create(nonTypeRequest, dummyContainer);

            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void CreateWithNonLambdaExpressionTypeRequestReturnsNoSpecimen()
        {
            var nonExpressionRequest = typeof(object);
            var dummyContainer = new DelegatingSpecimenContext();
            var sut = new LambdaExpressionGenerator();

            var result = sut.Create(nonExpressionRequest, dummyContainer);

            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            var expressionRequest = typeof(Expression<Func<object>>);
            var sut = new LambdaExpressionGenerator();
            Assert.Throws<ArgumentNullException>(() => sut.Create(expressionRequest, null));
        }

        [Fact]
        public void CreateWithLambdaExpressionTypeRequestReturnsCorrectResult()
        {
            var expressionRequest = typeof(Expression<Func<object>>);
            var dummyContainer = new DelegatingSpecimenContext();
            var sut = new LambdaExpressionGenerator();

            var result = sut.Create(expressionRequest, dummyContainer);

            Assert.IsType<Expression<Func<object>>>(result);
        }

        [Theory]
        [InlineData(typeof(Expression<Action>))]
        [InlineData(typeof(Expression<Action<object>>))]
        [InlineData(typeof(Expression<Action<bool, int>>))]
        [InlineData(typeof(Expression<Func<object>>))]
        [InlineData(typeof(Expression<Func<object, object>>))]
        [InlineData(typeof(Expression<Func<bool, int>>))]
        [InlineData(typeof(Expression<Func<bool, int, string>>))]
        public void CreateWithExpressionRequestReturnsCorrectResult(Type expected)
        {
            var expressionRequest = expected;
            var dummyContainer = new DelegatingSpecimenContext();
            var sut = new LambdaExpressionGenerator();

            var result = sut.Create(expressionRequest, dummyContainer);

            Assert.IsType(expected, result);
        }
    }
}