using System;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit.UnitTest.TestCases
{
    public class CustomizeAttributeTests
    {
        [Test]
        public void TestableSutIsSut()
        {
            var sut = new DelegatingCustomizeAttribute();

            Assert.IsInstanceOf<CustomizeAttribute>(sut);
        }

        [Test]
        public void SutIsAttribute()
        {
            var sut = new DelegatingCustomizeAttribute();
            
            Assert.IsInstanceOf<Attribute>(sut);
        }
    }
}
