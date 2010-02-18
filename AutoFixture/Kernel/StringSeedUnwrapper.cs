using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Unwraps a request for a string <see cref="Seed"/> to a request for a string and appends the
    /// seed to the result.
    /// </summary>
    public class StringSeedUnwrapper : InstanceGeneratorNode
    {
        /// <summary>
        /// Initialized a new instance of the the <see cref="StringSeedUnwrapper"/> class with the
        /// supplied parent.
        /// </summary>
        /// <param name="parent"></param>
        public StringSeedUnwrapper(IInstanceGenerator parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Creates a new <see cref="InstanceGeneratorNode.GeneratorStrategy" /> instance that
        /// implements the behavior of <see cref="StringSeedUnwrapper" />.
        /// </summary>
        /// <param name="request">
        /// A <see cref="ICustomAttributeProvider" /> instance.
        /// </param>
        /// <returns>
        /// A <see cref="InstanceGeneratorNode.GeneratorStrategy"/> that implements the behavior of 
        /// <see cref="StringSeedUnwrapper" />.
        /// </returns>
        protected override InstanceGeneratorNode.GeneratorStrategy CreateStrategy(ICustomAttributeProvider request)
        {
            return new StringSeedUnwrapStrategy(this.Parent, request);
        }

        private class StringSeedUnwrapStrategy : GeneratorStrategy
        {
            private readonly Seed seed;

            internal StringSeedUnwrapStrategy(IInstanceGenerator parent, ICustomAttributeProvider request)
                : base(parent, request)
            {
                this.seed = request as Seed;
            }

            public override bool CanGenerate()
            {
                return this.seed != null
                    && this.seed.TargetType == typeof(string)
                    && this.seed.Value is string
                    && this.Parent.CanGenerate(typeof(string));
            }

            public override object Generate()
            {
                return (string)this.seed.Value + this.Parent.Generate(typeof(string));
            }
        }

    }
}
