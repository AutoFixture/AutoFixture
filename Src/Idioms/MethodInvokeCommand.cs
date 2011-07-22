using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    public class MethodInvokeCommand : IGuardClauseCommand
    {
        private readonly IMethod method;
        private readonly IExpansion<object> expansion;
        private readonly ParameterInfo parameterInfo;

        public MethodInvokeCommand(IMethod method, IExpansion<object> expansion, ParameterInfo parameterInfo)
        {
            this.method = method;
            this.expansion = expansion;
            this.parameterInfo = parameterInfo;
        }

        public IMethod Method
        {
            get { return this.method; }
        }

        public IExpansion<object> Expansion
        {
            get { return this.expansion; }
        }

        public ParameterInfo ParameterInfo
        {
            get { return this.parameterInfo; }
        }

        #region IGuardClauseCommand Members

        public Type RequestedType
        {
            get { return this.ParameterInfo.ParameterType; }
        }

        public void Execute(object value)
        {
            this.method.Invoke(this.expansion.Expand(value));
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
                "An attempt was made to assign the value {0} to the parameter \"{1}\" of the method \"{2}\", and no Guard Clause prevented this. Are you missing a Guard Clause?{7}Method Signature: {3}{7}Parameter Type: {4}{7}Declaring Type: {5}{7}Reflected Type: {6}",
                value,
                this.ParameterInfo.Name,
                this.ParameterInfo.Member.Name,
                this.ParameterInfo.Member,
                this.ParameterInfo.ParameterType.AssemblyQualifiedName,
                this.ParameterInfo.Member.DeclaringType.AssemblyQualifiedName,
                this.ParameterInfo.Member.ReflectedType.AssemblyQualifiedName,
                Environment.NewLine);
        }
    }
}
