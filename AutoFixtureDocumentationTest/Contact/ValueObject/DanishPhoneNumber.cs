using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixtureDocumentationTest.Contact.ValueObject
{
    public class DanishPhoneNumber
    {
        private readonly int number;

        public DanishPhoneNumber(int number)
        {
            this.number = number;
        }

        public int RawNumber
        {
            get { return this.number; }
        }
    }
}
