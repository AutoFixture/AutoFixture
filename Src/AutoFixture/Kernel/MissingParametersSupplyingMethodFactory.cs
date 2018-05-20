using System;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Creates a new <see cref="MissingParametersSupplyingMethod" /> for an
    /// instance <see cref="MethodInfo"/>.
    /// </summary>
    public class MissingParametersSupplyingMethodFactory : IMethodFactory
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MissingParametersSupplyingMethodFactory"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public MissingParametersSupplyingMethodFactory(object owner)
        {
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        /// <summary>
        /// Gets the owner originally supplied through the constructor.
        /// </summary>
        /// <seealso cref="MissingParametersSupplyingMethodFactory(object)" />
        public object Owner { get; }

        /// <summary>
        /// Creates a <see cref="InstanceMethod" /> decorated with
        /// <see cref="MissingParametersSupplyingMethod" /> for the supplied methodInfo.
        /// </summary>
        /// <param name="methodInfo">The methodInfo.</param>
        /// <returns>Method for <paramref name="methodInfo"/>.</returns>
        public IMethod Create(MethodInfo methodInfo)
        {
            return new MissingParametersSupplyingMethod(new InstanceMethod(methodInfo, this.Owner));
        }
    }
}