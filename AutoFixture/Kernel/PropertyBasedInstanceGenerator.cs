using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Generates instances based on <see cref="PropertyInfo"/> instances.
    /// </summary>
    public class PropertyBasedInstanceGenerator : InstanceGeneratorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBasedInstanceGenerator" /> class
        /// with the provided parent.
        /// </summary>
        /// <param name="parent">The parent generator.</param>
        public PropertyBasedInstanceGenerator(IInstanceGenerator parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Creates a new <see cref="InstanceGeneratorNode.GeneratorStrategy" /> instance that
        /// implements the behavior of <see cref="PropertyBasedInstanceGenerator" />.
        /// </summary>
        /// <param name="attributeProvider">
        /// A <see cref="ICustomAttributeProvider" /> instance.
        /// </param>
        /// <returns>
        /// A <see cref="InstanceGeneratorNode.GeneratorStrategy"/> that implements the behavior of 
        /// <see cref="PropertyBasedInstanceGenerator" />.
        /// </returns>
        protected override InstanceGeneratorNode.GeneratorStrategy CreateStrategy(ICustomAttributeProvider attributeProvider)
        {
            return new PropertyGeneratorStrategy(this.Parent, attributeProvider);
        }

        private class PropertyGeneratorStrategy : GeneratorStrategy
        {
            private readonly PropertyInfo propertyInfo;

            internal PropertyGeneratorStrategy(IInstanceGenerator parent, ICustomAttributeProvider attributeProvider)
                : base(parent, attributeProvider)
            {
                this.propertyInfo = attributeProvider as PropertyInfo;
            }

            public override bool CanGenerate()
            {
                return this.propertyInfo != null
                    && this.Parent.CanGenerate(this.propertyInfo.PropertyType);
            }

            public override object Generate()
            {
                return this.Parent.Generate(this.propertyInfo.PropertyType);
            }
        }

    }
}
