using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Default reusable implementation of <see cref="IInstanceGenerator" />
    /// </summary>
    public abstract class InstanceGenerator : IInstanceGenerator
    {
        private readonly IInstanceGenerator parent;

        /// <summary>
        /// Initializes a new instance of <see cref="InstanceGenerator" /> with the provided
        /// parent.
        /// </summary>
        /// <param name="parent">The parent.</param>
        protected InstanceGenerator(IInstanceGenerator parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            this.parent = parent;
        }

        /// <summary>
        /// Gets the parent <see cref="IInstanceGenerator"/>
        /// </summary>
        public IInstanceGenerator Parent
        {
            get { return this.parent; }
        }

        #region IInstanceGenerator Members

        /// <summary>
        /// Indicates whether the current instance can generate object instances based on the given
        /// <see cref="ICustomAttributeProvider"/>.
        /// </summary>
        /// <param name="attributeProvider">
        /// Provides a description upon which the current instance can base its decision.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the current instance can generate object instances based on
        /// <paramref name="attributeProvider"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool CanGenerate(ICustomAttributeProvider attributeProvider)
        {
            return this.CreateStrategy(attributeProvider).CanGenerate();
        }

        /// <summary>
        /// Generates object instances based on the given <see cref="ICustomAttributeProvider"/>.
        /// </summary>
        /// <param name="attributeProvider">
        /// Provides a description that guides the current instance in generating object instances.
        /// </param>
        /// <returns>A new object based on <paramref name="attributeProvider"/>.</returns>
        public object Generate(ICustomAttributeProvider attributeProvider)
        {
            var strategy = this.CreateStrategy(attributeProvider);
            if (!strategy.CanGenerate())
            {
                throw new ArgumentException("Cannot generate a an instance for the requested attribute provider.", "attributeProvider");
            }
            return strategy.Generate();
        }

        #endregion

        /// <summary>
        /// Creates a new <see cref="InstanceGenerator.GeneratorStrategy" /> instance that
        /// implements the behavior of the derived type.
        /// </summary>
        /// <param name="attributeProvider">
        /// A <see cref="ICustomAttributeProvider" /> instance.
        /// </param>
        /// <returns>
        /// A <see cref="InstanceGenerator.GeneratorStrategy"/> that implements the behavior of the
        /// derived type.
        /// </returns>
        protected abstract GeneratorStrategy CreateStrategy(ICustomAttributeProvider attributeProvider);

        /// <summary>
        /// A strategy that implements the behavior for a type deriving from
        /// <see cref="InstanceGenerator"/>.
        /// </summary>
        protected abstract class GeneratorStrategy
        {
            private readonly IInstanceGenerator parent;
            private readonly ICustomAttributeProvider attributeProvider;

            /// <summary>
            /// Initializes a new instance of the <see cref="GeneratorStrategy"/> class with the
            /// provided parent and attribute provider.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="attributeProvider">A description of the requested instance.</param>
            protected GeneratorStrategy(IInstanceGenerator parent, ICustomAttributeProvider attributeProvider)
            {
                if (parent == null)
                {
                    throw new ArgumentNullException("parent");
                }

                this.parent = parent;
                this.attributeProvider = attributeProvider;
            }

            /// <summary>
            /// Indicates whether the current instance can generate an object instance.
            /// </summary>
            /// <returns>
            /// <see langword="true"/> if the current instance can generate object instances;
            /// otherwise, <see langword="false"/>.
            /// </returns>
            public abstract bool CanGenerate();

            /// <summary>
            /// Generates a new object instance.
            /// </summary>
            /// <returns>A new object instance.</returns>
            public abstract object Generate();

            /// <summary>
            /// Gets the description of the requested instance.
            /// </summary>
            protected ICustomAttributeProvider AttributeProvider
            {
                get { return this.attributeProvider; }
            }

            /// <summary>
            /// Gets the parent.
            /// </summary>
            protected IInstanceGenerator Parent
            {
                get { return this.parent; }
            }
        }
    }
}
