using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Contains methods for populating dictionaries with specimens.
    /// </summary>
    public class DictionaryFiller : ISpecimenCommand
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
        [Obsolete("Use the instance method Execute instead.", true)]
        public static void AddMany(object specimen, ISpecimenContext context)
        {
            new DictionaryFiller().Execute(specimen, context);
        }

        /// <summary>
        /// Adds many items to a dictionary.
        /// </summary>
        /// <param name="specimen">The dictionary to which items should be added.</param>
        /// <param name="context">The context which can be used to resolve other specimens.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="specimen"/> is not an instance of <see cref="IDictionary{TKey, TValue}" />.
        /// </exception>
        public void Execute(object specimen, ISpecimenContext context)
        {
            if (specimen == null)
                throw new ArgumentNullException(nameof(specimen));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var typeArguments = specimen.GetType().GetTypeInfo().GetGenericArguments();
            if (typeArguments.Length != 2)
                throw new ArgumentException("The specimen must be an instance of IDictionary<TKey, TValue>.", nameof(specimen));

            if (!typeof(IDictionary<,>).MakeGenericType(typeArguments).GetTypeInfo().IsAssignableFrom(specimen.GetType()))
                throw new ArgumentException("The specimen must be an instance of IDictionary<TKey, TValue>.", nameof(specimen));

            var filler = (ISpecimenCommand)Activator.CreateInstance(
                typeof(Filler<,>).MakeGenericType(typeArguments));
            filler.Execute(specimen, context);
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
            Justification = "It's activated via reflection.")]
        private class Filler<TKey, TValue> : ISpecimenCommand
        {
            public void Execute(object specimen, ISpecimenContext context)
            {
                Fill((IDictionary<TKey, TValue>)specimen, context);
            }

            private static void Fill(
                IDictionary<TKey, TValue> dictionary,
                ISpecimenContext context)
            {
                foreach (var kvp in GetValues(context))
                {
                    if (!dictionary.ContainsKey(kvp.Key))
                        dictionary.Add(kvp);
                }
            }

            private static IEnumerable<KeyValuePair<TKey, TValue>> GetValues(
                ISpecimenContext context)
            {
                return
                    ((IEnumerable)context.Resolve(
                        new MultipleRequest(typeof(KeyValuePair<TKey, TValue>))))
                    .Cast<KeyValuePair<TKey, TValue>>();
            }
        }
    }
}
