using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Unwraps a request for a <see cref="Seed"/> to a request for its
    /// <see cref="Seed.TargetType"/> while ignoring the <see cref="Seed.Value"/>.
    /// </summary>
    public class ValueIgnoringSeedUnwrapper : InstanceGeneratorNode
    {
        /// <summary>
        /// Initialized a new instance of the <see cref="ValueIgnoringSeedUnwrapper"/> class with
        /// the supplied parent.
        /// </summary>
        /// <param name="parent">The parent generator.</param>
        public ValueIgnoringSeedUnwrapper(IInstanceGenerator parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Creates a new <see cref="InstanceGeneratorNode.GeneratorStrategy" /> instance that
        /// implements the behavior of <see cref="ValueIgnoringSeedUnwrapper" />.
        /// </summary>
        /// <param name="request">
        /// A <see cref="ICustomAttributeProvider" /> instance.
        /// </param>
        /// <returns>
        /// A <see cref="InstanceGeneratorNode.GeneratorStrategy"/> that implements the behavior of 
        /// <see cref="ValueIgnoringSeedUnwrapper" />.
        /// </returns>
        protected override InstanceGeneratorNode.GeneratorStrategy CreateStrategy(ICustomAttributeProvider request)
        {
            return new TargetTypeForwardingGeneratorStrategy(this.Parent, request);
        }

        private class TargetTypeForwardingGeneratorStrategy : GeneratorStrategy
        {
            private readonly Seed seed;

            internal TargetTypeForwardingGeneratorStrategy(IInstanceGenerator parent, ICustomAttributeProvider request)
                : base(parent, request)
            {
                this.seed = request as Seed;
            }

            public override bool CanGenerate()
            {
                return this.seed != null
                    && this.Parent.CanGenerate(this.seed.TargetType);
            }

            public override object Generate()
            {
                return this.Parent.Generate(this.seed.TargetType);
            }
        }

    }
}
