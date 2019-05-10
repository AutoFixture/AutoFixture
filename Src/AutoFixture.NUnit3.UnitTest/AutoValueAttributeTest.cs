using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TestTypeFoundation;

namespace AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class AutoValueAttributeTest
    {
        [Test]
        [InlineAutoData(typeof(int))]
        [InlineAutoData(typeof(string))]
        [InlineAutoData(typeof(DoubleParameterType<int, string>))]
        public void AutoValueAttribute_generates_a_model(Type typeToGenerate, AutoValueAttribute sut)
        {
            var mockParameter = new Mock<IParameterInfo>();

            mockParameter.SetupGet(p => p.ParameterType).Returns(typeToGenerate);

            var values = sut.GetData(mockParameter.Object);

            Assert.That(values, Has.Exactly(1).InstanceOf(typeToGenerate));
        }
    }
}
