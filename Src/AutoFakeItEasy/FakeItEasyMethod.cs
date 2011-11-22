using System;
using System.Collections.Generic;
using System.Reflection;
using FakeItEasy;
using FakeItEasy.Creation;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// Encapsulates logic on how to create a mock instance with FakeIteasy, using a constructor with
    /// appropriate parameters.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The main purpose of FakeItEasyMethod is to support creation of fake instances of
    /// abstract classes with non-default constructors. In this case FakeItEasy must be supplied
    /// with the appropriate parameter values to be able to properly initialize the fake instance,
    /// since it needs to pass those parameters to the base class.
    /// </para>
    /// </remarks>
    public class FakeItEasyMethod : IMethod
    {
        private readonly ParameterInfo[] methodParameters;
        private readonly Type targetType;

        private IEnumerable<object> argumentsForConstructor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeItEasyMethod"/> class.
        /// </summary>
        /// <param name="targetType">
        /// The type of which a mock instance should be created.
        /// </param>
        /// <param name="methodParameters">
        /// The parameter information which can be used to identify the signature of the method.
        /// </param>
        public FakeItEasyMethod(Type targetType, ParameterInfo[] methodParameters)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            if (methodParameters == null)
            {
                throw new ArgumentNullException("methodParameters");
            }

            this.targetType = targetType;
            this.methodParameters = methodParameters;
        }

        /// <summary>
        /// Gets the type of which a mock instance should be created.
        /// </summary>
        /// <seealso cref="FakeItEasyMethod(Type, ParameterInfo[])" />
        public Type TargetType
        {
            get { return this.targetType; }
        }

        /// <summary>
        /// Gets information about the parameters of the method.
        /// </summary>
        public IEnumerable<ParameterInfo> Parameters
        {
            get { return this.methodParameters; }
        }

        /// <summary>
        /// Creates a mock instance using FakeItEasy.
        /// </summary>
        /// <param name="parameters">
        /// The parameters which will be supplied to the base constructor.
        /// </param>
        /// <returns>A mock instance created with FakeItEasy.</returns>
        public object Invoke(IEnumerable<object> parameters)
        {
            this.argumentsForConstructor = parameters;

            Type actionType = typeof(Action<>).MakeGenericType(
                 typeof(IFakeOptionsBuilder<>).MakeGenericType(this.targetType));

            MethodInfo actionMethod = this.GetType()
                .GetMethod("SetArgumentsForConstructor", BindingFlags.Instance | BindingFlags.NonPublic)
                .MakeGenericMethod(new[] { this.targetType });

            Delegate action = Delegate.CreateDelegate(actionType, this, actionMethod);

            return typeof(Fake<>)
                .MakeGenericType(this.targetType)
                .GetConstructors()[1]
                .Invoke(new[] { action });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This method is used by Reflection. It describes the method that is passed in the Action<IFakeOptionsBuilder<T>> overload of the Fake<T> constructor.")]
        private void SetArgumentsForConstructor<T>(IFakeOptionsBuilder<T> o)
        {
            if (typeof(T).IsInterface)
            {
                return;
            }

            o.WithArgumentsForConstructor(this.argumentsForConstructor);
        }
    }
}
