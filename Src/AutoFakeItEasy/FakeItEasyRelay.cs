using System;
using FakeItEasy;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// Relays a request for an interface or an abstract class to a request for a
    /// <see cref="Fake{T}"/> of that class.
    /// </summary>
    public class FakeItEasyRelay : ISpecimenBuilder
    {
        private readonly Func<Type, bool> shouldBeFaked;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeItEasyRelay"/> class.
        /// </summary>
        public FakeItEasyRelay()
            : this(FakeItEasyRelay.ShouldBeFaked)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeItEasyRelay"/> class with a specification
        /// that determines whether a type should be mocked.
        /// </summary>
        /// <param name="fakeableSpecification">
        /// A specification that determines whether a type should be mocked or not.
        /// </param>
        public FakeItEasyRelay(Func<Type, bool> fakeableSpecification)
        {
            if (fakeableSpecification == null)
            {
                throw new ArgumentNullException("fakeableSpecification");
            }

            this.shouldBeFaked = fakeableSpecification;
        }

        /// <summary>
        /// Gets a specification that determines whether a given type should be faked.
        /// </summary>
        /// <value>The specification.</value>
        /// <remarks>
        /// <para>
        /// This specification determines whether a given type should be relayed as a request for a
        /// fake of the same type. By default it only returns <see langword="true"/> for interfaces
        /// and abstract classes, but a different specification can be supplied by using the
        /// <see cref="FakeItEasyRelay(Func{Type, bool})"/> overloaded constructor that takes a
        /// specification as input. In that case, this property returns the specification supplied
        /// to the constructor.
        /// </para>
        /// </remarks>
        /// <seealso cref="FakeItEasyRelay(Func{Type, bool})"/>
        public Func<Type, bool> FakeableSpecification
        {
            get { return this.shouldBeFaked; }
        }

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A dynamic fake instance of the requested interface or abstract class if possible;
        /// otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var type = request as Type;
            if (!this.shouldBeFaked(type))
            {
                return new NoSpecimen(request);
            }

            var fake = FakeItEasyRelay.ResolveFake(type, context);
            if (fake == null)
            {
                return new NoSpecimen(request);
            }

            return fake;
        }

        private static bool ShouldBeFaked(Type t)
        {
            return (t != null) 
               && ((t.IsAbstract) || (t.IsInterface));
        }

        private static object ResolveFake(Type t, ISpecimenContext context)
        {
            var fakeType = typeof(Fake<>).MakeGenericType(t);
            var specimen = context.Resolve(fakeType);

            var specimenType = specimen.GetType();
            if (specimenType.BaseType.IsGenericType)
            {
                return specimen.GetType().GetProperty("FakedObject").GetValue(specimen, null);
            }
            
            return specimen.GetType().BaseType == t ? specimen : null;
        }
    }
}
