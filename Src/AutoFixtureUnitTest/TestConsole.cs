//#define ALLOWCONSOLE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ploeh.AutoFixtureUnitTest
{
    internal static class TestConsole
    {
        internal static TextWriter Out
        {
            get 
            {
#if ALLOWCONSOLE
                return Console.Out;
#else
                return new NullWriter();
#endif
            }
        }

        private class NullWriter : TextWriter
        {
            public override Encoding Encoding
            {
                get { return Encoding.Unicode; }
            }
        }
    }
}
