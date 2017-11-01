using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// A customization that uses a particular constructor selection mechanism to pick and invoke
    /// a constructor to create specimens of the targeted type.
    /// </summary>
    public class ConstructorCustomization : ICustomization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorCustomization"/> class.
        /// </summary>
        /// <param name="targetType">
        /// The <see cref="Type"/> for which <paramref name="query"/> should be used to select the
        /// most appropriate constructor.
        /// </param>
        /// <param name="query">
        /// The query that selects a constructor for <paramref name="targetType"/>.
        /// </param>
        public ConstructorCustomization(Type targetType, IMethodQuery query)
        {
            this.TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            this.Query = query ?? throw new ArgumentNullException(nameof(query));
        }

        /// <summary>
        /// Gets the <see cref="Type"/> for which <see cref="Query"/> should be used to select the
        /// most appropriate constructor.
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// Gets the query that selects a constructor for <see cref="TargetType"/>.
        /// </summary>
        public IMethodQuery Query { get; }

        /// <summary>
        /// Customizes the specified fixture by modifying <see cref="TargetType"/> to use
        /// <see cref="Query"/> as the strategy for creating new specimens.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null) throw new ArgumentNullException(nameof(fixture));

            var factory = new MethodInvoker(this.Query);
            var builder = SpecimenBuilderNodeFactory.CreateTypedNode(
                this.TargetType,
                factory);

            fixture.Customizations.Insert(0, builder);
        }
    }
}
