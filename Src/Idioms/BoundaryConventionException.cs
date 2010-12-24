using System;
using System.Runtime.Serialization;

namespace Ploeh.AutoFixture.Idioms
{
    [Serializable]
    public class BoundaryConventionException : Exception
    {
        public BoundaryConventionException()
        {
        }

        public BoundaryConventionException(string message) : base(message)
        {
        }

        public BoundaryConventionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BoundaryConventionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}