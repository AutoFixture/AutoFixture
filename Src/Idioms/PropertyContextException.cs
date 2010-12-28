using System;
using System.Runtime.Serialization;

namespace Ploeh.AutoFixture.Idioms
{
    [Serializable]
    public class PropertyContextException : Exception
    {
        public PropertyContextException()
        {
        }

        public PropertyContextException(string message) : base(message)
        {
        }

        public PropertyContextException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PropertyContextException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}