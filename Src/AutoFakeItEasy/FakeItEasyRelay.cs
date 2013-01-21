﻿using System;
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
        private readonly IRequestSpecification shouldBeFaked;

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
            {
                throw new ArgumentNullException("fakeableSpecification");
            }

            this.shouldBeFaked = fakeableSpecification;
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
            get { return this.shouldBeFaked; }
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
            {
                throw new ArgumentNullException("context");
            }

            if (!this.shouldBeFaked.IsSatisfiedBy(request))
            {
                return new NoSpecimen(request);
            }

            var type = request as Type;
            var fakeType = typeof(Fake<>).MakeGenericType(type);

            var fake = context.Resolve(fakeType) as FakeItEasy.Configuration.IHideObjectMembers;
            if (fake == null)
            {
                return new NoSpecimen(request);
            }

            return fake.GetType().GetProperty("FakedObject").GetValue(fake, null);
        }

        private class IsFakeableSpecification : IRequestSpecification
        {
            public bool IsSatisfiedBy(object request)
            {
                var t = request as Type;
                return (t != null) && ((t.IsAbstract) || (t.IsInterface));
            }
        }
    }
}
