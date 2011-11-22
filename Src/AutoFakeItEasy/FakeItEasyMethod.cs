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
        private readonly ParameterInfo[] paramInfos;
        private readonly Type fakeTargetType;

        private IEnumerable<object> parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeItEasyMethod"/> class.
        /// </summary>
        /// <param name="fakeTargetType">
        /// The type of which a mock instance should be created.
        /// </param>
        /// <param name="parameterInfos">
        /// The parameter information which can be used to identify the signature of the
        /// constructor.
        /// </param>
        public FakeItEasyMethod(Type fakeTargetType, ParameterInfo[] parameterInfos)
        {
            if (fakeTargetType == null)
            {
                throw new ArgumentNullException("fakeTargetType");
            }

            if (parameterInfos == null)
            {
                throw new ArgumentNullException("parameterInfos");
            }

            this.fakeTargetType = fakeTargetType;
            this.paramInfos = parameterInfos;
        }

        /// <summary>
        /// Gets the type of which a mock instance should be created.
        /// </summary>
        /// <seealso cref="FakeItEasyMethod(Type, ParameterInfo[])" />
        public Type FakeTargetType
        {
            get { return this.fakeTargetType; }
        }

        /// <summary>
        /// Gets information about the parameters of the method.
        /// </summary>
        public IEnumerable<ParameterInfo> Parameters
        {
            get { return this.paramInfos; }
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
            MethodInfo argumentsForConstructor = this.GetType()
                .GetMethod("SetArgumentsForConstructor", BindingFlags.Instance | BindingFlags.NonPublic)
                .MakeGenericMethod(new[] { this.fakeTargetType });

            this.parameters = parameters;
            Type actionType = typeof(Action<>).MakeGenericType(
                 typeof(IFakeOptionsBuilder<>).MakeGenericType(this.fakeTargetType));
            Delegate action = Delegate.CreateDelegate(actionType, this, argumentsForConstructor);

            var ok = typeof(Fake<>)
                .MakeGenericType(this.fakeTargetType)
                .GetConstructors();

            return typeof(Fake<>)
                .MakeGenericType(this.fakeTargetType)
                .GetConstructors()[1]
                .Invoke(new[] { action });
        }

        private void SetArgumentsForConstructor<T>(IFakeOptionsBuilder<T> o)
        {
            if (typeof(T).IsInterface)
            {
                return;
            }
            
            o.WithArgumentsForConstructor(this.parameters);
        }
    }
}
