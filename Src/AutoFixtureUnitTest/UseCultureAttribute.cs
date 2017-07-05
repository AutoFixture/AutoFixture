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
            originalCulture = CultureInfo.CurrentCulture;
            originalUiCulture = CultureInfo.CurrentCulture;

            SetCurrentCulture(culture, uiCulture);
        }

        public override void After(MethodInfo methodUnderTest)
        {
            SetCurrentCulture(originalCulture, originalUiCulture);
        }

        private static void SetCurrentCulture(CultureInfo culture, CultureInfo uiCulture)
        {
#if NETFULL
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = uiCulture;
#else
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = uiCulture;
#endif
        }
    }
}
