// #define ALLOWCONSOLE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
