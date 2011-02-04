using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class GuardedMethodHost
    {
        public void ConsumeString(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (s.Length == 0)
            {
                throw new ArgumentException("String cannot be empty.", "s");
            }
        }

        public void ConsumeInt32(int i)
        {
        }

        public void ConsumeGuid(Guid g)
        {
            if (g == Guid.Empty)
            {
                throw new ArgumentException("Guid cannont be empty.", "g");
            }
        }

        public void ConsumeStringAndInt32(string s, int i)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (s.Length == 0)
            {
                throw new ArgumentException("String cannot be empty.", "s");
            }
        }

        public void ConsumeStringAndGuid(string s, Guid g)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (s.Length == 0)
            {
                throw new ArgumentException("String cannot be empty.", "s");
            }
            if (g == Guid.Empty)
            {
                throw new ArgumentException("Guid cannont be empty.", "g");
            }
        }

        public void ConsumeInt32AndGuid(int i, Guid g)
        {
            if (g == Guid.Empty)
            {
                throw new ArgumentException("Guid cannont be empty.", "g");
            }
        }

        public void ConsumeStringAndInt32AndGuid(string s, int i, Guid g)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (s.Length == 0)
            {
                throw new ArgumentException("String cannot be empty.", "s");
            }
            if (g == Guid.Empty)
            {
                throw new ArgumentException("Guid cannont be empty.", "g");
            }
        }
    }
}
