using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;
using Rhino.Mocks;

namespace Ploeh.AutoFixture.AutoRhinoMock
{
    /// <summary>
    /// Encapsulates how to create a mock instance with Rhino Mocks, using a constructor with
    /// appropriate parameters.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The main purpose of RhinoMockConstructorMethod is to support creation of mock instances of
    /// abstract classes with non-default constructors. In this case Rhino Mocks must be supplied
    /// with the appropriate parameter values to be able to properly initialize the mock instance,
    /// since it needs to pass those parameters to the base class.
    /// </para>
    /// </remarks>
    public class RhinoMockConstructorMethod : IMethod
    {
        private readonly Type mockTargetType;
        private readonly ParameterInfo[] paramInfos;

        /// <summary>
        /// Initializes a new instance of the <see cref="RhinoMockConstructorMethod"/> class.
        /// </summary>
        /// <param name="mockTargetType">
        /// The type of which a mock instance should be created.
        /// </param>
        /// <param name="parameterInfos">
        /// The parameter information which can be used to identify the signature of the
        /// constructor.
        /// </param>
        public RhinoMockConstructorMethod(Type mockTargetType, ParameterInfo[] parameterInfos)
        {
            if (mockTargetType == null)
            {
                throw new ArgumentNullException("mockTargetType");
            }
            if (parameterInfos == null)
            {
                throw new ArgumentNullException("parameterInfos");
            }

            this.mockTargetType = mockTargetType;
            this.paramInfos = parameterInfos;
        }

        /// <summary>
        /// Gets the type of which a mock instance should be created.
        /// </summary>
        /// <seealso cref="RhinoMockConstructorMethod(Type, ParameterInfo[])" />
        public Type MockTargetType
        {
            get { return this.mockTargetType; }
        }

        /// <summary>
        /// Gets information about the parameters of the method.
        /// </summary>
        public IEnumerable<ParameterInfo> Parameters
        {
            get { return this.paramInfos; }
        }

        /// <summary>
        /// Creates a mock instance using Rhino Mocks.
        /// </summary>
        /// <param name="parameters">
        /// The parameters which will be supplied to the base constructor.
        /// </param>
        /// <returns>A mock instance created with Rhino Mocks.</returns>
        public object Invoke(IEnumerable<object> parameters)
        {
            return MockRepository.GenerateMock(this.mockTargetType, new Type[0], parameters.ToArray());
        }
    }
}
