//#define ALLOWCONSOLE

using System;
using System.IO;
using System.Text;

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
            public override void Write(char value)
            {
            }

            public override Encoding Encoding
            {
                get { return Encoding.Unicode; }
            }
        }
    }
}
