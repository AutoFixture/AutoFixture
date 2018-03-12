using System;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A specification that evaluates specimen request to see if it's for the delegate type.
    /// </summary>
    public class DelegateSpecification : IRequestSpecification
    {
        /// <summary>
        /// Evaluates a request for a specimen.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is of a delegate type, otherwise <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            // Test against MulticastDelegate instead of Delegate base class
            // because Brad Abrams says that we "should pretend that [Delegate
            // and MulticaseDelegate] are merged and that only MulticastDelegate exists."
            // http://blogs.msdn.com/b/brada/archive/2004/02/05/68415.aspx
            return request is Type type && type.GetTypeInfo().IsSubclassOf(typeof(MulticastDelegate));
        }
    }
}