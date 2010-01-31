using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Generates instances based on <see cref="PropertyInfo"/> instances.
    /// </summary>
    public class PropertyBasedInstanceGenerator : InstanceGenerator
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
        /// Creates a new <see cref="InstanceGenerator.GeneratorStrategy" /> instance that
        /// implements the behavior of <see cref="PropertyBasedInstanceGenerator" />.
        /// </summary>
        /// <param name="attributeProvider">
        /// A <see cref="ICustomAttributeProvider" /> instance.
        /// </param>
        /// <returns>
        /// A <see cref="InstanceGenerator.GeneratorStrategy"/> that implements the behavior of 
        /// <see cref="PropertyBasedInstanceGenerator" />.
        /// </returns>
        protected override InstanceGenerator.GeneratorStrategy CreateStrategy(ICustomAttributeProvider attributeProvider)
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
