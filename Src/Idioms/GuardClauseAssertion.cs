using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public class GuardClauseAssertion : IdiomaticAssertion
    {
        private readonly ISpecimenBuilderComposer composer;
        private readonly IBehaviorExpectation behaviorExpectation;

        public GuardClauseAssertion(ISpecimenBuilderComposer composer)
            : this(composer, new CompositeBehaviorExpectation(new NullReferenceBehaviorExpectation()))
        {
        }

        public GuardClauseAssertion(ISpecimenBuilderComposer composer, IBehaviorExpectation behaviorExpectation)
        {
            this.composer = composer;
            this.behaviorExpectation = behaviorExpectation;
        }

        public ISpecimenBuilderComposer Composer
        {
            get { return this.composer; }
        }

        public IBehaviorExpectation BehaviorExpectation
        {
            get { return this.behaviorExpectation; }
        }

        public override void Verify(ConstructorInfo constructorInfo)
        {
            var paramInfos = constructorInfo.GetParameters();

            var method = new ConstructorMethod(constructorInfo);

            var parameters = (from pi in paramInfos
                              select this.Composer.CreateAnonymous(pi.ParameterType)).ToList();

            var i = 0;
            foreach (var pi in paramInfos)
            {
                var expansion = new IndexedReplacement<object>(i++, parameters);

                var command = new MethodInvokeCommand(method, expansion, pi);
                var unwrapper = new ReflectionExceptionUnwrappingCommand(command);
                this.BehaviorExpectation.Verify(unwrapper);
            }
        }

        public override void Verify(MethodInfo methodInfo)
        {
            var paramInfos = methodInfo.GetParameters();

            var owner = this.Composer.CreateAnonymous(methodInfo.ReflectedType);
            var method = new InstanceMethod(methodInfo, owner);

            var parameters = (from pi in paramInfos
                              select this.Composer.CreateAnonymous(pi.ParameterType)).ToList();

            var i = 0;
            foreach (var pi in paramInfos)
            {
                var expansion = new IndexedReplacement<object>(i++, parameters);

                var command = new MethodInvokeCommand(method, expansion, pi);
                var unwrapper = new ReflectionExceptionUnwrappingCommand(command);
                this.BehaviorExpectation.Verify(unwrapper);
            }
        }

        public override void Verify(PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetSetMethod() == null)
            {
                return;
            }

            var owner = this.Composer.CreateAnonymous(propertyInfo.ReflectedType);
            var command = new PropertySetCommand(propertyInfo, owner);
            var unwrapper = new ReflectionExceptionUnwrappingCommand(command);
            this.BehaviorExpectation.Verify(unwrapper);
        }
    }
}
