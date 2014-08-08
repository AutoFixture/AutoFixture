using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class MockSetupTest
    {
        [Fact]
        public void ConstructorThrowsWhenBuilderIsNull()
        {
            // Fixture setup
            var initializers = Enumerable.Empty<IMockInitializer>();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new MockSetup(null, initializers));
        }

        [Fact]
        public void ConstructorThrowsWhenInitializersAreNull()
        {
            // Fixture setup
            var builder = new Mock<ISpecimenBuilder>().Object;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new MockSetup(builder, null));
        }

        [Fact]
        public void TriggersInitializersWhenUnderlyingBuilderHandlesRequest()
        {
            // Fixture setup
            var request = new object();
            var mock = new Mock<object>();
            var context = new Mock<ISpecimenContext>();
            var builder = new Mock<ISpecimenBuilder>();
            var initializer1 = new Mock<IMockInitializer>();
            var initializer2 = new Mock<IMockInitializer>();

            builder.Setup(b => b.Create(request, context.Object))
                   .Returns(mock);

            var sut = new MockSetup(builder.Object, initializer1.Object, initializer2.Object);
            // Exercise system
            sut.Create(request, context.Object);
            // Verify outcome
            initializer1.Verify(i => i.Setup(mock, context.Object), Times.Once());
            initializer2.Verify(i => i.Setup(mock, context.Object), Times.Once());
            // Teardown
        }

        [Fact]
        public void DoesNotTriggerInitializersWhenUnderlyingBuilderCantHandleRequest()
        {
            // Fixture setup
            var request = new object();
            var noSpecimen = new NoSpecimen();
            var context = new Mock<ISpecimenContext>();
            var builder = new Mock<ISpecimenBuilder>();
            var initializer1 = new Mock<IMockInitializer>();
            var initializer2 = new Mock<IMockInitializer>();

            builder.Setup(b => b.Create(request, context.Object))
                   .Returns(noSpecimen);

            var sut = new MockSetup(builder.Object, initializer1.Object, initializer2.Object);
            // Exercise system
            sut.Create(request, context.Object);
            // Verify outcome
            initializer1.Verify(i => i.Setup(It.IsAny<Mock>(), context.Object), Times.Never());
            initializer2.Verify(i => i.Setup(It.IsAny<Mock>(), context.Object), Times.Never());
            // Teardown
        }

        [Fact]
        public void ReturnsNoSpecimenCreatedByUnderlyingBuilder()
        {
            // Fixture setup
            var request = new object();
            var noSpecimen = new NoSpecimen();
            var context = new Mock<ISpecimenContext>();
            var builder = new Mock<ISpecimenBuilder>();
            builder.Setup(b => b.Create(request, context.Object))
                   .Returns(noSpecimen);

            var sut = new MockSetup(builder.Object, new IMockInitializer[] {});
            // Exercise system
            var result = sut.Create(request, context.Object);
            // Verify outcome
            Assert.Same(noSpecimen, result);
            // Teardown
        }

        [Fact]
        public void ReturnsMockCreatedByUnderlyingBuilder()
        {
            // Fixture setup
            var request = new object();
            var mock = new Mock<object>();
            var context = new Mock<ISpecimenContext>();
            var builder = new Mock<ISpecimenBuilder>();
            builder.Setup(b => b.Create(request, context.Object))
                   .Returns(mock);

            var sut = new MockSetup(builder.Object, new IMockInitializer[] { });
            // Exercise system
            var result = sut.Create(request, context.Object);
            // Verify outcome
            Assert.Same(mock, result);
            // Teardown
        }
    }
}
