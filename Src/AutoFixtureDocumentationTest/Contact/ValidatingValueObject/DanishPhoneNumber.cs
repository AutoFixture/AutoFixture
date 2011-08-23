using System;

namespace Ploeh.AutoFixtureDocumentationTest.Contact.ValidatingValueObject
{
    public class DanishPhoneNumber
    {
        public const int MinValue = 112;

        private readonly int number;

        public DanishPhoneNumber(int number)
        {
            if (!DanishPhoneNumber.IsValid(number))
            {
                throw new ArgumentOutOfRangeException("number");
            }
            this.number = number;
        }

        public static bool IsValid(int number)
        {
            return (112 <= number)
                && (number <= 99999999);
        }

        public int RawNumber
        {
            get { return this.number; }
        }
    }
}
