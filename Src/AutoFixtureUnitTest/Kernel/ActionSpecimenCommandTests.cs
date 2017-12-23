using System;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public abstract class ActionSpecimenCommandTests<T>
    {
        [Fact]
        public void SutIsSpecimenCommand()
        {
            var sut = new ActionSpecimenCommand<T>(_ => { });
            Assert.IsAssignableFrom<ISpecimenCommand>(sut);
        }

        [Fact]
        public void ExecuteCorrectlyInvokesSingleAction()
        {
            // Arrange
            var specimen = this.CreateSpecimen();

            var verified = false;
            Action<T> mock = x => verified = specimen.Equals(x);

            var sut = new ActionSpecimenCommand<T>(mock);
            // Act
            var dummyContext = new DelegatingSpecimenContext();
            sut.Execute(specimen, dummyContext);
            // Assert
            Assert.True(verified, "Action not invoked with expected specimen.");
        }

        [Fact]
        public void ExecuteCorrectlyInvokeDoubleAction()
        {
            // Arrange
            var specimen = this.CreateSpecimen();
            var context = new DelegatingSpecimenContext();

            var verified = false;
            Action<T, ISpecimenContext> mock =
                (s, c) => verified = specimen.Equals(s) && c == context;

            var sut = new ActionSpecimenCommand<T>(mock);
            // Act
            sut.Execute(specimen, context);
            // Assert
            Assert.True(verified, "Action not invoked with expected specimen.");
        }

        public abstract T CreateSpecimen();
    }

    public class ActionSpecimenCommandTestsOfObject : ActionSpecimenCommandTests<object>
    {
        public override object CreateSpecimen()
        {
            return new object();
        }
    }

    public class ActionSpecimenCommandTestsOfString : ActionSpecimenCommandTests<string>
    {
        public override string CreateSpecimen()
        {
            return Guid.NewGuid().ToString();
        }
    }

    public class ActionSpecimenCommandTestsOfInt32 : ActionSpecimenCommandTests<int>
    {
        public override int CreateSpecimen()
        {
            return new Random().Next();
        }
    }

    public class ActionSpecimenCommandTestsOfVersion : ActionSpecimenCommandTests<Version>
    {
        public override Version CreateSpecimen()
        {
            var r = new Random();
            return new Version(r.Next(), r.Next());
        }
    }
}
