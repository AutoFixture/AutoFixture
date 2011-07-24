using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Assigns a value to a property.
    /// </summary>
    public class PropertySetCommand : IGuardClauseCommand
    {
        private readonly PropertyInfo propertyInfo;
        private readonly object owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySetCommand"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property which should have a value assigned.</param>
        /// <param name="owner">The instance exposing the property.</param>
        /// <remarks>
        /// <para>
        /// Although the constructor doesn't enforce this, the <paramref name="owner" /> is
        /// expected to expose the property identified by <paramref name="propertyInfo" />. If not,
        /// the <see cref="Execute" /> method will fail.
        /// </para>
        /// </remarks>
        public PropertySetCommand(PropertyInfo propertyInfo, object owner)
        {
            this.propertyInfo = propertyInfo;
            this.owner = owner;
        }

        /// <summary>
        /// Gets the owner supplied via the constructor.
        /// </summary>
        public object Owner
        {
            get { return this.owner; }
        }

        /// <summary>
        /// Gets the property supplied via the constructor.
        /// </summary>
        public PropertyInfo PropertyInfo
        {
            get { return this.propertyInfo; }
        }

        #region IContextualCommand Members

        public Type RequestedType
        {
            get { return this.propertyInfo.PropertyType; }
        }

        public void Execute(object value)
        {
            this.propertyInfo.SetValue(this.Owner, value, null);
        }

        public Exception CreateException(string value)
        {
            return new GuardClauseException(this.CreateExceptionMessage(value));
        }

        public Exception CreateException(string value, Exception innerException)
        {
            return new GuardClauseException(this.CreateExceptionMessage(value), innerException);
        }

        #endregion

        private string CreateExceptionMessage(string value)
        {
            return string.Format(
                "An attempt was made to assign the value {0} to the property {1}, and no Guard Clause prevented this. Are you missing a Guard Clause?{5}Property Type: {2}{5}Declaring Type: {3}{5}Reflected Type: {4}",
                value,
                this.PropertyInfo.Name,
                this.PropertyInfo.PropertyType.AssemblyQualifiedName,
                this.PropertyInfo.DeclaringType.AssemblyQualifiedName,
                this.PropertyInfo.ReflectedType.AssemblyQualifiedName,
                Environment.NewLine);
        }
    }
}
