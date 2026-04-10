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

namespace Fare
{
    /// <summary>
    /// Special automata operations.
    /// </summary>
    public static class SpecialOperations
    {
        /// <summary>
        /// Reverses the language of the given (non-singleton) automaton while returning the set of 
        /// new initial states.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <returns></returns>
        public static HashSet<State> Reverse(Automaton a)
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

        /// <summary>
        /// Returns an automaton that accepts the overlap of strings that in more than one way can be 
        /// split into a left part being accepted by <code>a1</code> and a right part being accepted 
        /// by <code>a2</code>.
        /// </summary>
        /// <param name="a1">The a1.</param>
        /// <param name="a2">The a2.</param>
        /// <returns></returns>
        public static Automaton Overlap(Automaton a1, Automaton a2)
        {
            throw new NotImplementedException();
        }

        private static void AcceptToAccept(Automaton a)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an automaton that accepts the single chars that occur in strings that are accepted
        /// by the given automaton. Never modifies the input automaton.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <returns></returns>
        public static Automaton SingleChars(Automaton a)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an automaton that accepts the trimmed language of the given automaton. The 
        /// resulting automaton is constructed as follows: 1) Whenever a <code>c</code> character is
        /// allowed in the original automaton, one or more <code>set</code> characters are allowed in
        /// the new automaton. 2) The automaton is prefixed and postfixed with any number of <code>
        /// set</code> characters.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <param name="set">The set of characters to be trimmed.</param>
        /// <param name="c">The canonical trim character (assumed to be in <code>set</code>).</param>
        /// <returns></returns>
        public static Automaton Trim(Automaton a, string set, char c)
        {
            throw new NotImplementedException();
        }

        private static void AddSetTransitions(State s, string set, State p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an automaton that accepts the compressed language of the given automaton. 
        /// Whenever a <code>c</code> character is allowed in the original automaton, one or more 
        /// <code>set</code> characters are allowed in the new automaton.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <param name="set">The set of characters to be compressed.</param>
        /// <param name="c">The canonical compress character (assumed to be in <code>set</code>).
        /// </param>
        /// <returns></returns>
        public static Automaton Compress(Automaton a, string set, char c)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an automaton where all transition labels have been substituted. 
        /// <p> Each transition labeled <code>c</code> is changed to a set of transitions, one for 
        /// each character in <code>map(c)</code>. If <code>map(c)</code> is null, then the 
        /// transition is unchanged.
        /// </p>
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <param name="dictionary">The dictionary from characters to sets of characters (where 
        /// characters are <code>char</code> objects).</param>
        /// <returns></returns>
        public static Automaton Subst(Automaton a, IDictionary<char, HashSet<char>> dictionary)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Rinds the largest entry whose value is less than or equal to c, or 0 if there is no
        /// such entry.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="points">The points.</param>
        /// <returns></returns>
        private static int FindIndex(char c, char[] points)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an automaton where all transitions of the given char are replaced by a string.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <param name="c">The c.</param>
        /// <param name="s">The s.</param>
        /// <returns>
        /// A new automaton.
        /// </returns>
        public static Automaton Subst(Automaton a, char c, string s)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an automaton accepting the homomorphic image of the given automaton using the
        /// given function.
        /// <p>
        /// This method maps each transition label to a new value.
        /// <code>source</code> and <code>dest</code> are assumed to be arrays of same length,
        /// and <code>source</code> must be sorted in increasing order and contain no duplicates.
        /// <code>source</code> defines the starting points of char intervals, and the corresponding
        /// entries in <code>dest</code> define the starting points of corresponding new intervals.
        /// </p>
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <param name="source">The source.</param>
        /// <param name="dest">The dest.</param>
        /// <returns></returns>
        public static Automaton Homomorph(Automaton a, char[] source, char[] dest)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an automaton with projected alphabet. The new automaton accepts all strings that
        /// are projections of strings accepted by the given automaton onto the given characters
        /// (represented by <code>Character</code>). If <code>null</code> is in the set, it abbreviates
        /// the intervals u0000-uDFFF and uF900-uFFFF (i.e., the non-private code points). It is assumed
        /// that all other characters from <code>chars</code> are in the interval uE000-uF8FF.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <param name="chars">The chars.</param>
        /// <returns></returns>
        public static Automaton ProjectChars(Automaton a, HashSet<char> chars)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns true if the language of this automaton is finite.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <returns>
        ///   <c>true</c> if the specified a is finite; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFinite(Automaton a)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks whether there is a loop containing s. (This is sufficient since there are never
        /// transitions to dead states).
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="path">The path.</param>
        /// <param name="visited">The visited.</param>
        /// <returns>
        ///   <c>true</c> if the specified s is finite; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsFinite(State s, HashSet<State> path, HashSet<State> visited)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the set of accepted strings of the given length.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static HashSet<string> GetStrings(Automaton a, int length)
        {
            throw new NotImplementedException();
        }

        private static void GetStrings(State s, HashSet<string> strings, StringBuilder path, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the set of accepted strings, assuming this automaton has a finite language. If the
        /// language is not finite, null is returned.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <returns></returns>
        public static HashSet<string> GetFiniteStrings(Automaton a)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the set of accepted strings, assuming that at most <code>limit</code> strings are
        /// accepted. If more than <code>limit</code> strings are accepted, null is returned. If
        /// <code>limit</code>&lt;0, then this methods works like {@link #getFiniteStrings(Automaton)}.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        public static HashSet<string> GetFiniteStrings(Automaton a, int limit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the strings that can be produced from the given state, or false if more than
        /// <code>limit</code> strings are found. <code>limit</code>&lt;0 means "infinite".
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="pathStates">The path states.</param>
        /// <param name="strings">The strings.</param>
        /// <param name="path">The path.</param>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        private static bool GetFiniteStrings(
            State s,
            HashSet<State> pathStates,
            HashSet<string> strings,
            StringBuilder path,
            int limit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the longest string that is a prefix of all accepted strings and visits each state
        /// at most once.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <returns>
        /// A common prefix.
        /// </returns>
        public static string GetCommonPrefix(Automaton a)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prefix closes the given automaton.
        /// </summary>
        /// <param name="a">The automaton.</param>
        public static void PrefixClose(Automaton a)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructs automaton that accepts the same strings as the given automaton but ignores upper/lower 
        /// case of A-F.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <returns>An automaton.</returns>
        public static Automaton HexCases(Automaton a)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructs automaton that accepts 0x20, 0x9, 0xa, and 0xd in place of each 0x20 transition
        /// in the given automaton.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <returns>An automaton.</returns> 
        public static Automaton ReplaceWhitespace(Automaton a)
        {
            throw new NotImplementedException();
        }
    }
}
