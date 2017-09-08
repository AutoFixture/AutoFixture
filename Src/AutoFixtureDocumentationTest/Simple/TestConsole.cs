//#define ALLOWCONSOLE

using System;

namespace Ploeh.AutoFixtureDocumentationTest.Simple
{
    internal static class TestConsole
    {
        internal static void WriteLine(string value)
        {
#if ALLOWCONSOLE
            Console.WriteLine(value);
#endif
        }
    }
}
