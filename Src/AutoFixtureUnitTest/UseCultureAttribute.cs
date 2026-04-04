using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Xunit.Sdk;

namespace AutoFixtureUnitTest;

public class UseCultureAttribute : BeforeAfterTestAttribute
{
    [ThreadStatic]
    private static CultureInfo s_originalCulture_t;

    [ThreadStatic]
    private static CultureInfo s_originalUiCulture_t;

    private readonly CultureInfo _culture;
    private readonly CultureInfo _uiCulture;

    public UseCultureAttribute(string culture)
        : this(culture, culture)
    {
    }

    public UseCultureAttribute(string culture, string uiCulture)
    {
        _culture = new CultureInfo(culture);
        _uiCulture = new CultureInfo(uiCulture);
    }

    public override void Before(MethodInfo methodUnderTest)
    {
        s_originalCulture_t = CultureInfo.CurrentCulture;
        s_originalUiCulture_t = CultureInfo.CurrentCulture;

        SetCurrentCulture(_culture, _uiCulture);
    }

    public override void After(MethodInfo methodUnderTest)
    {
        SetCurrentCulture(s_originalCulture_t, s_originalUiCulture_t);
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