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
#if SYSTEM_THREADING_THREAD_CULTURESETTERS
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = uiCulture;
#elif SYSTEM_GLOBALIZATION_CULTUREINFO_CULTURESETTERS
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = uiCulture;
#else
#error No culture setter is defined.
#endif
        }
    }
}
