using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Creates a new instance of the requested type by invoking its most modest constructor.
    /// </summary>
    public class ModestConstructorInvoker : InstanceGeneratorNode
    {
        /// <summary>
        /// Initialized a new instance of the <see cref="ModestConstructorInvoker"/> class with the
        /// supplied parent.
        /// </summary>
        /// <param name="parent">The parent generator</param>
        public ModestConstructorInvoker(IInstanceGenerator parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Creates a new <see cref="InstanceGeneratorNode.GeneratorStrategy" /> instance that
        /// implements the behavior of <see cref="ModestConstructorGeneratorStrategy" />.
        /// </summary>
        /// <param name="request">
        /// A <see cref="ICustomAttributeProvider" /> instance.
        /// </param>
        /// <returns>
        /// A <see cref="InstanceGeneratorNode.GeneratorStrategy"/> that implements the behavior of 
        /// <see cref="ModestConstructorGeneratorStrategy" />.
        /// </returns>
        protected override InstanceGeneratorNode.GeneratorStrategy CreateStrategy(ICustomAttributeProvider request)
        {
            return new ModestConstructorGeneratorStrategy(this.Parent, request);
        }

        private class ModestConstructorGeneratorStrategy : GeneratorStrategy
        {
            private readonly ConstructorInfo ctor;

            internal ModestConstructorGeneratorStrategy(IInstanceGenerator parent, ICustomAttributeProvider request)
                : base(parent, request)
            {
                this.ctor = ModestConstructorGeneratorStrategy.GetModestConstructor(request);
            }

            public override bool CanGenerate()
            {
                if (this.ctor == null)
                {
                    return false;
                }

                return (from pi in this.ctor.GetParameters()
                        select this.Parent.CanGenerate(pi.ParameterType)).All(canGenerate => canGenerate);
            }

            public override object Generate()
            {
                var paramValues = from pi in this.ctor.GetParameters()
                                  select this.Parent.Generate(pi.ParameterType);
                return this.ctor.Invoke(paramValues.ToArray());
            }

            private static ConstructorInfo GetModestConstructor(ICustomAttributeProvider request)
            {
                var requestedType = request as Type;
                if (requestedType == null)
                {
                    return null;
                }

                return (from ci in requestedType.GetConstructors()
                        let ps = ci.GetParameters()
                        orderby ps.Length ascending
                        select ci).FirstOrDefault();
            }
        }
    }
}
