using System;
using System.Collections.Generic;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class TracingBuilderTest
    {
        [Fact]
        public void TestSpecificSutIsSut()
        {
            // Arrange
            // Act
            var sut = new DelegatingTracingBuilder();
            // Assert
            Assert.IsAssignableFrom<TracingBuilder>(sut);
        }

        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Arrange
            // Act
            var sut = new DelegatingTracingBuilder();
            // Assert
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void InitializeWithNullSpecimenBuilderWillThrow()
        {
            // Arrange
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                new DelegatingTracingBuilder(null));
        }

        [Fact]
        public void BuilderIsCorrect()
        {
            // Arrange
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new TracingBuilder(expectedBuilder);
            // Act
            ISpecimenBuilder result = sut.Builder;
            // Assert
            Assert.Equal(expectedBuilder, result);
        }

        [Fact]
        public void CreateWillPassThroughToDecoratedBuilder()
        {
            // Arrange
            object expectedSpecimen = Guid.NewGuid();
            var decoratedBuilder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r };
            var sut = new DelegatingTracingBuilder(decoratedBuilder);

            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            object result = sut.Create(expectedSpecimen, dummyContainer);

            // Assert
            Assert.Equal(expectedSpecimen, result);
        }

        [Fact]
        public void CreateWillCorrectlyRaiseSpecimenRequested()
        {
            // Arrange
            var verified = false;
            var request = new object();
            var sut = new DelegatingTracingBuilder();
            sut.SpecimenRequested += (sender, e) => verified = e.Request == request && e.Depth == 1;
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(request, dummyContainer);
            // Assert
            Assert.True(verified, "Event raised");
        }

        [Fact]
        public void CreateWillCorrectlyRaiseSpecimenRequestedInCompositeRequest()
        {
            // Arrange
            object requestedObject = "The request";
            object subRequest = "Some sub request";

            var spy = new List<RequestTraceEventArgs>();
            var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == requestedObject ? c.Resolve(subRequest) : new NoSpecimen() };
            var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? new object() : new NoSpecimen() };
            var compBuilder = new CompositeSpecimenBuilder(builder2, builder3);

            var sut = new DelegatingTracingBuilder(compBuilder);
            sut.SpecimenRequested += (sender, e) => spy.Add(e);

            var container = new SpecimenContext(sut);
            // Act
            sut.Create(requestedObject, container);
            // Assert
            Assert.Equal(2, spy.Count);
            Assert.Equal(subRequest, spy[1].Request);
            Assert.Equal(2, spy[1].Depth);
        }

        [Fact]
        public void CreateWillTrackCompositeRequests()
        {
            // Arrange
            object requestedObject = "The request";
            object subRequest = "Some sub request";
            var spy = new List<object>();
            var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == requestedObject ? c.Resolve(subRequest) : new NoSpecimen() };
            var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? new object() : new NoSpecimen() };
            var compBuilder = new CompositeSpecimenBuilder(builder2, builder3);

            var sut = new DelegatingTracingBuilder(compBuilder);
            sut.SpecimenRequested += (sender, e) => spy.Add(e.Request);
            var container = new SpecimenContext(sut);
            // Act
            sut.Create(requestedObject, container);

            // Assert
            Assert.Equal(2, spy.Count);
            Assert.Equal(subRequest, spy[1]);
        }

        [Fact]
        public void CreateWillCorrectlyRaiseSpecimenCreated()
        {
            // Arrange
            var request = new object();
            var specimen = new object();

            var verified = false;
            var sut = new DelegatingTracingBuilder(new DelegatingSpecimenBuilder { OnCreate = (r, c) => specimen });
            sut.SpecimenCreated += (sender, e) => verified = e.Request == request && e.Specimen == specimen && e.Depth == 1;
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(request, dummyContainer);
            // Assert
            Assert.True(verified, "Event raised");
        }

        [Fact]
        public void CreateWillTrackCreatedSpecimen()
        {
            // Arrange
            object tracked = null;
            object createdSpecimen = Guid.NewGuid();
            var container = new DelegatingSpecimenContext { OnResolve = r => createdSpecimen };
            var sut = new DelegatingTracingBuilder();

            // Act
            object res = sut.Create(new object(), container);

            // Assert
            Assert.Equal(res, tracked);
        }

        [Fact]
        public void CreateWillCorrectRaiseSpecimenCreatedInCompositeRequest()
        {
            // Arrange
            object request = "The request";
            object subRequest = "Some sub request";
            var createdSpecimen = new object();

            var spy = new List<SpecimenCreatedEventArgs>();
            var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == request ? c.Resolve(subRequest) : new NoSpecimen() };
            var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? createdSpecimen : new NoSpecimen() };
            var compBuilder = new CompositeSpecimenBuilder(builder2, builder3);

            var sut = new DelegatingTracingBuilder(compBuilder);
            sut.SpecimenCreated += (sender, e) => spy.Add(e);

            var container = new SpecimenContext(sut);
            // Act
            sut.Create(request, container);
            // Assert
            Assert.Equal(2, spy.Count);
            Assert.Equal(createdSpecimen, spy[0].Specimen);
            Assert.Equal(2, spy[0].Depth);
            Assert.Equal(createdSpecimen, spy[1].Specimen);
            Assert.Equal(1, spy[1].Depth);
        }

        [Fact]
        public void CreateWillTrackCreatedSpecimensComposite()
        {
            // Arrange
            object requestedObject = "The request";
            object subRequest = "Some sub request";
            object createdSpecimen = Guid.NewGuid();
            var spy = new List<object>();
            var builder2 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == requestedObject ? c.Resolve(subRequest) : new NoSpecimen() };
            var builder3 = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == subRequest ? createdSpecimen : new NoSpecimen() };
            var compBuilder = new CompositeSpecimenBuilder(builder2, builder3);

            var sut = new DelegatingTracingBuilder(compBuilder);
            sut.SpecimenCreated += (sender, e) => spy.Add(e.Specimen);
            var container = new SpecimenContext(sut);
            // Act
            sut.Create(requestedObject, container);

            // Assert
            Assert.Equal(2, spy.Count);
            Assert.Equal(createdSpecimen, spy[0]);
            Assert.Equal(createdSpecimen, spy[1]);
        }

        [Fact]
        public void AssignNullFilterWillThrow()
        {
            // Arrange
            var sut = new DelegatingTracingBuilder();
            // Act & assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Filter = null);
        }

        [Fact]
        public void FilterIsProperWritableProperty()
        {
            // Arrange
            var sut = new DelegatingTracingBuilder();
            IRequestSpecification expectedFilter = new DelegatingRequestSpecification();
            // Act
            sut.Filter = expectedFilter;
            IRequestSpecification result = sut.Filter;
            // Assert
            Assert.Equal(expectedFilter, result);
        }

        [Fact]
        public void CreateWillNotRaiseSpecimenRequestForFilteredRequest()
        {
            // Arrange
            var eventRaised = false;

            var request = new object();
            var sut = new DelegatingTracingBuilder();
            sut.SpecimenRequested += (sender, e) => eventRaised = true;
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(request, dummyContainer);
            // Assert
            Assert.False(eventRaised, "Event raised");
        }

        [Fact]
        public void CreateWillNotTrackFilteredRequest()
        {
            // Arrange
            object tracked = null;
            object requestedObject = new object();
            var sut = new DelegatingTracingBuilder();
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(requestedObject, dummyContainer);
            // Assert
            Assert.Null(tracked);
        }

        [Fact]
        public void CreateWillNotRaiseSpecimenCreatedForFilteredRequest()
        {
            // Arrange
            var eventRaised = false;

            var request = new object();
            var sut = new DelegatingTracingBuilder();
            sut.SpecimenCreated += (sender, e) => eventRaised = true;
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(request, dummyContainer);
            // Assert
            Assert.False(eventRaised, "Event raised");
        }

        [Fact]
        public void CreateWillNotTrackFilteredSpecimen()
        {
            // Arrange
            object tracked = null;
            object requestedObject = new object();
            var decoratedBuilder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r };
            var sut = new DelegatingTracingBuilder(decoratedBuilder);
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => false };
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(requestedObject, dummyContainer);
            // Assert
            Assert.Null(tracked);
        }

        [Fact]
        public void CreateWillNotRaiseSpecimenRequestedForIgnoredType()
        {
            // Arrange
            var eventRaised = false;

            var request = Guid.NewGuid();
            var sut = new DelegatingTracingBuilder();
            sut.SpecimenRequested += (sender, e) => eventRaised = true;

            var ignoredTypes = new List<Type> { typeof(Guid) };
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => !ignoredTypes.Contains(r.GetType()) };
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(request, dummyContainer);
            // Assert
            Assert.False(eventRaised, "Event raised");
        }

        [Fact]
        public void IgnoredTypeWillNotTrackRequest()
        {
            // Arrange
            object tracked = null;
            object requestedObject = Guid.NewGuid();
            var sut = new DelegatingTracingBuilder();

            var ignoredTypes = new List<Type> { typeof(Guid) };
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => !ignoredTypes.Contains(r.GetType()) };

            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(requestedObject, dummyContainer);

            // Assert
            Assert.Null(tracked);
        }

        [Fact]
        public void DepthWillBeResetAfterDecoratedBuilderThrows()
        {
            // Arrange
            int createCallNumber = 0;
            int lastRequestDepth = 0;
            var firstRequest = Guid.NewGuid();
            var secondRequest = Guid.NewGuid();
            var dummyContainer = new DelegatingSpecimenContext();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) =>
{
createCallNumber++;
if (createCallNumber == 1) throw new PrivateException();
return c.Resolve(r);
}
            };
            var sut = new DelegatingTracingBuilder(builder);
            sut.SpecimenRequested += (sender, e) => lastRequestDepth = e.Depth;

            // Act & assert
            Assert.Throws<PrivateException>(() => sut.Create(firstRequest, dummyContainer));
            Assert.Equal(1, lastRequestDepth);

            sut.Create(secondRequest, dummyContainer);
            Assert.Equal(1, lastRequestDepth);
        }

        class PrivateException : Exception { }

        [Fact]
        public void CreateWillNotRaiseSpecimenCreatedForIgnoredType()
        {
            // Arrange
            var eventRaised = false;

            var request = Guid.NewGuid();
            var sut = new DelegatingTracingBuilder();
            sut.SpecimenCreated += (sender, e) => eventRaised = true;

            var ignoredTypes = new List<Type> { typeof(Guid) };
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => !ignoredTypes.Contains(r.GetType()) };
            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            sut.Create(request, dummyContainer);
            // Assert
            Assert.False(eventRaised, "Event raised");
        }

        [Fact]
        public void IgnoredTypeWillNotTrackCreatedSpecimen()
        {
            // Arrange
            object tracked = null;
            object requestedObject = Guid.NewGuid();

            var decoratedBuilder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r };
            var sut = new DelegatingTracingBuilder(decoratedBuilder);

            var ignoredTypes = new List<Type> { typeof(Guid) };
            sut.Filter = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => !ignoredTypes.Contains(r.GetType()) };

            // Act
            var dummyContainer = new DelegatingSpecimenContext();
            object res = sut.Create(requestedObject, dummyContainer);

            // Assert
            Assert.Null(tracked);
        }
    }
}