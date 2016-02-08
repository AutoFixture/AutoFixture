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
    public class AutoDataAttributeIntegrationTests
    {
        [Test, AutoData]
        public void Can_generate_DateTime_and_CultureInfo(DateTime anyTime, CultureInfo cultureInfo)
        {
            Assert.That(anyTime, Is.Not.Null);
            Assert.That(cultureInfo, Is.Not.Null);
        }

        [Theory, AutoData]
        public void FrozenAttribute_keeps_Singletons([Frozen] DateTime anytime, [Frozen] DateTime anyOtherTime)
        {
            Assert.That(anytime, Is.EqualTo(anyOtherTime));
        }

        [Theory, AutoData]
        public void Can_generate_collection(IList<string> anyStrings)
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

        [Theory, AutoMoqData]
        public void Can_be_inherited_using_customization(string anyMessage, [Frozen] IDependencyStub dependencyStub, ContainerStub containerStub)
        {
            Mock.Get(dependencyStub).Setup(i => i.DoSomething(anyMessage)).Returns(anyMessage);

            Assert.That(containerStub.CallDependencyWith(anyMessage), Is.EqualTo(anyMessage));
        }
    }

    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(new Fixture().Customize(new AutoMoqCustomization()))
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
