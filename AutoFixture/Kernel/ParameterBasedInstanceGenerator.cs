using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Generates instances based on <see cref="ParameterInfo"/> instances.
    /// </summary>
    public class ParameterBasedInstanceGenerator : InstanceGeneratorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterBasedInstanceGenerator"/> class
        /// with the supplied parent.
        /// </summary>
        /// <param name="parent">The parent generator.</param>
        public ParameterBasedInstanceGenerator(IInstanceGenerator parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Creates a new <see cref="InstanceGeneratorNode.GeneratorStrategy" /> instance that
        /// implements the behavior of <see cref="ParameterBasedInstanceGenerator" />.
        /// </summary>
        /// <param name="request">
        /// A <see cref="ICustomAttributeProvider" /> instance.
        /// </param>
        /// <returns>
        /// A <see cref="InstanceGeneratorNode.GeneratorStrategy"/> that implements the behavior of 
        /// <see cref="ParameterBasedInstanceGenerator" />.
        /// </returns>
        protected override InstanceGeneratorNode.GeneratorStrategy CreateStrategy(ICustomAttributeProvider request)
        {
            return new ParameterGeneratorStrategy(this.Parent, request);
        }

        private class ParameterGeneratorStrategy : GeneratorStrategy
        {
            private readonly ParameterInfo parameterInfo;

            internal ParameterGeneratorStrategy(IInstanceGenerator parent, ICustomAttributeProvider request)
                : base(parent, request)
            {
                this.parameterInfo = request as ParameterInfo;
            }

            public override bool CanGenerate()
            {
                return this.parameterInfo != null
                    && this.Parent.CanGenerate(new Seed(this.parameterInfo.ParameterType, this.parameterInfo.Name));
            }

            public override object Generate()
            {
                return this.Parent.Generate(new Seed(this.parameterInfo.ParameterType, this.parameterInfo.Name));
            }
        }

    }
}
