using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates an instance method. This is essentially an Adapter over
    /// <see cref="MethodInfo"/>.
    /// </summary>
    public class InstanceMethod : IMethod
    {
        private readonly MethodInfo method;
        private readonly ParameterInfo[] paramInfos;
        private readonly object owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceMethod"/> class.
        /// </summary>
        /// <param name="instanceMethod">The instance method.</param>
        /// <param name="owner">The owner.</param>
        /// <remarks>
        /// <para>
        /// The owner is expected to expose the method designated by
        /// <paramref name="instanceMethod" />. If not, the <see cref="Invoke" /> method will fail.
        /// </para>
        /// </remarks>
        /// <seealso cref="Invoke" />
        /// <seealso cref="Method" />
        /// <seealso cref="Owner" />
        public InstanceMethod(MethodInfo instanceMethod, object owner)
        {
            if (instanceMethod == null)
            {
                throw new ArgumentNullException("instanceMethod");
            }
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            this.method = instanceMethod;
            this.paramInfos = this.method.GetParameters();
            this.owner = owner;
        }

        /// <summary>
        /// Gets the method originally supplied through the constructor.
        /// </summary>
        /// <seealso cref="InstanceMethod(MethodInfo, object)" />
        public MethodInfo Method
        {
            get { return this.method; }
        }

        /// <summary>
        /// Gets the owner originally supplied through the constructor.
        /// </summary>
        /// <seealso cref="InstanceMethod(MethodInfo, object)" />
        public object Owner
        {
            get { return this.owner; }
        }

        #region IMethod Members

        /// <summary>
        /// Gets information about the parameters of the method.
        /// </summary>
        public IEnumerable<ParameterInfo> Parameters
        {
            get { return this.paramInfos; }
        }

        /// <summary>
        /// Invokes the method with the supplied parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The result of the method call.</returns>
        public object Invoke(IEnumerable<object> parameters)
        {
            return this.method.Invoke(this.owner, parameters.ToArray());
        }

        #endregion
    }
}
