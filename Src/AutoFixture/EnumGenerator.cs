using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Generates enum values in a round-robin fashion.
    /// </summary>
    public class EnumGenerator : ISpecimenBuilder
    {
        private readonly Dictionary<Type, IEnumerator> enumerators;
        private readonly object syncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumGenerator"/> class.
        /// </summary>
        public EnumGenerator()
        {
            this.syncRoot = new object();
            this.enumerators = new Dictionary<Type, IEnumerator>();
        }

        /// <summary>
        /// Creates a new enum value based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">
        /// A context that can be used to create other specimens. Not used.
        /// </param>
        /// <returns>
        /// An enum value if appropriate; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a <see cref="Type"/> that represents an enum, an
        /// instance of that enum is returned. Differing values are returned, starting with the
        /// first value. When all values of the enum type have been served, the sequence starts
        /// over again.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            var type = request as Type;
            if (type == null)
                return new NoSpecimen();

            if (!type.GetTypeInfo().IsEnum)
                return new NoSpecimen();

            lock (this.syncRoot)
            {
                return this.CreateValue(type);
            }
        }

        private object CreateValue(Type t)
        {
            var generator = this.EnsureGenerator(t);
            generator.MoveNext();
            return generator.Current;
        }

        private IEnumerator EnsureGenerator(Type t)
        {
            IEnumerator enumerator = null;
            if (!this.enumerators.TryGetValue(t, out enumerator))
            {
                enumerator = new RoundRobinEnumEnumerable(t).GetEnumerator();
                this.enumerators.Add(t, enumerator);
            }
            return enumerator;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1010:Collections should implement generic interface",
            Justification = "This is not a usual enumerable and for our purpose generic interface is not required.")]
        private class RoundRobinEnumEnumerable : IEnumerable
        {
            private readonly IEnumerable<object> values;

            internal RoundRobinEnumEnumerable(Type enumType)
            {
                if (enumType == null)
                {
                    throw new ArgumentNullException(nameof(enumType));
                }

                this.values = Enum.GetValues(enumType).Cast<object>();

                if (!this.values.Any())
                {
                    throw new ObjectCreationException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            "AutoFixture was unable to create a value for {0} since it is an enum containing no values. " +
                            "Please add at least one value to the enum.",
                            enumType.FullName));
                }
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
