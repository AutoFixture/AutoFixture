using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FakeItEasy;
using FakeItEasy.Creation;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoFakeItEasy
{
    internal class FakeItEasyMethod : IMethod
    {
        private readonly ParameterInfo[] paramInfos;
        private readonly Type targetType;

        private IEnumerable<object> parameters;

        internal FakeItEasyMethod(Type fakeTargetType, ParameterInfo[] paramInfos)
        {
            if (fakeTargetType == null)
            {
                throw new ArgumentNullException("fakeTargetType");
            }

            if (paramInfos == null)
            {
                throw new ArgumentNullException("paramInfos");
            }

            this.paramInfos = paramInfos;
            this.targetType = fakeTargetType;
        }

        public IEnumerable<ParameterInfo> Parameters
        {
            get { return this.paramInfos; }
        }

        public object Invoke(IEnumerable<object> parameters)
        {
            this.parameters = parameters;
            MethodInfo fake = typeof(A)
                .GetMethods().Where(x =>
                       x.Name.StartsWith("Fake")
                    && x.ContainsGenericParameters
                    && x.GetParameters().Count() == 1).Single()
                .MakeGenericMethod(new[] { this.targetType });

            MethodInfo argumentsForConstructor = this.GetType()
                .GetMethod("SetArgumentsForConstructor", BindingFlags.Instance | BindingFlags.NonPublic)
                .MakeGenericMethod(new[] { this.targetType });

            Type actionType = typeof(Action<>).MakeGenericType(
                 typeof(IFakeOptionsBuilder<>).MakeGenericType(this.targetType));
            Delegate action = Delegate.CreateDelegate(actionType, this, argumentsForConstructor);

            return fake.Invoke(null, new[] { action });
        }

        private void SetArgumentsForConstructor<T>(IFakeOptionsBuilder<T> o)
        {
            o.WithArgumentsForConstructor(this.parameters);
        }
    }
}
