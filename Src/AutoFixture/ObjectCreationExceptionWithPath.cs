using System;

namespace AutoFixture
{

    /// <summary>
    /// The exception that is thrown when AutoFixture is unable to create an object.
    /// This exception is supposed to contain the full request path.
    /// </summary>
#if SYSTEM_RUNTIME_SERIALIZATION
    [Serializable]
#endif
    internal class ObjectCreationExceptionWithPath: ObjectCreationException
    {
        public ObjectCreationExceptionWithPath()
        {
        }       
        
        public ObjectCreationExceptionWithPath(string message) : base(message)
        {
        }

        public ObjectCreationExceptionWithPath(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if SYSTEM_RUNTIME_SERIALIZATION
        protected ObjectCreationExceptionWithPath(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}