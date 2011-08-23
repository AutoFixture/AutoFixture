using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Contains methods for populating dictionaries with specimens.
    /// </summary>
    public static class DictionaryFiller
    {
        /// <summary>
        /// Adds many items to a dictionary.
        /// </summary>
        /// <param name="specimen">The dictionary to which items should be added.</param>
        /// <param name="context">The context which can be used to resolve other specimens.</param>
        /// <remarks>
        /// <para>
        /// This method mainly exists to support AutoFixture's infrastructure code (particularly
        /// <see cref="MultipleCustomization" /> and is not intended for use in user code.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// <paramref name="specimen"/> is not an instance of <see cref="IDictionary{TKey, TValue}" />.
        /// </exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void AddMany(object specimen, ISpecimenContext context)
        {
            if (specimen == null)
            {
                throw new ArgumentNullException("specimen");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var typeArguments = specimen.GetType().GetGenericArguments();
            if (typeArguments.Length != 2)
            {
                throw new ArgumentException("The specimen must be an instance of IDictionary<TKey, TValue>.", "specimen");
            }

            if (!typeof(IDictionary<,>).MakeGenericType(typeArguments).IsAssignableFrom(specimen.GetType()))
            {
                throw new ArgumentException("The specimen must be an instance of IDictionary<TKey, TValue>.", "specimen");
            }

            var kvpType = typeof(KeyValuePair<,>).MakeGenericType(typeArguments);
            var enumerable = context.Resolve(new MultipleRequest(kvpType)) as IEnumerable;
            foreach (var item in enumerable)
            {
                var addMethod = typeof(ICollection<>).MakeGenericType(kvpType).GetMethod("Add", new[] { kvpType });
                addMethod.Invoke(specimen, new[] { item });
            }
        }
    }
}
