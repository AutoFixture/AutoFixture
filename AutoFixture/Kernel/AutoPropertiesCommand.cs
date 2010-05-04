using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// A command that assigns anonymous values to all writable properties and fields of a type.
    /// </summary>
    public class AutoPropertiesCommand : AutoPropertiesCommand<object>
    {
        private readonly Type specimenType;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPropertiesCommand"/> class with the
        /// supplied specimen type.
        /// </summary>
        /// <param name="specimenType">The specimen type on which properties are assigned.</param>
        public AutoPropertiesCommand(Type specimenType)
        {
            if (specimenType == null)
            {
                throw new ArgumentNullException("specimenType");
            }

            this.specimenType = specimenType;
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
            this.specimenType = specimenType;
        }

        /// <summary>
        /// Gets the type of the specimen.
        /// </summary>
        protected override Type SpecimenType
        {
            get { return this.specimenType; }
        }
    }

    /// <summary>
    /// A command that assigns anonymous values to all writable properties and fields of a type.
    /// </summary>
    /// <typeparam name="T">The specimen type on which properties are assigned.</typeparam>
    public class AutoPropertiesCommand<T> : ISpecifiedSpecimenCommand<T>
    {
        private readonly IRequestSpecification specification;

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
                throw new ArgumentNullException("specification");
            }

            this.specification = specification;
        }

        #region ISpecifiedSpecimenCommand<T> Members

        /// <summary>
        /// Assigns anonymous values to properties and fields on a specimen
        /// </summary>
        /// <param name="specimen">
        /// The specimen on which property and field values will be assigned.
        /// </param>
        /// <param name="container">
        /// An <see cref="ISpecimenContainer"/> that is used to create property and field values.
        /// </param>
        public void Execute(T specimen, ISpecimenContainer container)
        {
            if (specimen == null)
            {
                throw new ArgumentNullException("specimen");
            }
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            foreach (var pi in this.GetProperties())
            {
                var propertyValue = container.Create(pi);
                pi.SetValue(specimen, propertyValue, null);
            }

            foreach (var fi in this.GetFields())
            {
                var fieldValue = container.Create(fi);
                fi.SetValue(specimen, fieldValue);
            }
        }

        #endregion

        #region IRequestSpecification Members

        /// <summary>
        /// Evaluates whether a request matches a property or field affected by this command.
        /// </summary>
        /// <param name="request">The specimen request.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="request"/> is a <see cref="PropertyInfo"/>
        /// or <see cref="FieldInfo"/> that identifies a property or field affected by this
        /// <see cref="AutoPropertiesCommand{T}"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsSatisfiedBy(object request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (this.GetProperties().Any(pi => pi.Equals(request)))
            {
                return true;
            }

            if (this.GetFields().Any(fi => fi.Equals(request)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the type of the specimen.
        /// </summary>
        protected virtual Type SpecimenType
        {
            get { return typeof(T); }
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            return from fi in this.SpecimenType.GetFields()
                   where this.specification.IsSatisfiedBy(fi)
                   select fi;
        }

        private IEnumerable<PropertyInfo> GetProperties()
        {
            return from pi in this.SpecimenType.GetProperties()
                   where pi.GetSetMethod() != null
                   && pi.GetIndexParameters().Length == 0
                   && this.specification.IsSatisfiedBy(pi)
                   select pi;
        }

        #endregion
    }
}
