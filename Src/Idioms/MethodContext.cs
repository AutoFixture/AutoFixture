using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    public class MethodContext : IMemberContext
    {
        private readonly ISpecimenBuilderComposer composer;
        private readonly MethodBase methodBase;
        private readonly Action<object[]> invoke;

        public MethodContext(ISpecimenBuilderComposer composer, MethodInfo methodInfo)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }

            this.composer = composer;
            this.methodBase = methodInfo;
            this.invoke = args =>
            {
                var specimen = this.Composer.CreateAnonymous(this.MethodBase.ReflectedType);
                methodInfo.Invoke(specimen, args);
            };
        }

        public MethodContext(ISpecimenBuilderComposer composer, ConstructorInfo constructorInfo)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (constructorInfo == null)
            {
                throw new ArgumentNullException("constructorInfo");
            }

            this.composer = composer;
            this.methodBase = constructorInfo;
            this.invoke = args => constructorInfo.Invoke(args);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ISpecimenBuilderComposer Composer
        {
            get { return this.composer; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public MethodBase MethodBase
        {
            get { return this.methodBase; }
        }

        #region IMemberContext Members

        public void VerifyBoundaries(IBoundaryConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            var argumentMap = (from p in this.MethodBase.GetParameters()
                               select new
                               {
                                   Parameter = p,
                                   Value = this.Composer.CreateAnonymous(p)
                               }
                               ).ToDictionary(x => x.Parameter, x => x.Value);

            var combinations = from p in argumentMap.Keys
                               from b in convention.CreateBoundaryBehaviors(p.ParameterType)
                               select new
                               {
                                   Parameter = p,
                                   Behavior = b.UnwrapReflectionExceptions()
                               };

            foreach (var c in combinations)
            {
                Action<object> invokeMethod = x =>
                    {
                        var arguments = new Dictionary<ParameterInfo, object>(argumentMap);
                        arguments[c.Parameter] = x;

                        this.invoke(arguments.Values.ToArray());
                    };

                c.Behavior.Assert(invokeMethod);
            }
        }

        #endregion
    }
}
