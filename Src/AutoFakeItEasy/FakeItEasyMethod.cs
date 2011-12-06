using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// Represents a Fake(OfT) method that can be invoked with a known set of parameters.
    /// </summary>
    public class FakeItEasyMethod : IMethod
    {
        private readonly MethodInfo methodInfo;
        private readonly ParameterInfo[] methodParameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeItEasyMethod"/> class.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <param name="methodParameters">The parameter information which can be used to identify the signature of the method.</param>
        public FakeItEasyMethod(MethodInfo methodInfo, ParameterInfo[] methodParameters)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }

            if (methodParameters == null)
            {
                throw new ArgumentNullException("methodParameters");
            }

            this.methodInfo = methodInfo;
            this.methodParameters = methodParameters;
        }

        /// <summary>
        /// Gets information about the parameters of the method.
        /// </summary>
        public IEnumerable<ParameterInfo> Parameters
        {
            get { return this.methodParameters; }
        }

        /// <summary>
        /// Invokes the method with the supplied parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The result of the method call.
        /// </returns>
        public object Invoke(IEnumerable<object> parameters)
        {
            return this.methodInfo.Invoke(null, new[] { parameters.ToArray() });
        }
    }
}