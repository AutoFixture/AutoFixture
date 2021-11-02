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
    public class ReadonlyCollectionPropertiesCommand : ISpecimenCommand
    {
        /// <summary>
        /// Constructs an instance of <see cref="ReadonlyCollectionPropertiesCommand"/>, used to fill all readonly
        /// properties in a specimen that implement <see cref="ICollection{T}"/>.
        /// </summary>
        public ReadonlyCollectionPropertiesCommand()
            : this(ReadonlyCollectionPropertiesSpecification.DefaultPropertyQuery)
        {
        }

        /// <summary>
        /// Constructs an instance of <see cref="ReadonlyCollectionPropertiesCommand"/>, used to fill all readonly
        /// properties in a specimen that implement <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="propertyQuery">The query that will be applied to select readonly collection properties.</param>
        public ReadonlyCollectionPropertiesCommand(IPropertyQuery propertyQuery)
        {
            this.PropertyQuery = propertyQuery;
        }

        /// <summary>
        /// Gets the query used to determine whether or not a specified type has readonly collection properties.
        /// </summary>
        public IPropertyQuery PropertyQuery { get; }

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
            foreach (var pi in this.PropertyQuery.SelectProperties(specimenType))
            {
                var addMethod = new InstanceMethodQuery(pi.GetValue(specimen), nameof(ICollection<object>.Add))
                    .SelectMethods()
                    .SingleOrDefault();
                if (addMethod == null) continue;

                var valuesToAdd = SpecimenFactory.CreateMany(
                    context,
                    addMethod.Parameters.Single().ParameterType);

                foreach (var valueToAdd in valuesToAdd)
                {
                    try
                    {
                        addMethod.Invoke(new[] { valueToAdd });
                    }
                    catch (TargetInvocationException e)
                    {
                        if (e.InnerException?.GetType() == typeof(NotSupportedException)) break;
                        throw;
                    }
                }
            }
        }
    }
}