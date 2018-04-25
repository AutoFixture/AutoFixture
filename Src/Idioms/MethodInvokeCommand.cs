using System;
using System.Globalization;
using System.Reflection;
using AutoFixture.Kernel;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Invokes a method.
    /// </summary>
    public class MethodInvokeCommand : IGuardClauseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInvokeCommand"/> class.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <param name="expansion">
        /// An expansion which is used to transform the single value in the Execute method into an
        /// appropriate number of input arguments for the method.
        /// </param>
        /// <param name="parameterInfo">The parameter.</param>
        public MethodInvokeCommand(IMethod method, IExpansion<object> expansion, ParameterInfo parameterInfo)
        {
            this.Method = method;
            this.Expansion = expansion;
            this.ParameterInfo = parameterInfo;
        }

        /// <summary>
        /// Gets the method supplied via the constructor.
        /// </summary>
        public IMethod Method { get; }

        /// <summary>
        /// Gets the expansion supplied via the constructor.
        /// </summary>
        /// <seealso cref="Execute" />
        public IExpansion<object> Expansion { get; }

        /// <summary>
        /// Gets the parameter supplied via the constructor.
        /// </summary>
        public ParameterInfo ParameterInfo { get; }

        /// <summary>
        /// Gets the type of the requested value.
        /// </summary>
        /// <remarks>
        /// The RequestedType property identifies the type of object which should be supplied to
        /// the <see cref="Execute"/> method - in this case the type of the
        /// <see cref="ParameterInfo" />.
        /// </remarks>
        public Type RequestedType => this.ParameterInfo.ParameterType;

        /// <summary>
        /// Gets the parameter name of the requested value.
        /// </summary>
        /// <remarks>
        /// The RequestedParameterName property identifies the parameter name of object which should be supplied to
        /// the <see cref="Execute"/> method - in this case the name of the
        /// <see cref="ParameterInfo" />.
        /// </remarks>
        public string RequestedParameterName => this.ParameterInfo.Name;

        /// <summary>
        /// Invokes the method with the specified value.
        /// </summary>
        /// <param name="value">The value with wich the method is executed.</param>
        /// <remarks>
        /// <para>
        /// Invokes <see cref="Method" /> by expanding the single <paramref name="value" /> with
        /// the <see cref="Expansion" />.
        /// </para>
        /// </remarks>
        public void Execute(object value)
        {
            this.Method.Invoke(this.Expansion.Expand(value));
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
                "An attempt was made to assign the value {1} to the parameter \"{3}\" of the method \"{4}\", and {2}{0}" +
                "Method Signature: {5}{0}Parameter Type: {6}{0}Declaring Type: {7}{0}Reflected Type: {8}",
                Environment.NewLine,
                value,
                failureReason,
                this.ParameterInfo.Name,
                this.ParameterInfo.Member.Name,
                this.ParameterInfo.Member,
                this.ParameterInfo.ParameterType.AssemblyQualifiedName,
                this.ParameterInfo.Member.DeclaringType.AssemblyQualifiedName,
                this.ParameterInfo.Member.ReflectedType.AssemblyQualifiedName);
        }
    }
}
