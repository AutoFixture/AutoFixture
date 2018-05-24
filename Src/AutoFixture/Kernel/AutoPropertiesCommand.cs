using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
#pragma warning disable SA1402 // File may only contain a single type
    /// <summary>
    /// A command that assigns anonymous values to all writable properties and fields of a type.
    /// </summary>
#pragma warning disable 618
    public class AutoPropertiesCommand : AutoPropertiesCommand<object>
#pragma warning restore 618
    {
        /// <summary>
        /// The explicitly specified <see cref="Type"/> that should be used to resolve fields and properties
        /// to populate for the specimen.
        /// <remarks>
        /// <para>
        /// Property will return null if no explicit specimen type was specified during the command construction.
        /// In this case command uses the runtime type of the generated specimen to resolve its fields and properties.
        /// </para>
        /// </remarks>
        /// </summary>
        public Type ExplicitSpecimenType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPropertiesCommand"/> class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When created without an explicit specimen type, the <see cref="AutoPropertiesCommand"/>
        /// will infer the specimen type from the actual specimen instance.
        /// </para>
        /// </remarks>
        public AutoPropertiesCommand()
        {
            this.ExplicitSpecimenType = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPropertiesCommand"/> class with the
        /// supplied specimen type.
        /// </summary>
        /// <param name="specimenType">The specimen type on which properties are assigned.</param>
        public AutoPropertiesCommand(Type specimenType)
        {
            this.ExplicitSpecimenType = specimenType ?? throw new ArgumentNullException(nameof(specimenType));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPropertiesCommand"/> class with the
        /// supplied specification.
        /// </summary>
        /// <param name="specification">
        /// A specification that is used as a filter to include properties or fields.
        /// </param>
        /// <remarks>
        /// <para>
        /// Only properties or fields satisfied by <paramref name="specification"/> will get
        /// assigned values.
        /// </para>
        /// </remarks>
        public AutoPropertiesCommand(IRequestSpecification specification)
            : base(specification)
        {
            this.ExplicitSpecimenType = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPropertiesCommand"/> class with the
        /// supplied specimen type and specification.
        /// </summary>
        /// <param name="specimenType">The specimen type on which properties are assigned.</param>
        /// <param name="specification">
        /// A specification that is used as a filter to include properties or fields.
        /// </param>
        /// <remarks>
        /// <para>
        /// Only properties or fields satisfied by <paramref name="specification"/> will get
        /// assigned values.
        /// </para>
        /// </remarks>
        public AutoPropertiesCommand(Type specimenType, IRequestSpecification specification)
            : base(specification)
        {
            this.ExplicitSpecimenType = specimenType ?? throw new ArgumentNullException(nameof(specimenType));
        }

        /// <inheritdoc />
        protected override Type GetSpecimenType(object specimen)
        {
            if (specimen == null) throw new ArgumentNullException(nameof(specimen));

            return this.ExplicitSpecimenType ?? specimen.GetType();
        }
    }

    /// <summary>
    /// A command that assigns anonymous values to all writable properties and fields of a type.
    /// </summary>
    /// <typeparam name="T">The specimen type on which properties are assigned.</typeparam>
    [Obsolete("The generic version of the AutoPropertiesCommand is no longer used and will be removed in future versions. Please use the non-generic version of the AutoPropertiesCommand type.")]
    public class AutoPropertiesCommand<T> : ISpecimenCommand, ObsoletedMemberShims.ISpecifiedSpecimenCommand<T>
    {
        /// <summary>
        /// Specification that filters properties and files that should be populated.
        /// </summary>
        public IRequestSpecification Specification { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPropertiesCommand{T}"/> class.
        /// </summary>
        public AutoPropertiesCommand()
            : this(new TrueRequestSpecification())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPropertiesCommand{T}"/> class with the
        /// the supplied specification.
        /// </summary>
        /// <param name="specification">
        /// A specification that is used as a filter to include properties or fields.
        /// </param>
        /// <remarks>
        /// <para>
        /// Only properties or fields satisfied by <paramref name="specification"/> will get
        /// assigned values.
        /// </para>
        /// </remarks>
        public AutoPropertiesCommand(IRequestSpecification specification)
        {
            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            this.Specification = specification;
        }

        /// <summary>
        /// Assigns anonymous values to properties and fields on a specimen.
        /// </summary>
        /// <param name="specimen">
        /// The specimen on which property and field values will be assigned.
        /// </param>
        /// <param name="context">
        /// An <see cref="ISpecimenContext"/> that is used to create property and field values.
        /// </param>
        [Obsolete("This method is no longer used and will be removed in future versions. Please use the Execute(object, ISpecimenContext) overload instead.")]
        public void Execute(T specimen, ISpecimenContext context)
        {
            if (specimen == null)
            {
                throw new ArgumentNullException(nameof(specimen));
            }
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            foreach (var pi in this.GetProperties(specimen))
            {
                var propertyValue = context.Resolve(pi);
                if (!(propertyValue is OmitSpecimen))
                    pi.SetValue(specimen, propertyValue, null);
            }

            foreach (var fi in this.GetFields(specimen))
            {
                var fieldValue = context.Resolve(fi);
                if (!(fieldValue is OmitSpecimen))
                    fi.SetValue(specimen, fieldValue);
            }
        }

        /// <summary>
        /// Evaluates whether a request matches a property or field affected by this command.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a <see cref="PropertyInfo"/>
        /// or <see cref="FieldInfo"/> that identifies a property or field affected by this
        /// <see cref="AutoPropertiesCommand{T}"/>; otherwise, <see langword="false"/>.
        /// </returns>
        [Obsolete("This method is no longer used and will be removed in future versions. Please use the this.Specification.IsSpecifiedBy(request) method instead.")]
        public bool IsSatisfiedBy(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (this.GetProperties(request).Any(pi => pi.Equals(request)))
            {
                return true;
            }

            if (this.GetFields(request).Any(fi => fi.Equals(request)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the type of the specimen.
        /// </summary>
        /// <param name="specimen">The specimen.</param>
        /// <returns>The type of the specimen.</returns>
        /// <remarks>
        /// <para>
        /// This implementation ignores <paramref name="specimen"/> and returns the type parameter
        /// of <see cref="AutoPropertiesCommand{T}"/>.
        /// </para>
        /// </remarks>
        protected virtual Type GetSpecimenType(object specimen)
        {
            return typeof(T);
        }

        private IEnumerable<FieldInfo> GetFields(object specimen)
        {
            return from fi in this.GetSpecimenType(specimen).GetTypeInfo().GetFields(BindingFlags.Public | BindingFlags.Instance)
                   where !fi.IsInitOnly
                   && this.Specification.IsSatisfiedBy(fi)
                   select fi;
        }

        private IEnumerable<PropertyInfo> GetProperties(object specimen)
        {
            return from pi in this.GetSpecimenType(specimen).GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   where pi.GetSetMethod() != null
                   && pi.GetIndexParameters().Length == 0
                   && this.Specification.IsSatisfiedBy(pi)
                   select pi;
        }

        /// <summary>
        /// Assigns anonymous values to properties and fields on a specimen.
        /// </summary>
        public void Execute(object specimen, ISpecimenContext context)
        {
            if (specimen == null)
                throw new ArgumentNullException(nameof(specimen));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            foreach (var pi in this.GetProperties(specimen))
            {
                var propertyValue = context.Resolve(pi);
                if (!(propertyValue is OmitSpecimen))
                    pi.SetValue(specimen, propertyValue, null);
            }

            foreach (var fi in this.GetFields(specimen))
            {
                var fieldValue = context.Resolve(fi);
                if (!(fieldValue is OmitSpecimen))
                    fi.SetValue(specimen, fieldValue);
            }
        }
    }
#pragma warning restore SA1402 // File may only contain a single type
}
