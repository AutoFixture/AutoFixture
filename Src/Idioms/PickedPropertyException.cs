using System;
namespace Ploeh.AutoFixture.Idioms
{
    public class PickedPropertyException : Exception
    {
        public PickedPropertyException()
        {
        }

        public PickedPropertyException(string message) : base(message)
        {
        }

        public PickedPropertyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}