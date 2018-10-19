using AutoFixture.AutoNSubstitute.CustomCallHandler;
using AutoFixture.Kernel;
using NSubstitute;
using NSubstitute.Core;
using Xunit;

namespace AutoFixture.AutoNSubstitute.UnitTest
{
    public partial class NSubstituteRegisterCallHandlerCommandTest
    {
        [Fact]
        public void ValidCallSpecificationFactoryIsPassedToHandler()
        {
            // Arrange
            var substitutionContext = Substitute.For<ISubstitutionContext>();
            var sut = new NSubstituteRegisterCallHandlerCommand(substitutionContext);

            var specimen = new object();
            var context = Substitute.For<ISpecimenContext>();
            var substituteState = Substitute.For<ISubstituteState>();

            var callRouter = new CallRouterStub();
            substitutionContext.GetCallRouterFor(specimen).Returns(callRouter);

            var callSpecFactory = Substitute.For<ICallSpecificationFactory>();
            substituteState.CallSpecificationFactory.Returns(callSpecFactory);

            sut.Execute(specimen, context);

            // Act
            var handler = (AutoFixtureValuesHandler)callRouter.RegisteredFactory.Invoke(substituteState);

            // Assert
            Assert.Equal(callSpecFactory, handler.CallSpecificationFactory);
        }
    }
}