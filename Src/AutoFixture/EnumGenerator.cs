﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
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
            var t = request as Type;
            if (!EnumGenerator.IsEnumType(t))
            {
#pragma warning disable 618
                return new NoSpecimen(request);
#pragma warning restore 618
            }

            lock (this.syncRoot)
            {
                return this.CreateValue(t);
            }
        }

        private static bool IsEnumType(Type t)
        {
            return (t != null) && t.IsEnum;
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
