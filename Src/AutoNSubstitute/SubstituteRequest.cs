using System;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    /// <summary>
    /// Defines a request for a substitute.
    /// </summary>
    /// <remarks>
    /// Unlike some other test isolation frameworks which define types that represent the dynamically generated proxies,
    /// such as Mock{T} in Moq and Fake{T} in FakeItEasy, NSubstitute does not. This class fills this gap and allows 
    /// AutoFixture to distinguish an explicit request for a substitute from a request for a regular type. This is 
    /// necessary to support creation of substitutes for concrete types.
    /// </remarks>
    public class SubstituteRequest
    {
        private readonly Type targetType;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubstituteRequest"/> class.
        /// </summary>
        /// <param name="targetType">
        /// A <see cref="Type"/> for which a substitute is being requested.
        /// </param>
        public SubstituteRequest(Type targetType)
        {      
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            this.targetType = targetType;
        }

        /// <summary>
        /// Gets the type for which a substitute is requested.
        /// </summary>
        public Type TargetType 
        {
            get { return this.targetType; }            
        }
    }
}
