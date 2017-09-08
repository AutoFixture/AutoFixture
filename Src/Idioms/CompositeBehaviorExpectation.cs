using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Composes an arbitrary number of <see cref="IBehaviorExpectation" /> instances.
    /// </summary>
    public class CompositeBehaviorExpectation : IBehaviorExpectation
    {
        private readonly IEnumerable<IBehaviorExpectation> expectations;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeBehaviorExpectation"/> class with
        /// the supplied <see cref="IBehaviorExpectation" /> instances.
        /// </summary>
        /// <param name="behaviorExpectations">The encapsulated behavior expectations.</param>
        /// <seealso cref="CompositeBehaviorExpectation(IEnumerable{IBehaviorExpectation})"/>
        /// <seealso cref="BehaviorExpectations" />
        public CompositeBehaviorExpectation(params IBehaviorExpectation[] behaviorExpectations)
        {
            this.expectations = behaviorExpectations;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeBehaviorExpectation"/> class with
        /// the supplied <see cref="IBehaviorExpectation" /> instances.
        /// </summary>
        /// <param name="behaviorExpectations">The encapsulated behavior expectations.</param>
        /// <seealso cref="CompositeBehaviorExpectation(IBehaviorExpectation[])" />
        /// <seealso cref="BehaviorExpectations" />
        public CompositeBehaviorExpectation(IEnumerable<IBehaviorExpectation> behaviorExpectations)
            : this(behaviorExpectations.ToArray())
        {
        }

        /// <summary>
        /// Verifies the behavior of the command by delegating the implementation to all
        /// <see cref="BehaviorExpectations" />.
        /// </summary>
        /// <param name="command">The command whose behavior must be examined.</param>
        public void Verify(IGuardClauseCommand command)
        {
            foreach (var be in this.BehaviorExpectations)
            {
                be.Verify(command);
            }
        }

        /// <summary>
        /// Gets the behavior expectations supplied via the constructor.
        /// </summary>
        /// <seealso cref="CompositeBehaviorExpectation(IBehaviorExpectation[])" />
        /// <seealso cref="CompositeBehaviorExpectation(IEnumerable{IBehaviorExpectation})"/>
        public IEnumerable<IBehaviorExpectation> BehaviorExpectations
        {
            get { return this.expectations; }
        }
    }
}
