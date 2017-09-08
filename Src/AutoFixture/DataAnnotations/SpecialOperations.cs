using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.DataAnnotations
{
    /// <summary>
    /// Special automata operations.
    /// </summary>
    internal static class SpecialOperations
    {
        /// <summary>
        /// Reverses the language of the given (non-singleton) automaton while returning the set of 
        /// new initial states.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <returns></returns>
        internal static HashSet<State> Reverse(Automaton a)
        {
            // Reverse all edges.
            var m = new Dictionary<State, HashSet<Transition>>();
            HashSet<State> states = a.GetStates();
            HashSet<State> accept = a.GetAcceptStates();
            foreach (State r in states)
            {
                m.Add(r, new HashSet<Transition>());
                r.Accept = false;
            }

            foreach (State r in states)
            {
                foreach (Transition t in r.Transitions)
                {
                    m[t.To].Add(new Transition(t.Min, t.Max, r));
                }
            }

            foreach (State r in states)
            {
                r.Transitions = m[r].ToList();
            }

            // Make new initial+final states.
            a.Initial.Accept = true;
            a.Initial = new State();
            foreach (State r in accept)
            {
                a.Initial.AddEpsilon(r); // Ensures that all initial states are reachable.
            }

            a.IsDeterministic = false;
            return accept;
        }
    }
}
