using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// A command which invokes <see cref="ICollection{T}.Add"/> to fill all readonly properties in a specimen that
    /// implement <see cref="ICollection{T}"/>.
    /// </summary>
    public class FillReadonlyCollectionPropertiesCommand : ISpecimenCommand
    {
        /// <summary>
        /// Invokes <see cref="ICollection{T}.Add"/> to fill all readonly properties in a specimen that implement
        /// <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="specimen">
        /// The specimen on which readonly collection properties should be filled.
        /// </param>
        /// <param name="context">
        /// An <see cref="ISpecimenContext"/> that is used to create the elements used to fill collections.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="specimen"/> or <paramref name="context"/> is <see langword="null"/>.
        /// </exception>
        public void Execute(object specimen, ISpecimenContext context)
        {
            if (specimen == null) throw new ArgumentNullException(nameof(specimen));
            if (context == null) throw new ArgumentNullException(nameof(context));

            var specimenType = specimen.GetType();
            foreach (var pi in GetReadonlyCollectionProperties(specimenType))
            {
                var propertyType = pi.PropertyType;
                var collectionTypeGenericArgument = propertyType.GenericTypeArguments[0];

                var addMethod = propertyType.GetTypeInfo().GetMethod(nameof(ICollection<object>.Add));
                if (addMethod == null) continue;

                var valuesToAdd = CreateMany(context, collectionTypeGenericArgument);

                foreach (var valueToAdd in valuesToAdd)
                {
                    addMethod.Invoke(pi.GetValue(specimen), new[] { valueToAdd });
                }
            }
        }

        private static IEnumerable<PropertyInfo> GetReadonlyCollectionProperties(Type type)
        {
            return type.GetTypeInfo()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                .Where(pi => pi.GetSetMethod() == null
                             && pi.PropertyType.GenericTypeArguments?.Length == 1
                             && (pi.PropertyType.Name == typeof(ICollection<>).Name
                                 || pi.PropertyType.GetTypeInfo().GetInterface(typeof(ICollection<>).Name) != null));
        }

        private static IEnumerable<object> CreateMany(ISpecimenContext context, Type type)
        {
            return ((IEnumerable<object>)context.Resolve(
                    new MultipleRequest(new SeededRequest(type, GetDefaultValue(type)))))
                .Select(v => Convert.ChangeType(v, type));
        }

        private static object GetDefaultValue(Type type)
        {
            return type.GetTypeInfo().IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}