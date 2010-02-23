using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Creates new instances by asking the first able child to create the instance.
    /// </summary>
    public class CoalescingInstanceGenerator : IInstanceGenerator
    {
        private readonly List<IInstanceGenerator> generators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoalescingInstanceGenerator"/> class.
        /// </summary>
        public CoalescingInstanceGenerator()
        {
            this.generators = new List<IInstanceGenerator>();
        }

        /// <summary>
        /// Gets the child generators.
        /// </summary>
        public IList<IInstanceGenerator> Generators
        {
            get { return this.generators; }
        }

        #region IInstanceGenerator Members

        /// <summary>
        /// Asks each of its child <see cref="Generators"/> whether they can handle the current
        /// request.
        /// </summary>
        /// <param name="request">
        /// Provides a description upon which the children can base their decision.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if any child can handle the request; otherwise, <see langword="false"/>.
        /// </returns>
        public bool CanGenerate(ICustomAttributeProvider request)
        {
            return new CoalescingGeneratorStrategy(this.generators, request).CanGenerate();
        }

        /// <summary>
        /// Asks the first able child to generate the requested instance.
        /// </summary>
        /// <param name="request">
        /// Provides a description that guides the able child in generating object instances.
        /// </param>
        /// <returns>A new object based on <paramref name="request"/>.</returns>
        public object Generate(ICustomAttributeProvider request)
        {
            var strategy = new CoalescingGeneratorStrategy(this.generators, request);
            if (!strategy.CanGenerate())
            {
                throw new ArgumentException("Cannot generate an instance for the request.", "request");
            }
            return strategy.Generate();
        }

        #endregion

        private class CoalescingGeneratorStrategy
        {
            private readonly IInstanceGenerator ableGenerator;
            private readonly ICustomAttributeProvider request;

            internal CoalescingGeneratorStrategy(IEnumerable<IInstanceGenerator> generators, ICustomAttributeProvider request)
            {
                this.ableGenerator = generators.FirstOrDefault(g => g.CanGenerate(request));
                this.request = request;
            }

            internal bool CanGenerate()
            {
                return this.ableGenerator != null;
            }

            internal object Generate()
            {
                return this.ableGenerator.Generate(this.request);
            }
        }
    }
}
