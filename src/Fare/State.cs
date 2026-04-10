/*
 * dk.brics.automaton
 * 
 * Copyright (c) 2001-2011 Anders Moeller
 * All rights reserved.
 * http://github.com/moodmosaic/Fare/
 * Original Java code:
 * http://www.brics.dk/automaton/
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Fare
{
    /// <summary>
    /// <tt>Automaton</tt> state.
    /// </summary>
    public class State : IEquatable<State>, IComparable<State>, IComparable
    {
        private readonly int id;
        private static int nextId;

        /// <summary>
        /// Initializes a new instance of the <see cref="State"/> class. Initially, the new state is a 
        ///   reject state.
        /// </summary>
        public State()
        {
            this.ResetTransitions();
            id = Interlocked.Increment(ref nextId);
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public int Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this State is Accept.
        /// </summary>
        public bool Accept { get; set; }

        /// <summary>
        /// Gets or sets this State Number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets this State Transitions.
        /// </summary>
        public IList<Transition> Transitions { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(State left, State right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(State left, State right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj))
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(State))
            {
                return false;
            }

            return this.Equals((State)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int result = id;
                result = (result * 397) ^ Accept.GetHashCode();
                result = (result * 397) ^ Number;
                return result;
            }
        }


        /// <inheritdoc />
        public int CompareTo(object other)
        {
            if (other == null)
            {
                return 1;
            }

            if (other.GetType() != typeof(State))
            {
                throw new ArgumentException("Object is not a State");
            }

            return this.CompareTo((State)other);
        }

        /// <inheritdoc />
        public bool Equals(State other)
        {
            if (object.ReferenceEquals(null, other))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return other.id == id 
                && other.Accept.Equals(Accept)
                && other.Number == Number;
        }

        /// <inheritdoc />
        public int CompareTo(State other)
        {
            return other.Id - this.Id;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("state ").Append(this.Number);
            sb.Append(this.Accept ? " [accept]" : " [reject]");
            sb.Append(":\n");
            foreach (Transition t in this.Transitions)
            {
                sb.Append("  ").Append(t.ToString()).Append("\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Adds an outgoing transition.
        /// </summary>
        /// <param name="t">
        /// The transition.
        /// </param>
        public void AddTransition(Transition t)
        {
            this.Transitions.Add(t);
        }

        /// <summary>
        /// Performs lookup in transitions, assuming determinism.
        /// </summary>
        /// <param name="c">
        /// The character to look up.
        /// </param>
        /// <returns>
        /// The destination state, null if no matching outgoing transition.
        /// </returns>
        public State Step(char c)
        {
            return (from t in this.Transitions where t.Min <= c && c <= t.Max select t.To).FirstOrDefault();
        }

        /// <summary>
        /// Performs lookup in transitions, allowing nondeterminism.
        /// </summary>
        /// <param name="c">
        /// The character to look up.
        /// </param>
        /// <param name="dest">
        /// The collection where destination states are stored.
        /// </param>
        public void Step(char c, List<State> dest)
        {
            dest.AddRange(from t in this.Transitions where t.Min <= c && c <= t.Max select t.To);
        }

        /// <summary>
        /// Gets the transitions sorted by (min, reverse max, to) or (to, min, reverse max).
        /// </summary>
        /// <param name="toFirst">
        /// if set to <c>true</c> [to first].
        /// </param>
        /// <returns>
        /// The transitions sorted by (min, reverse max, to) or (to, min, reverse max).
        /// </returns>
        public IList<Transition> GetSortedTransitions(bool toFirst)
        {
            Transition[] e = this.Transitions.ToArray();
            Array.Sort(e, new TransitionComparer(toFirst));
            return e.ToList();
        }

        internal void AddEpsilon(State to)
        {
            if (to.Accept)
            {
                this.Accept = true;
            }

            foreach (Transition t in to.Transitions)
            {
                this.Transitions.Add(t);
            }
        }

        internal void ResetTransitions()
        {
            this.Transitions = new List<Transition>();
        }
    }
}
