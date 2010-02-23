using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Default reusable implementation of <see cref="IInstanceGenerator" />
    /// </summary>
    public class InstanceGeneratorNode : IInstanceGenerator
    {
        private readonly IInstanceGenerator parent;

        /// <summary>
        /// Initializes a new instance of <see cref="InstanceGeneratorNode" /> with the provided
        /// parent.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public InstanceGeneratorNode(IInstanceGenerator parent)
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
        /// <param name="request">
        /// Provides a description upon which the current instance can base its decision.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the current instance can generate object instances based on
        /// <paramref name="request"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool CanGenerate(ICustomAttributeProvider request)
        {
            return this.CreateStrategy(request).CanGenerate();
        }

        /// <summary>
        /// Generates object instances based on the given <see cref="ICustomAttributeProvider"/>.
        /// </summary>
        /// <param name="request">
        /// Provides a description that guides the current instance in generating object instances.
        /// </param>
        /// <returns>A new object based on <paramref name="request"/>.</returns>
        public object Generate(ICustomAttributeProvider request)
        {
            var strategy = this.CreateStrategy(request);
            if (!strategy.CanGenerate())
            {
                throw new ArgumentException("Cannot generate an instance for the request.", "request");
            }
            return strategy.Generate();
        }

        #endregion

        /// <summary>
        /// Creates a new <see cref="InstanceGeneratorNode.GeneratorStrategy" /> instance that
        /// implements the behavior of the derived type.
        /// </summary>
        /// <param name="request">
        /// A <see cref="ICustomAttributeProvider" /> instance.
        /// </param>
        /// <returns>
        /// A <see cref="InstanceGeneratorNode.GeneratorStrategy"/> that implements the behavior of the
        /// derived type.
        /// </returns>
        protected virtual GeneratorStrategy CreateStrategy(ICustomAttributeProvider request)
        {
            return new ParentGeneratorStrategy(this.Parent, request);
        }

        /// <summary>
        /// A strategy that implements the behavior for a type deriving from
        /// <see cref="InstanceGeneratorNode"/>.
        /// </summary>
        protected abstract class GeneratorStrategy
        {
            private readonly IInstanceGenerator parent;
            private readonly ICustomAttributeProvider request;

            /// <summary>
            /// Initializes a new instance of the <see cref="GeneratorStrategy"/> class with the
            /// provided parent and attribute provider.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="request">A description of the requested instance.</param>
            protected GeneratorStrategy(IInstanceGenerator parent, ICustomAttributeProvider request)
            {
                if (parent == null)
                {
                    throw new ArgumentNullException("parent");
                }

                this.parent = parent;
                this.request = request;
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
                get { return this.request; }
            }

            /// <summary>
            /// Gets the parent.
            /// </summary>
            protected IInstanceGenerator Parent
            {
                get { return this.parent; }
            }
        }

        private class ParentGeneratorStrategy : GeneratorStrategy
        {
            internal ParentGeneratorStrategy(IInstanceGenerator parent, ICustomAttributeProvider request)
                : base(parent, request)
            {
            }

            public override bool CanGenerate()
            {
                return this.Parent.CanGenerate(this.AttributeProvider);
            }

            public override object Generate()
            {
                return this.Parent.Generate(this.AttributeProvider);
            }
        }

    }
}
