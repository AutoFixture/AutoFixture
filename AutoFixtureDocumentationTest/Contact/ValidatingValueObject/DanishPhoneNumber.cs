using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixtureDocumentationTest.Contact.ValidatingValueObject
{
    public class DanishPhoneNumber
    {
        private readonly int number;

        public const int MinValue = 112;

        public DanishPhoneNumber(int number)
        {
            if ((number < 112) ||
                (number > 99999999))
            {
                throw new ArgumentOutOfRangeException("number");
            }
            this.number = number;
        }

        public int RawNumber
        {
            get { return this.number; }
        }
    }
}
