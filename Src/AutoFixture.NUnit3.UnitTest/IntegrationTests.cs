using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test, AutoMoqData]
        public void CanInjectDataTypes(DateTime anyTime, CultureInfo cultureInfo)
        {
            Assert.That(anyTime, Is.Not.Null);
            Assert.That(cultureInfo, Is.Not.Null);
        }

        [Theory, AutoMoqData]
        public void FrozenAttributeKeepsSingletons([Frozen] DateTime anytime, [Frozen] DateTime anyOtherTime)
        {
            Assert.That(anytime, Is.EqualTo(anyOtherTime));
        }

        [Theory, AutoMoqData]
        public void PutInConstructorInjection(string anyMessage, [Frozen] IDependencyStub dependencyStub, ContainerStub containerStub)
        {
            Mock.Get(dependencyStub).Setup(i => i.DoSomething(anyMessage)).Returns(anyMessage);

            Assert.That(containerStub.CallDependencyWith(anyMessage), Is.EqualTo(anyMessage));
        }

        [Theory, AutoMoqData]
        public void WithCollection(IList<string> anyStrings)
        {
            //sanity
            Assume.That(Enumerable.Empty<string>(), Is.Empty);
            Assume.That(String.Empty, Is.Empty);

            Assert.That(anyStrings, Is.Not.Empty);

            foreach (var s in anyStrings)
            {
                Assert.That(s, Is.Not.Empty);
            }
        }
    }

    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(new ParameterValueProvider(new AutoFixtureDataProvider(new AutoMoqCustomization())))
        {
        }
    }

    public class ContainerStub
    {
        private readonly IDependencyStub _dependencyStub;

        public ContainerStub(IDependencyStub dependencyStub)
        {
            this._dependencyStub = dependencyStub;
        }

        public string CallDependencyWith(string anything)
        {
            return this._dependencyStub.DoSomething(anything);
        }
    }

    public interface IDependencyStub
    {
        string DoSomething(string input);
    }
}
