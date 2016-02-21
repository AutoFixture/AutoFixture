using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class AutoDataAttributeIntegrationTest
    {
        [Theory, AutoData]
        public void CanGenerateParameters(int anyInteger, double anyDouble, DateTime anyDateTime,
            CultureInfo anyCultureInfo)
        {
            // If parameters fail to receive values, 
            // then nunit runner will give error "No arguments were provided"
            // and never reach this point

            Assert.IsNotNull(anyCultureInfo);
        }
    }
}
