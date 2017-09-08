using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    [TestClass]
    public class CompositeSpecimenBuilderTest
    {
        [TestMethod]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeSpecimenBuilder();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ISpecimenBuilder));
            // Teardown
        }

        [TestMethod]
        public void BuildersWillNotBeNullWhenSutIsCreatedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new CompositeSpecimenBuilder();
            // Exercise system
            IList<ISpecimenBuilder> result = sut.Builders;
            // Verify outcome
            Assert.IsNotNull(result, "Builders");
            // Teardown
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateWithNullEnumerableWillThrow()
        {
            // Fixture setup
            IEnumerable<ISpecimenBuilder> nullEnumerable = null;
            // Exercise system
            new CompositeSpecimenBuilder(nullEnumerable);
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void BuildersWillMatchListParameter()
        {
            // Fixture setup
            var expectedBuilders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            }.AsEnumerable();
            var sut = new CompositeSpecimenBuilder(expectedBuilders);
            // Exercise system
            var result = sut.Builders;
            // Verify outcome
            Assert.IsTrue(expectedBuilders.SequenceEqual(result), "Builders");
            // Teardown
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateWithNullArrayWillThrow()
        {
            // Fixture setup
            ISpecimenBuilder[] nullArray = null;
            // Exercise system
            new CompositeSpecimenBuilder(nullArray);
            // Verify outcome (expected exception)
            // Teardown
        }

        [TestMethod]
        public void BuildersWillMatchParamsArray()
        {
            // Fixture setup
            var expectedBuilders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var sut = new CompositeSpecimenBuilder(expectedBuilders[0], expectedBuilders[1], expectedBuilders[2]);
            // Exercise system
            var result = sut.Builders;
            // Verify outcome
            Assert.IsTrue(expectedBuilders.SequenceEqual(result), "Builders");
            // Teardown
        }

        [TestMethod]
        public void CreateWillReturnFirstNonNullResultFromBuilders()
        {
            // Fixture setup
            var expectedResult = new object();
            var builders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => null },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => new object() }
            };
            var sut = new CompositeSpecimenBuilder(builders);
            // Exercise system
            var anonymousRequest = new object();
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(anonymousRequest, dummyContainer);
            // Verify outcome
            Assert.AreEqual(expectedResult, result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWillReturnNullIfAllBuildersReturnNull()
        {
            // Fixture setup
            var builders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => null },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => null },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => null }
            };
            var sut = new CompositeSpecimenBuilder(builders);
            // Exercise system
            var anonymousRequest = new object();
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(anonymousRequest, dummyContainer);
            // Verify outcome
            Assert.IsNull(result, "Create");
            // Teardown
        }

        [TestMethod]
        public void CreateWillInvokeBuilderWithCorrectRequest()
        {
            // Fixture setup
            var expectedRequest = new object();

            var mockVerified = false;
            var builderMock = new DelegatingSpecimenBuilder();
            builderMock.OnCreate = (r, c) =>
                {
                    Assert.AreEqual(expectedRequest, r, "Create");
                    mockVerified = true;
                    return new object();
                };

            var sut = new CompositeSpecimenBuilder(builderMock);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            sut.Create(expectedRequest, dummyContainer);
            // Verify outcome
            Assert.IsTrue(mockVerified, "Mock verification");
            // Teardown
        }

        [TestMethod]
        public void CreateWillInvokeBuilderWithCorrectContainer()
        {
            // Fixture setup
            var expectedContainer = new DelegatingSpecimenContainer();

            var mockVerified = false;
            var builderMock = new DelegatingSpecimenBuilder();
            builderMock.OnCreate = (r, c) =>
                {
                    Assert.AreEqual(expectedContainer, c, "Create");
                    mockVerified = true;
                    return new object();
                };

            var sut = new CompositeSpecimenBuilder(builderMock);
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, expectedContainer);
            // Verify outcome
            Assert.IsTrue(mockVerified, "Mock verification");
            // Teardown
        }
    }
}
