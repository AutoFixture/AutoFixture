using System;
using System.Runtime.Serialization;

namespace Ploeh.AutoFixture.Idioms
{
    [Serializable]
    public class ValueGuardConventionException : Exception
    {
        public ValueGuardConventionException()
        {
        }

        public ValueGuardConventionException(string message) : base(message)
        {
        }

        public ValueGuardConventionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ValueGuardConventionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}