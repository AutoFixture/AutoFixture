using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.DataAnnotations
{
    /// <summary>
    /// Handles a request for a string that matches an Enum data type.
    /// </summary>
    public class EnumDataTypeAttributeRelay : ISpecimenBuilder
    {
        private readonly object syncLock = new object();
        private readonly Dictionary<Type, IEnumerator> enumerators = new Dictionary<Type, IEnumerator>();
        private IRequestMemberTypeResolver requestMemberTypeResolver = new RequestMemberTypeResolver();

        /// <summary>
        /// Gets or sets the current IRequestMemberTypeResolver.
        /// </summary>
        public IRequestMemberTypeResolver RequestMemberTypeResolver
        {
            get => this.requestMemberTypeResolver;
            set => this.requestMemberTypeResolver = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
            {
                return new NoSpecimen();
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var enumDataTypeAttribute = TypeEnvy.GetAttribute<EnumDataTypeAttribute>(request);
            if (enumDataTypeAttribute == null)
            {
                return new NoSpecimen();
            }

            if (!this.RequestMemberTypeResolver.TryGetMemberType(request, out var memberType))
            {
                return new NoSpecimen();
            }

            if (!enumDataTypeAttribute.EnumType.GetTypeInfo().IsEnum)
                return new NoSpecimen();

            if (memberType == typeof(string))
            {
                lock (this.syncLock)
                {
                    return this.CreateValue(enumDataTypeAttribute.EnumType).ToString();
                }
            }

            return new NoSpecimen();
        }

        /// <summary>
        /// The code below was copied from the <see cref="EnumGenerator"/>.
        /// </summary>
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
