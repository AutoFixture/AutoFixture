using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public class ReflectionExceptionUnwrappingCommand : IGuardClauseCommand
    {
        private IGuardClauseCommand command;

        public ReflectionExceptionUnwrappingCommand(IGuardClauseCommand command)
        {
            this.command = command;
        }

        public IGuardClauseCommand Command
        {
            get { return this.command; }
        }

        #region IContextualCommand Members

        public MemberInfo MemberInfo
        {
            get { return this.command.MemberInfo; }
        }

        public Type ContextType
        {
            get { return this.command.ContextType; }
        }

        public void Execute(object value)
        {
            try
            {
                this.Command.Execute(value);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        public Exception Throw(string value)
        {
            return this.Command.Throw(value);
        }

        public void Throw(string value, Exception innerException)
        {
            this.Command.Throw(value, innerException);
        }

        #endregion
    }
}
