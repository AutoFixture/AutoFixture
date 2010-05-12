using System;
using System.Runtime.Serialization;

namespace Ploeh.AutoFixture.Idioms
{
    [Serializable]
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

        protected PickedPropertyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}