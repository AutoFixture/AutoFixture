using System;
using FakeItEasy;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoFakeItEasy2
{
    /// <summary>
    /// Relays a request for an interface or an abstract class to a request for a
    /// <see cref="Fake{T}"/> of that class.
    /// </summary>
    [Obsolete("The AutoFakeItEasy2 package has been retired; use the AutoFakeItEasy (without the trailing \"2\") package instead. Details: it's turned out that it's possible to enable AutoFakeItEasy to also work with FakeItEasy 2. From version 3.49.1, you should be able to use AutoFakeItEasy with FakeItEasy 2 by adding an assembly binding redirect. This enables us, the maintainers of AutoFixture, to maintain only one code base for FakeItEasy, instead of two. If this causes problems, please create an issue at https://github.com/AutoFixture/AutoFixture/issues. We apologise for any inconvenience this may cause.", true)]
    public class FakeItEasyRelay : ISpecimenBuilder
    {
        private readonly IRequestSpecification fakeableSpecification;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeItEasyRelay"/> class.
        /// </summary>
        public FakeItEasyRelay()
            : this(new IsFakeableSpecification())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeItEasyRelay"/> class with a specification
        /// that determines whether a type should be mocked.
        /// </summary>
        /// <param name="fakeableSpecification">
        /// A specification that determines whether a type should be mocked or not.
        /// </param>
        public FakeItEasyRelay(IRequestSpecification fakeableSpecification)
        {
            if (fakeableSpecification == null)
                throw new ArgumentNullException("fakeableSpecification");

            this.fakeableSpecification = fakeableSpecification;
        }

        /// <summary>
        /// Gets a specification that determines whether a given type should be mocked.
        /// </summary>
        /// <value>The specification.</value>
        /// <remarks>
        /// <para>
        /// This specification determins whether a given type should be relayed as a request for a
        /// mock of the same type. By default it only returns <see langword="true"/> for interfaces
        /// and abstract classes, but a different specification can be supplied by using the
        /// overloaded constructor that takes an
        /// <see cref="IRequestSpecification" /> as input. In that case, this
        /// property returns the specification supplied to the constructor.
        /// </para>
        /// </remarks>
        /// <seealso cref="FakeItEasyRelay(IRequestSpecification)"/>
        public IRequestSpecification FakeableSpecification
        {
            get { return this.fakeableSpecification; }
        }

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A dynamic mock instance of the requested interface or abstract class if possible;
        /// otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (!this.fakeableSpecification.IsSatisfiedBy(request))
                return new NoSpecimen();

            var type = request as Type;
            if (type == null)
                return new NoSpecimen();

            var fakeType = typeof(Fake<>).MakeGenericType(type);

            var fake = context.Resolve(fakeType);
            if (!fakeType.IsInstanceOfType(fake))
            {
                return new NoSpecimen();
            }

            return fake.GetType().GetProperty("FakedObject").GetValue(fake, null);
        }

        private class IsFakeableSpecification : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request)
            {
                var t = request as Type;
                return (t != null) && (t.IsAbstract || t.IsInterface);
            }
        }
    }
}
