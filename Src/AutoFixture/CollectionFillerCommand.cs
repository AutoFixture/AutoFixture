using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Ploeh.AutoFixture.Kernel;
using System.Linq;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Contains methods for populating collections with specimens.
    /// </summary>
    public class CollectionFillerCommand : ISpecimenCommand
    {
        /// <summary>
        /// Adds many items to a collection.
        /// </summary>
        /// <param name="specimen">The collection to which items should be added.</param>
        /// <param name="context">The context which can be used to resolve other specimens.</param>
        /// <remarks>
        /// <para>
        /// This method mainly exists to support AutoFixture's infrastructure code (particularly
        /// <see cref="MultipleCustomization" /> and is not intended for use in user code.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// <paramref name="specimen"/> is not an instance of <see cref="ICollection{T}" />.
        /// </exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use the instance method Execute instead.")]
        public static void AddMany(object specimen, ISpecimenContext context)
        {
            new CollectionFillerCommand().Execute(specimen, context);
        }

        /// <summary>
        /// Adds many items to a collection.
        /// </summary>
        /// <param name="specimen">The collection to which items should be added.</param>
        /// <param name="context">The context which can be used to resolve other specimens.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="specimen"/> is not an instance of <see cref="ICollection{T}" />.
        /// </exception>
        public void Execute(object specimen, ISpecimenContext context)
        {
            if (specimen == null)
                throw new ArgumentNullException(nameof(specimen));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var typeArguments = GetInheritedCollectionGenericArguments(specimen.GetType());
            if (typeArguments.Length != 1)
                throw new ArgumentException("The specimen must be an instance of ICollection<T>.", nameof(specimen));

            if (!typeof(ICollection<>).MakeGenericType(typeArguments).IsAssignableFrom(specimen.GetType()))
                throw new ArgumentException("The specimen must be an instance of ICollection<T>.", nameof(specimen));

            var filler = (ISpecimenCommand)Activator.CreateInstance(
                typeof(Filler<>).MakeGenericType(typeArguments));
            filler.Execute(specimen, context);
        }

        private static Type[] GetInheritedCollectionGenericArguments(Type type)
        {
            var collectionInterfaces =
                from i in type.GetInterfaces()
                where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>)
                select i;

            var collectionInterface = collectionInterfaces.FirstOrDefault();
            return collectionInterface == null 
                ? new Type[0] 
                : collectionInterface.GetGenericArguments();
        }

        private class Filler<TValue> : ISpecimenCommand
        {
            public void Execute(object specimen, ISpecimenContext context)
            {
                Fill((ICollection<TValue>)specimen, context);
            }

            private static void Fill(
                ICollection<TValue> collection,
                ISpecimenContext context)
            {
                foreach (var kvp in GetValues(context))
                    collection.Add(kvp);
            }

            private static IEnumerable<TValue> GetValues(
                ISpecimenContext context)
            {
                return
                    ((IEnumerable)context.Resolve(
                        new MultipleRequest(typeof(TValue))))
                    .Cast<TValue>();
            }
        }
    }
}
