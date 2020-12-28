using System;
using System.Collections;
using System.Collections.Generic;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Generates enum values in a round-robin fashion from a subset of values.
    /// </summary>
    /// <typeparam name="TEnum">The type of enum values to generate.</typeparam>
    public sealed class RestrictedEnumGenerator<TEnum> : ISpecimenBuilder
        where TEnum : struct, Enum
    {
        private readonly IEnumerator enumerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestrictedEnumGenerator{TEnum}"/> class
        /// with the value selection options provided.
        /// </summary>
        /// <param name="values">The subset of values from which to generate values of
        /// <typeparamref name="TEnum"/>.</param>
        public RestrictedEnumGenerator(EnumValueCollection<TEnum> values)
        {
            if (values is null)
                throw new ArgumentNullException(nameof(values));

            this.enumerator = new RoundRobinEnumEnumerable(values).GetEnumerator();
        }

        /// <summary>
        /// Creates a new enum value based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens. Not used.</param>
        /// <returns>An enum value if appropriate; otherwise a <see cref="NoSpecimen"/> instance.</returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is <typeparamref name="TEnum"/>, an instance of that enum is returned.
        /// Differing values are returned, starting with the first value from the subset of enum values.
        /// When all values of the subset have been served, the sequence starts over again.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (request as Type != typeof(TEnum))
                return new NoSpecimen();

            this.enumerator.MoveNext();

            return this.enumerator.Current;
        }

        private class RoundRobinEnumEnumerable : IEnumerable
        {
            private readonly IEnumerable<TEnum> values;

            internal RoundRobinEnumEnumerable(IEnumerable<TEnum> values)
            {
                this.values = values;
            }

            public IEnumerator GetEnumerator()
            {
                while (true)
                {
                    foreach (var obj in this.values)
                    {
                        yield return obj;
                    }
                }
            }
        }
    }
}
