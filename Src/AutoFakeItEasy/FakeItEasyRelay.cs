﻿using System;
using System.Reflection;
using AutoFixture.Kernel;
using FakeItEasy;

namespace AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// Relays a request for an interface or an abstract class to a request for a
    /// <see cref="Fake{T}"/> of that class.
    /// </summary>
    public class FakeItEasyRelay : ISpecimenBuilder
    {
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
            this.FakeableSpecification = fakeableSpecification ?? throw new ArgumentNullException(nameof(fakeableSpecification));
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
        public IRequestSpecification FakeableSpecification { get; }

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
            if (context is null) throw new ArgumentNullException(nameof(context));

            if (!this.FakeableSpecification.IsSatisfiedBy(request))
                return new NoSpecimen();

            if (request is not Type type)
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
                if (request is not Type type)
                {
                    return false;
                }

                var typeInfo = type.GetTypeInfo();
                return typeInfo.IsAbstract || typeInfo.IsInterface;
            }
        }
    }
}
