using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoFakeItEasy2
{
    /// <summary>
    /// Provides pre- and post-condition checks for requests for fake instances.
    /// </summary>
    /// <seealso cref="Create(object, ISpecimenContext)" />
    [Obsolete("The AutoFakeItEasy2 package has been retired; use the AutoFakeItEasy (without the trailing \"2\") package instead. Details: it's turned out that it's possible to enable AutoFakeItEasy to also work with FakeItEasy 2. From version 3.49.1, you should be able to use AutoFakeItEasy with FakeItEasy 2 by adding an assembly binding redirect. This enables us, the maintainers of AutoFixture, to maintain only one code base for FakeItEasy, instead of two. If this causes problems, please create an issue at https://github.com/AutoFixture/AutoFixture/issues. We apologise for any inconvenience this may cause.", true)]
    public class FakeItEasyBuilder : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeItEasyBuilder"/> class with an
        /// <see cref="ISpecimenBuilder" /> to decorate.
        /// </summary>
        /// <param name="builder">The builder which must build mock instances.</param>
        /// <remarks>
        /// <para>
        /// <paramref name="builder" /> is subsequently available through the <see cref="Builder"/>
        /// property.
        /// </para>
        /// </remarks>
        /// <seealso cref="Builder" />
        public FakeItEasyBuilder(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            this.builder = builder;
        }

        /// <summary>
        /// Gets the decorated builder supplied through the constructor.
        /// </summary>
        /// <seealso cref="FakeItEasyBuilder(ISpecimenBuilder)" />
        public ISpecimenBuilder Builder
        {
            get { return builder; }
        }

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A mock instance created by FakeItEasy if appropriate; otherwise a
        /// <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The Create method checks whether a request is for an interface or abstract class. If so
        /// it delegates the call to the decorated <see cref="Builder"/>. When the specimen is
        /// returned from the decorated <see cref="ISpecimenBuilder"/> the method checks whether
        /// the returned instance is a FakeItEasy Fake instance of the correct type.
        /// </para>
        /// <para>
        /// If all pre- and post-conditions are satisfied, the mock instance is returned; otherwise
        /// a <see cref="NoSpecimen" /> instance.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            var type = request as Type;
            if (!type.IsFake())
            {
                return new NoSpecimen();
            }

            var fake = this.builder.Create(request, context);
            if (!type.IsInstanceOfType(fake))
            {
                return new NoSpecimen();
            }

            return fake;
        }
    }
}