namespace Ploeh.AutoFixtureUnitTest
{
    using System;
    using System.Linq.Expressions;
    using AutoFixture;
    using AutoFixture.Kernel;
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
            var sut = new LambdaExpressionGenerator();

            var result = sut.Create(nonTypeRequest, null);

            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void CreateWithNonLambdaExpressionTypeRequestReturnsNoSpecimen()
        {
            var nonExpressionRequest = typeof(object);
            var sut = new LambdaExpressionGenerator();

            var result = sut.Create(nonExpressionRequest, null);

            Assert.IsType<NoSpecimen>(result);
        }

        [Fact]
        public void CreateWithLambdaExpressionTypeRequestReturnsCorrectResult()
        {
            var expressionRequest = typeof(Expression<Func<object>>);
            var sut = new LambdaExpressionGenerator();

            var result = sut.Create(expressionRequest, null);

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
            var sut = new LambdaExpressionGenerator();

            var result = sut.Create(expressionRequest, null);

            Assert.IsType(expected, result);
        }
    }
}