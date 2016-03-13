using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    [TestFixture(Description="This test suite is not exhaustive")]
    public class FrozenAttributeIntegrationTest
    {
        [Theory, AutoData]
        public void FrozenWorksCorrectly(
            [Frozen]int anyInteger, 
            [Frozen]int anyOtherInteger,
            [Frozen] DateTime anyDateTime,
            [Frozen] DateTime anyOtherDateTime,
            [Frozen] CultureInfo anyCultureInfo,
            [Frozen] CultureInfo anyOtherCultureInfo)
        {
            Assert.That(anyInteger, Is.EqualTo(anyOtherInteger));
            Assert.That(anyDateTime, Is.EqualTo(anyOtherDateTime));
            Assert.That(anyCultureInfo, Is.EqualTo(anyOtherCultureInfo));
        }

        [Theory, AutoData]
        public void CanUseFrozenWithPropertyNameMatching(
            [Frozen(Matching.PropertyName)] int DummyProperty,
            DummyA dummyA,
            DummyB dummyB,
            DummyC dummyC)
        {
            Assert.That(dummyB.DummyProperty, Is.EqualTo(dummyA.DummyProperty));
            Assert.That(dummyC.DummyProperty, Is.EqualTo(dummyA.DummyProperty));
        }

        public class DummyA
        {
            public int DummyProperty { get; set; }
        }

        public class DummyB
        {
            public int DummyProperty { get; set; }
        }

        public class DummyC
        {
            public int DummyProperty { get; set; }
        }
    }
}
