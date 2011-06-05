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

        public Type ValueType
        {
            get { return this.command.ValueType; }
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

        public Exception CreateException(string value)
        {
            return this.Command.CreateException(value);
        }

        public Exception CreateException(string value, Exception innerException)
        {
            return this.Command.CreateException(value, innerException);
        }

        #endregion
    }
}
