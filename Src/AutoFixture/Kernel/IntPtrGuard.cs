using System;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Guards against requests for <see cref="IntPtr"/> by throwing an exception.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Some types (e.g. <see cref="Func{T}"/>) have constructors that take an <see cref="IntPtr"/>
    /// instance that indicate the address of the code block to be executed. IntPtr in itself have
    /// several constructors, amongst a few that AutoFixture thinks it can resolve; e.g. the
    /// constructor that takes an <see cref="int"/> as input. This means that AutoFixture, unless
    /// prevented, will create IntPtr instances with completely invalid addresses such as 1 or 2.
    /// When code attempts to use these invalid IntPtr instances, the process crashes.
    /// </para>
    /// <para>
    /// To prevent the process from crashing, AutoFixture considers request for IntPtr instances
    /// illegal. This class implements that rule by throwing an exception if such a request is
    /// detected.
    /// </para>
    /// </remarks>
    public class IntPtrGuard : ISpecimenBuilder
    {
        /// <summary>
        /// Guards against requests for <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A <see cref="NoSpecimen"/> instance, unless <paramref name="request"/> is a request for
        /// <see cref="IntPtr"/> in which case an exception is thrown.
        /// </returns>
        /// <exception cref="IllegalRequestException">
        /// <paramref name="request"/> is the <see cref="IntPtr"/> <see cref="Type"/>.
        /// </exception>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(IntPtr).Equals(request))
            {
                return new NoSpecimen();
            }

            throw new IllegalRequestException("A request for an IntPtr was detected. This is an unsafe resource that will crash the process if used, so the request is denied. A common source of IntPtr requests are requests for delegates such as Func<T> or Action<T>. If this is the case, the expected workaround is to Customize (Register or Inject) the offending type by specifying a proper creational strategy.");
        }
    }
}
