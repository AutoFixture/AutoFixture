using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class FrozenAttributeIntegrationTest
    {
        [Theory, AutoData]
        public void FrozenParameterValuesAreKeptIdentical(
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
    }
}
