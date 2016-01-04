﻿/*
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

namespace Ploeh.AutoFixture.DataAnnotations
{
    /// <summary>
    /// <tt>Automaton</tt> state.
    /// </summary>
    internal sealed class State : IEquatable<State>, IComparable<State>
    {
        private static int nextId;

        /// <summary>
        /// Initializes a new instance of the <see cref="State"/> class. Initially, the new state is a 
        ///   reject state.
        /// </summary>
        internal State()
        {
            this.ResetTransitions();
            Id = Interlocked.Increment(ref nextId);
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        internal int Id { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this State is Accept.
        /// </summary>
        internal bool Accept { get; set; }

        /// <summary>
        /// Gets or sets this State Number.
        /// </summary>
        internal int Number { get; set; }

        /// <summary>
        /// Gets or sets this State Transitions.
        /// </summary>
        internal IList<Transition> Transitions { get; set; }

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

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current
        ///  <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current
        ///  <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current 
        /// <see cref="T:System.Object"/>. 
        ///                 </param><exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        ///                 </exception><filterpriority>2</filterpriority>
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

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = Id;
                result = (result * 397) ^ Accept.GetHashCode();
                result = (result * 397) ^ Number;
                return result;
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
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

            return other.Id == Id 
                && other.Accept.Equals(Accept)
                && other.Number == Number;
        }

        /// <summary>
        /// Compares the current object with another object of the same type. States are ordered by 
        /// the time of construction.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(State other)
        {
            if (other == null)
            {
                return 1;
            }

            return other.Id - this.Id;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> describing this state.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> describing this state.
        /// </returns>
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
        internal void AddTransition(Transition t)
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
        internal State Step(char c)
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
        internal void Step(char c, List<State> dest)
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
        internal IList<Transition> GetSortedTransitions(bool toFirst)
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