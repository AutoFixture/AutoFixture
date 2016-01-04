using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Aggregates a set of <see cref="ISpecimenCommand"/>.
    /// Executing the aggregate will trigger the execution of the parts.
    /// </summary>
    public class CompositeSpecimenCommand : ISpecimenCommand
    {
        private readonly ISpecimenCommand[] commands;

        /// <summary>
        /// Initializes a new instance of <see cref="CompositeSpecimenCommand"/> class with the supplied set of commands.
        /// </summary>
        /// <param name="commands">The child commands.</param>
        public CompositeSpecimenCommand(IEnumerable<ISpecimenCommand> commands)
            : this(commands.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CompositeSpecimenCommand"/> class with the supplied set of commands.
        /// </summary>
        /// <param name="commands">The child commands.</param>
        public CompositeSpecimenCommand(params ISpecimenCommand[] commands)
        {
            if (commands == null) throw new ArgumentNullException(nameof(commands));

            this.commands = commands;
        }

        /// <summary>
        /// Gets the child commands.
        /// </summary>
        public IEnumerable<ISpecimenCommand> Commands => commands;

        /// <summary>
        /// Executes all child commands using a given specimen and context.
        /// </summary>
        /// <param name="specimen">The specimen on which the child commands will be executed.</param>
        /// <param name="context">The context of <paramref name="specimen"/>.</param>
        public void Execute(object specimen, ISpecimenContext context)
        {
            foreach (var command in commands)
                command.Execute(specimen, context);
        }
    }
}
