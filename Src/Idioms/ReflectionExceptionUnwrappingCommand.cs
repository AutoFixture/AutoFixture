using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public class ReflectionExceptionUnwrappingCommand : IContextualCommand
    {
        private IContextualCommand command;

        public ReflectionExceptionUnwrappingCommand(IContextualCommand command)
        {
            this.command = command;
        }

        public IContextualCommand Command
        {
            get { return this.command; }
        }

        #region IContextualCommand Members

        public Type ContextType
        {
            get { return this.command.ContextType; }
        }

        public void Execute()
        {
            try
            {
                this.Command.Execute();
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        #endregion
    }
}
