using System;
using System.Globalization;
using System.Reflection;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Assigns a value to a property.
    /// </summary>
    public class PropertySetCommand : IGuardClauseCommand
    {
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
            this.PropertyInfo = propertyInfo;
            this.Owner = owner;
        }

        /// <summary>
        /// Gets the owner supplied via the constructor.
        /// </summary>
        public object Owner { get; }

        /// <summary>
        /// Gets the property supplied via the constructor.
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets the type of the requested value.
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// The RequestedType property identifies the type of object which should be supplied to
        /// the <see cref="Execute"/> method - in this case the type of the
        /// <see cref="PropertyInfo" />.
        /// </remarks>
        public Type RequestedType => this.PropertyInfo.PropertyType;

        /// <summary>
        /// Gets the parameter name of the requested value.
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// The RequestedParameterName always returns "value".
        /// </remarks>
        public string RequestedParameterName => "value";

        /// <inheritdoc />
        /// <remarks>
        /// <para>
        /// Assigns <paramref name="value" /> to the <see cref="AutoFixture.Idioms.PropertySetCommand.PropertyInfo" />.
        /// </para>
        /// </remarks>
        public void Execute(object value)
        {
            this.PropertyInfo.SetValue(this.Owner, value, null);
        }

        /// <inheritdoc />
        public Exception CreateException(string value)
        {
            return new GuardClauseException(this.CreateExceptionMessage(value));
        }

        /// <inheritdoc />
        public Exception CreateException(string value, Exception innerException)
        {
            return new GuardClauseException(this.CreateExceptionMessage(value), innerException);
        }

        /// <inheritdoc />
        public Exception CreateException(string value, string customError, Exception innerException)
        {
            return new GuardClauseException(this.CreateExceptionMessage(value, customError), innerException);
        }

        private string CreateExceptionMessage(string value,
            string failureReason = "no Guard Clause prevented this. Are you missing a Guard Clause?")
        {
            return string.Format(CultureInfo.CurrentCulture,
                "An attempt was made to assign the value {1} to the property {3}, and {2}{0}" +
                "Property Type: {4}{0}Declaring Type: {5}{0}Reflected Type: {6}",
                Environment.NewLine,
                value,
                failureReason,
                this.PropertyInfo.Name,
                this.PropertyInfo.PropertyType.AssemblyQualifiedName,
                this.PropertyInfo.DeclaringType.AssemblyQualifiedName,
                this.PropertyInfo.ReflectedType.AssemblyQualifiedName);
        }
    }
}