using System;

namespace AutoFixtureDocumentationTest.Contact.ValidatingValueObject
{
    public class DanishPhoneNumber
    {
        public const int MinValue = 112;

        public DanishPhoneNumber(int number)
        {
            if (!DanishPhoneNumber.IsValid(number))
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }
            this.RawNumber = number;
        }

        public static bool IsValid(int number)
        {
            return (number >= 112)
                && (number <= 99999999);
        }

        public int RawNumber { get; }
    }
}
