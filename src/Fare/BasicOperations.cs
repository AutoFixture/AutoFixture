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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Fare
{
    public static class BasicOperations
    {
        /// <summary>
        /// Adds epsilon transitions to the given automaton. This method adds extra character interval
        /// transitions that are equivalent to the given set of epsilon transitions.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <param name="pairs">A collection of <see cref="StatePair"/> objects representing pairs of
        /// source/destination states where epsilon transitions should be added.</param>
        public static void AddEpsilons(Automaton a, ICollection<StatePair> pairs)
        {
            a.ExpandSingleton();
            var forward = new Dictionary<State, HashSet<State>>();
            var back = new Dictionary<State, HashSet<State>>();
            foreach (StatePair p in pairs)
            {
                HashSet<State> to = forward[p.FirstState];
                if (to == null)
                {
                    to = new HashSet<State>();
                    forward.Add(p.FirstState, to);
                }

                to.Add(p.SecondState);
                HashSet<State> from = back[p.SecondState];
                if (from == null)
                {
                    from = new HashSet<State>();
                    back.Add(p.SecondState, from);
                }

                from.Add(p.FirstState);
            }

            var worklist = new LinkedList<StatePair>(pairs);
            var workset = new HashSet<StatePair>(pairs);
            while (worklist.Count != 0)
            {
                StatePair p = worklist.RemoveAndReturnFirst();
                workset.Remove(p);
                HashSet<State> to = forward[p.SecondState];
                HashSet<State> from = back[p.FirstState];
                if (to != null)
                {
                    foreach (State s in to)
                    {
                        var pp = new StatePair(p.FirstState, s);
                        if (!pairs.Contains(pp))
                        {
                            pairs.Add(pp);
                            forward[p.FirstState].Add(s);
                            back[s].Add(p.FirstState);
                            worklist.AddLast(pp);
                            workset.Add(pp);
                            if (from != null)
                            {
                                foreach (State q in from)
                                {
                                    var qq = new StatePair(q, p.FirstState);
                                    if (!workset.Contains(qq))
                                    {
                                        worklist.AddLast(qq);
                                        workset.Add(qq);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Add transitions.
            foreach (StatePair p in pairs)
            {
                p.FirstState.AddEpsilon(p.SecondState);
            }

            a.IsDeterministic = false;
            a.ClearHashCode();
            a.CheckMinimizeAlways();
        }

        /// <summary>
        /// Returns an automaton that accepts the union of the languages of the given automata.
        /// </summary>
        /// <param name="automatons">The l.</param>
        /// <returns>
        /// An automaton that accepts the union of the languages of the given automata.
        /// </returns>
        /// <remarks>
        /// Complexity: linear in number of states.
        /// </remarks>
        public static Automaton Union(IList<Automaton> automatons)
        {
            var ids = new HashSet<int>();
            foreach (Automaton a in automatons)
            {
                ids.Add(RuntimeHelpers.GetHashCode(a));
            }

            bool hasAliases = ids.Count != automatons.Count;
            var s = new State();
            foreach (Automaton b in automatons)
            {
                if (b.IsEmpty)
                {
                    continue;
                }

                Automaton bb = b;
                bb = hasAliases ? bb.CloneExpanded() : bb.CloneExpandedIfRequired();

                s.AddEpsilon(bb.Initial);
            }

            var automaton = new Automaton();
            automaton.Initial = s;
            automaton.IsDeterministic = false;
            automaton.ClearHashCode();
            automaton.CheckMinimizeAlways();
            return automaton;
        }

        /// <summary>
        /// Returns a (deterministic) automaton that accepts the complement of the language of the 
        /// given automaton.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <returns>A (deterministic) automaton that accepts the complement of the language of the 
        /// given automaton.</returns>
        /// <remarks>
        /// Complexity: linear in number of states (if already deterministic).
        /// </remarks>
        public static Automaton Complement(Automaton a)
        {
            a = a.CloneExpandedIfRequired();
            a.Determinize();
            a.Totalize();
            foreach (State p in a.GetStates())
            {
                p.Accept = !p.Accept;
            }

            a.RemoveDeadTransitions();
            return a;
        }

        public static Automaton Concatenate(Automaton a1, Automaton a2)
        {
            if (a1.IsSingleton && a2.IsSingleton)
            {
                return BasicAutomata.MakeString(a1.Singleton + a2.Singleton);
            }

            if (BasicOperations.IsEmpty(a1) || BasicOperations.IsEmpty(a2))
            {
                return BasicAutomata.MakeEmpty();
            }

            bool deterministic = a1.IsSingleton && a2.IsDeterministic;
            if (a1 == a2)
            {
                a1 = a1.CloneExpanded();
                a2 = a2.CloneExpanded();
            }
            else
            {
                a1 = a1.CloneExpandedIfRequired();
                a2 = a2.CloneExpandedIfRequired();
            }

            foreach (State s in a1.GetAcceptStates())
            {
                s.Accept = false;
                s.AddEpsilon(a2.Initial);
            }

            a1.IsDeterministic = deterministic;
            a1.ClearHashCode();
            a1.CheckMinimizeAlways();
            return a1;
        }

        public static Automaton Concatenate(IList<Automaton> l)
        {
            if (l.Count == 0)
            {
                return BasicAutomata.MakeEmptyString();
            }

            bool allSingleton = l.All(a => a.IsSingleton);

            if (allSingleton)
            {
                var b = new StringBuilder();
                foreach (Automaton a in l)
                {
                    b.Append(a.Singleton);
                }

                return BasicAutomata.MakeString(b.ToString());
            }
            else
            {
                if (l.Any(a => a.IsEmpty))
                {
                    return BasicAutomata.MakeEmpty();
                }

                var ids = new HashSet<int>();
                foreach (Automaton a in l)
                {
                    ids.Add(RuntimeHelpers.GetHashCode(a));
                }

                bool hasAliases = ids.Count != l.Count;
                Automaton b = l[0];
                b = hasAliases ? b.CloneExpanded() : b.CloneExpandedIfRequired();

                var ac = b.GetAcceptStates();
                bool first = true;
                foreach (Automaton a in l)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        if (a.IsEmptyString())
                        {
                            continue;
                        }

                        Automaton aa = a;
                        aa = hasAliases ? aa.CloneExpanded() : aa.CloneExpandedIfRequired();

                        HashSet<State> ns = aa.GetAcceptStates();
                        foreach (State s in ac)
                        {
                            s.Accept = false;
                            s.AddEpsilon(aa.Initial);
                            if (s.Accept)
                            {
                                ns.Add(s);
                            }
                        }

                        ac = ns;
                    }
                }

                b.IsDeterministic = false;
                b.ClearHashCode();
                b.CheckMinimizeAlways();
                return b;
            }
        }

        /// <summary>
        /// Determinizes the specified automaton.
        /// </summary>
        /// <remarks>
        /// Complexity: exponential in number of states.
        /// </remarks>
        /// <param name="a">The automaton.</param>
        public static void Determinize(Automaton a)
        {
            if (a.IsDeterministic || a.IsSingleton)
            {
                return;
            }

            var initialset = new HashSet<State>();
            initialset.Add(a.Initial);
            BasicOperations.Determinize(a, initialset.ToList());
        }

        /// <summary>
        /// Determinizes the given automaton using the given set of initial states.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <param name="initialset">The initial states.</param>
        public static void Determinize(Automaton a, List<State> initialset)
        {
            char[] points = a.GetStartPoints();

            var comparer = new ListEqualityComparer<State>();

            // Subset construction.
            var sets = new Dictionary<List<State>, List<State>>(comparer);
            var worklist = new LinkedList<List<State>>();
            var newstate = new Dictionary<List<State>, State>(comparer);

            sets.Add(initialset, initialset);
            worklist.AddLast(initialset);
            a.Initial = new State();
            newstate.Add(initialset, a.Initial);

            while (worklist.Count > 0)
            {
                List<State> s = worklist.RemoveAndReturnFirst();
                State r;
                newstate.TryGetValue(s, out r);
                foreach (State q in s)
                {
                    if (q.Accept)
                    {
                        r.Accept = true;
                        break;
                    }
                }

                for (int n = 0; n < points.Length; n++)
                {
                    var set = new HashSet<State>();
                    foreach (State c in s)
                        foreach (Transition t in c.Transitions)
                            if (t.Min <= points[n] && points[n] <= t.Max)
                                set.Add(t.To);

                    var p = set.ToList();

                    if (!sets.ContainsKey(p))
                    {
                        sets.Add(p, p);
                        worklist.AddLast(p);
                        newstate.Add(p, new State());
                    }

                    State q;
                    newstate.TryGetValue(p, out q);
                    char min = points[n];
                    char max;
                    if (n + 1 < points.Length)
                    {
                        max = (char)(points[n + 1] - 1);
                    }
                    else
                    {
                        max = char.MaxValue;
                    }

                    r.Transitions.Add(new Transition(min, max, q));
                }
            }

            a.IsDeterministic = true;
            a.RemoveDeadTransitions();
        }

        /// <summary>
        /// Determines whether the given automaton accepts no strings.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <returns>
        ///   <c>true</c> if the given automaton accepts no strings; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmpty(Automaton a)
        {
            if (a.IsSingleton)
            {
                return false;
            }

            return !a.Initial.Accept && a.Initial.Transitions.Count == 0;
        }

        /// <summary>
        /// Determines whether the given automaton accepts the empty string and nothing else.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <returns>
        ///   <c>true</c> if the given automaton accepts the empty string and nothing else; otherwise,
        /// <c>false</c>.
        /// </returns>
        public static bool IsEmptyString(Automaton a)
        {
            if (a.IsSingleton)
            {
                return a.Singleton.Length == 0;
            }

            return a.Initial.Accept && a.Initial.Transitions.Count == 0;
        }

        /// <summary>
        /// Returns an automaton that accepts the intersection of the languages of the given automata.
        /// Never modifies the input automata languages.
        /// </summary>
        /// <param name="a1">The a1.</param>
        /// <param name="a2">The a2.</param>
        /// <returns></returns>
        public static Automaton Intersection(Automaton a1, Automaton a2)
        {
            if (a1.IsSingleton)
            {
                if (a2.Run(a1.Singleton))
                {
                    return a1.CloneIfRequired();
                }

                return BasicAutomata.MakeEmpty();
            }

            if (a2.IsSingleton)
            {
                if (a1.Run(a2.Singleton))
                {
                    return a2.CloneIfRequired();
                }

                return BasicAutomata.MakeEmpty();
            }

            if (a1 == a2)
            {
                return a1.CloneIfRequired();
            }

            Transition[][] transitions1 = Automaton.GetSortedTransitions(a1.GetStates());
            Transition[][] transitions2 = Automaton.GetSortedTransitions(a2.GetStates());
            var c = new Automaton();
            var worklist = new LinkedList<StatePair>();
            var newstates = new Dictionary<StatePair, StatePair>();
            var p = new StatePair(c.Initial, a1.Initial, a2.Initial);
            worklist.AddLast(p);
            newstates.Add(p, p);
            while (worklist.Count > 0)
            {
                p = worklist.RemoveAndReturnFirst();
                p.S.Accept = p.FirstState.Accept && p.SecondState.Accept;
                Transition[] t1 = transitions1[p.FirstState.Number];
                Transition[] t2 = transitions2[p.SecondState.Number];
                for (int n1 = 0, b2 = 0; n1 < t1.Length; n1++)
                {
                    while (b2 < t2.Length && t2[b2].Max < t1[n1].Min)
                    {
                        b2++;
                    }

                    for (int n2 = b2; n2 < t2.Length && t1[n1].Max >= t2[n2].Min; n2++)
                    {
                        if (t2[n2].Max >= t1[n1].Min)
                        {
                            var q = new StatePair(t1[n1].To, t2[n2].To);
                            StatePair r;
                            newstates.TryGetValue(q, out r);
                            if (r == null)
                            {
                                q.S = new State();
                                worklist.AddLast(q);
                                newstates.Add(q, q);
                                r = q;
                            }

                            char min = t1[n1].Min > t2[n2].Min ? t1[n1].Min : t2[n2].Min;
                            char max = t1[n1].Max < t2[n2].Max ? t1[n1].Max : t2[n2].Max;
                            p.S.Transitions.Add(new Transition(min, max, r.S));
                        }
                    }
                }
            }

            c.IsDeterministic = a1.IsDeterministic && a2.IsDeterministic;
            c.RemoveDeadTransitions();
            c.CheckMinimizeAlways();
            return c;
        }

        /// <summary>
        /// Returns an automaton that accepts the union of the empty string and the language of the 
        /// given automaton.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <remarks>
        /// Complexity: linear in number of states.
        /// </remarks>
        /// <returns>An automaton that accepts the union of the empty string and the language of the 
        /// given automaton.</returns>
        public static Automaton Optional(Automaton a)
        {
            a = a.CloneExpandedIfRequired();
            var s = new State();
            s.AddEpsilon(a.Initial);
            s.Accept = true;
            a.Initial = s;
            a.IsDeterministic = false;
            a.ClearHashCode();
            a.CheckMinimizeAlways();
            return a;
        }

        /// <summary>
        /// Accepts the Kleene star (zero or more concatenated repetitions) of the language of the
        /// given automaton. Never modifies the input automaton language.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <returns>
        /// An automaton that accepts the Kleene star (zero or more concatenated repetitions)
        /// of the language of the given automaton. Never modifies the input automaton language.
        /// </returns>
        /// <remarks>
        /// Complexity: linear in number of states.
        /// </remarks>
        public static Automaton Repeat(Automaton a)
        {
            a = a.CloneExpanded();
            var s = new State();
            s.Accept = true;
            s.AddEpsilon(a.Initial);
            foreach (State p in a.GetAcceptStates())
            {
                p.AddEpsilon(s);
            }

            a.Initial = s;
            a.IsDeterministic = false;
            a.ClearHashCode();
            a.CheckMinimizeAlways();
            return a;
        }

        /// <summary>
        /// Accepts <code>min</code> or more concatenated repetitions of the language of the given 
        /// automaton.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <param name="min">The minimum concatenated repetitions of the language of the given 
        /// automaton.</param>
        /// <returns>Returns an automaton that accepts <code>min</code> or more concatenated 
        /// repetitions of the language of the given automaton.
        /// </returns>
        /// <remarks>
        /// Complexity: linear in number of states and in <code>min</code>.
        /// </remarks>
        public static Automaton Repeat(Automaton a, int min)
        {
            if (min == 0)
            {
                return BasicOperations.Repeat(a);
            }

            var @as = new List<Automaton>();
            while (min-- > 0)
            {
                @as.Add(a);
            }

            @as.Add(BasicOperations.Repeat(a));
            return BasicOperations.Concatenate(@as);
        }

        /// <summary>
        /// Accepts between <code>min</code> and <code>max</code> (including both) concatenated
        /// repetitions of the language of the given automaton.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <param name="min">The minimum concatenated repetitions of the language of the given
        /// automaton.</param>
        /// <param name="max">The maximum concatenated repetitions of the language of the given
        /// automaton.</param>
        /// <returns>
        /// Returns an automaton that accepts between <code>min</code> and <code>max</code>
        /// (including both) concatenated repetitions of the language of the given automaton.
        /// </returns>
        /// <remarks>
        /// Complexity: linear in number of states and in <code>min</code> and <code>max</code>.
        /// </remarks>
        public static Automaton Repeat(Automaton a, int min, int max)
        {
            if (min > max)
            {
                return BasicAutomata.MakeEmpty();
            }

            max -= min;
            a.ExpandSingleton();
            Automaton b;
            if (min == 0)
            {
                b = BasicAutomata.MakeEmptyString();
            }
            else if (min == 1)
            {
                b = a.Clone();
            }
            else
            {
                var @as = new List<Automaton>();
                while (min-- > 0)
                {
                    @as.Add(a);
                }

                b = BasicOperations.Concatenate(@as);
            }

            if (max > 0)
            {
                Automaton d = a.Clone();
                while (--max > 0)
                {
                    Automaton c = a.Clone();
                    foreach (State p in c.GetAcceptStates())
                    {
                        p.AddEpsilon(d.Initial);
                    }

                    d = c;
                }

                foreach (State p in b.GetAcceptStates())
                {
                    p.AddEpsilon(d.Initial);
                }

                b.IsDeterministic = false;
                b.ClearHashCode();
                b.CheckMinimizeAlways();
            }

            return b;
        }

        /// <summary>
        /// Returns true if the given string is accepted by the automaton.
        /// </summary>
        /// <param name="a">The automaton.</param>
        /// <param name="s">The string.</param>
        /// <returns></returns>
        /// <remarks>
        /// Complexity: linear in the length of the string.
        /// For full performance, use the RunAutomaton class.
        /// </remarks>
        public static bool Run(Automaton a, string s)
        {
            if (a.IsSingleton)
            {
                return s.Equals(a.Singleton);
            }

            if (a.IsDeterministic)
            {
                State p = a.Initial;
                foreach (char t in s)
                {
                    State q = p.Step(t);
                    if (q == null)
                    {
                        return false;
                    }

                    p = q;
                }

                return p.Accept;
            }

            HashSet<State> states = a.GetStates();
            Automaton.SetStateNumbers(states);
            var pp = new LinkedList<State>();
            var ppOther = new LinkedList<State>();
            var bb = new BitArray(states.Count);
            var bbOther = new BitArray(states.Count);
            pp.AddLast(a.Initial);
            var dest = new List<State>();
            bool accept = a.Initial.Accept;

            foreach (char c in s)
            {
                accept = false;
                ppOther.Clear();
                bbOther.SetAll(false);
                foreach (State p in pp)
                {
                    dest.Clear();
                    p.Step(c, dest);
                    foreach (State q in dest)
                    {
                        if (q.Accept)
                        {
                            accept = true;
                        }

                        if (!bbOther.Get(q.Number))
                        {
                            bbOther.Set(q.Number, true);
                            ppOther.AddLast(q);
                        }
                    }
                }

                LinkedList<State> tp = pp;
                pp = ppOther;
                ppOther = tp;
                BitArray tb = bb;
                bb = bbOther;
                bbOther = tb;
            }

            return accept;
        }
    }
}
