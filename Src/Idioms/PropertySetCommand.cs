using System;
using System.Globalization;
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

        /// <summary>
        /// Gets the type of the requested value.
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// The RequestedType property identifies the type of object which should be supplied to
        /// the <see cref="Execute"/> method - in this case the type of the
        /// <see cref="PropertyInfo" />.
        /// </remarks>
        public Type RequestedType
        {
            get { return this.propertyInfo.PropertyType; }
        }


        /// <summary>
        /// Gets the parameter name of the requested value.
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// The RequestedParameterName always returns "value".
        /// </remarks>
        public string RequestedParameterName
        {
            get { return "value"; }
        }

        /// <summary>
        /// Executes the action with the specified value.
        /// </summary>
        /// <param name="value">The value with wich the action is executed.</param>
        /// <remarks>
        /// <para>
        /// Assigns <paramref name="value" /> to the <see cref="PropertyInfo" />.
        /// </para>
        /// </remarks>
        public void Execute(object value)
        {
            this.propertyInfo.SetValue(this.Owner, value, null);
        }

        /// <summary>
        /// Creates an exception which communicates that an error occured for a specific input
        /// value.
        /// </summary>
        /// <param name="value">A string representation of the value.</param>
        /// <returns>
        /// An exception which communicates the cause of the error.
        /// </returns>
        public Exception CreateException(string value)
        {
            return new GuardClauseException(this.CreateExceptionMessage(value));
        }

        /// <summary>
        /// Creates an exception which communicates that an error occured for a specific input
        /// value.
        /// </summary>
        /// <param name="value">A string representation of the value.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// <returns>
        /// An exception which communicates the cause of the error.
        /// </returns>
        public Exception CreateException(string value, Exception innerException)
        {
            return new GuardClauseException(this.CreateExceptionMessage(value), innerException);
        }

        private string CreateExceptionMessage(string value)
        {
            return string.Format(CultureInfo.CurrentCulture,
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