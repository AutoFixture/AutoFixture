using System;
using System.Reflection;
using System.Globalization;
using System.Threading;
using Xunit.Sdk;

namespace Ploeh.AutoFixtureUnitTest
{
    public class UseCultureAttribute : BeforeAfterTestAttribute
    {
        [ThreadStatic]
        private static CultureInfo originalCulture;
        [ThreadStatic]
        private static CultureInfo originalUiCulture;

        private readonly CultureInfo culture;
        private readonly CultureInfo uiCulture;

        public UseCultureAttribute(string culture)
            : this(culture, culture)
        {
        }

        public UseCultureAttribute(string culture, string uiCulture)
        {
            this.culture = new CultureInfo(culture);
            this.uiCulture = new CultureInfo(uiCulture);
        }


        public override void Before(MethodInfo methodUnderTest)
        {
            originalCulture = Thread.CurrentThread.CurrentCulture;
            originalUiCulture = Thread.CurrentThread.CurrentUICulture;

            Thread.CurrentThread.CurrentCulture = this.culture;
            Thread.CurrentThread.CurrentUICulture = this.uiCulture;
        }

        public override void After(MethodInfo methodUnderTest)
        {
            Thread.CurrentThread.CurrentCulture = originalCulture;
            Thread.CurrentThread.CurrentUICulture = originalUiCulture;
        }
    }
}
