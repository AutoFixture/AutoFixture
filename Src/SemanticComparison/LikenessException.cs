using System;

namespace Ploeh.SemanticComparison
{
    /// <summary>
    /// Represents an error where two semantically comparable instances were expected to match, but
    /// didn't.
    /// </summary>
    public class LikenessException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LikenessException"/> class.
        /// </summary>
        public LikenessException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LikenessException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public LikenessException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LikenessException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public LikenessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
