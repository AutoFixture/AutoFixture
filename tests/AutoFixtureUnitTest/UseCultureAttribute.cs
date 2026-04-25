using System.Globalization;
using System.Reflection;
using System.Threading;
using Xunit.v3;

namespace AutoFixtureUnitTest;

public class UseCultureAttribute : BeforeAfterTestAttribute
{
    private CultureInfo _originalCulture;

    private CultureInfo _originalUiCulture;

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

    public override void Before(MethodInfo methodUnderTest, IXunitTest test)
    {
        _originalCulture = CultureInfo.CurrentCulture;
        _originalUiCulture = CultureInfo.CurrentCulture;

        SetCurrentCulture(_culture, _uiCulture);
    }

    public override void After(MethodInfo methodUnderTest, IXunitTest test)
    {
        SetCurrentCulture(_originalCulture, _originalUiCulture);
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