using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Satisfies a request for <see cref="ImmutableDictionary{TKey, TValue}"/> by wrapping a <see cref="Dicrionary{TKey, TValue}"/>
    /// with an implementation of <see cref="ImmutableDictionary{TKey, TValue}"/>.
    /// </summary>
    public class ImmutableDictionaryRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates an implementation of <see cref="ImmutableDictionary{TKey, TValue}"/> wrapping <see cref="Dicrionary{TKey, TValue}"/>
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A finite sequence of the requested type if possible; otherwise a <see cref="NoSpecimen"/>
        /// instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var type = request as Type;
            if (type == null) return new NoSpecimen();
            var typeArguments = type.GetTypeInfo().GetGenericArguments();
            if (typeArguments.Length != 2) return new NoSpecimen();
            var gtd = type.GetGenericTypeDefinition();
            if (gtd != typeof(ImmutableDictionary<,>)) return new NoSpecimen();
            var dictionary = context.Resolve(typeof(Dictionary<,>).MakeGenericType(typeArguments));

            var collection = typeof(ImmutableDictionaryRelay)
                .GetMethod(nameof(Create), BindingFlags.Static | BindingFlags.NonPublic)
                .MakeGenericMethod(typeArguments)
                .Invoke(null, new[] { dictionary });

            return collection;
        }

        private static ImmutableDictionary<TKey, TValue> Create<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            return dictionary.ToImmutableDictionary(x => x.Key, x => x.Value);
        }
    }
}
