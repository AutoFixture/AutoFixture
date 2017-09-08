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
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.DataAnnotations
{
    internal static class BasicAutomata
    {
        /// <summary>
        /// Returns a new (deterministic) automaton that accepts any single character.
        /// </summary>
        /// <returns>A new (deterministic) automaton that accepts any single character.</returns>
        internal static Automaton MakeAnyChar()
        {
            return BasicAutomata.MakeCharRange(char.MinValue, char.MaxValue);
        }

        /// <summary>
        /// Returns a new (deterministic) automaton that accepts all strings.
        /// </summary>
        /// <returns>
        /// A new (deterministic) automaton that accepts all strings.
        /// </returns>
        internal static Automaton MakeAnyString()
        {
            var state = new State();
            state.Accept = true;
            state.Transitions.Add(new Transition(char.MinValue, char.MaxValue, state));

            var a = new Automaton();
            a.Initial = state;
            a.IsDeterministic = true;
            return a;
        }

        /// <summary>
        /// Returns a new (deterministic) automaton that accepts a single character of the given value.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>A new (deterministic) automaton that accepts a single character of the given value.</returns>
        internal static Automaton MakeChar(char c)
        {
            var a = new Automaton();
            a.Singleton = c.ToString();
            a.IsDeterministic = true;
            return a;
        }

        /// <summary>
        /// Returns a new (deterministic) automaton that accepts a single char whose value is in the
        /// given interval (including both end points).
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        /// <returns>
        /// A new (deterministic) automaton that accepts a single char whose value is in the
        /// given interval (including both end points).
        /// </returns>
        internal static Automaton MakeCharRange(char min, char max)
        {
            if (min == max)
            {
                return BasicAutomata.MakeChar(min);
            }

            var a = new Automaton();
            var s1 = new State();
            var s2 = new State();
            a.Initial = s1;
            s2.Accept = true;
            if (min <= max)
            {
                s1.Transitions.Add(new Transition(min, max, s2));
            }

            a.IsDeterministic = true;
            return a;
        }

        /// <summary>
        /// Returns a new (deterministic) automaton with the empty language.
        /// </summary>
        /// <returns>
        /// A new (deterministic) automaton with the empty language.
        /// </returns>
        internal static Automaton MakeEmpty()
        {
            var a = new Automaton();
            var s = new State();
            a.Initial = s;
            a.IsDeterministic = true;
            return a;
        }

        /// <summary>
        /// Returns a new (deterministic) automaton that accepts only the empty string.
        /// </summary>
        /// <returns>
        /// A new (deterministic) automaton that accepts only the empty string.
        /// </returns>
        internal static Automaton MakeEmptyString()
        {
            var a = new Automaton();
            a.Singleton = string.Empty;
            a.IsDeterministic = true;
            return a;
        }

        /// <summary>
        /// Returns a new automaton that accepts strings representing decimal non-negative integers in
        /// the given interval.
        /// </summary>
        /// <param name="min">The minimum value of interval.</param>
        /// <param name="max">The maximum value of interval (both end points are included in the 
        /// interval).</param>
        /// <param name="digits">If f >0, use fixed number of digits (strings must be prefixed by 0's 
        /// to obtain the right length) otherwise, the number of digits is not fixed.</param>
        /// <returns>A new automaton that accepts strings representing decimal non-negative integers 
        /// in the given interval.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Justification = "This method has been ported as-is.")]
        internal static Automaton MakeInterval(int min, int max, int digits)
        {
            var a = new Automaton();
            string x = Convert.ToString(min, CultureInfo.CurrentCulture);
            string y = Convert.ToString(max, CultureInfo.CurrentCulture);
            if (min > max || (digits > 0 && y.Length > digits))
            {
                throw new ArgumentException();
            }

            int d = digits > 0 ? digits : y.Length;
            var bx = new StringBuilder();
            for (int i = x.Length; i < d; i++)
            {
                bx.Append('0');
            }

            bx.Append(x);
            x = bx.ToString();
            var by = new StringBuilder();
            for (int i = y.Length; i < d; i++)
            {
                by.Append('0');
            }

            by.Append(y);
            y = by.ToString();
            ICollection<State> initials = new List<State>();
            a.Initial = BasicAutomata.Between(x, y, 0, initials, digits <= 0);
            if (digits <= 0)
            {
                List<StatePair> pairs = (from p in initials
                                         where a.Initial != p
                                         select new StatePair(a.Initial, p)).ToList();
                a.AddEpsilons(pairs);
                a.Initial.AddTransition(new Transition('0', a.Initial));
                a.IsDeterministic = false;
            }
            else
            {
                a.IsDeterministic = true;
            }

            a.CheckMinimizeAlways();
            return a;
        }

        /// <summary>
        /// Returns a new (deterministic) automaton that accepts the single given string.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>A new (deterministic) automaton that accepts the single given string.</returns>
        internal static Automaton MakeString(string s)
        {
            var a = new Automaton();
            a.Singleton = s;
            a.IsDeterministic = true;
            return a;
        }

        /// <summary>
        /// Constructs sub-automaton corresponding to decimal numbers of length x.Substring(n).Length.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        private static State AnyOfRightLength(string x, int n)
        {
            var s = new State();

            if (x.Length == n)
            {
                s.Accept = true;
            }
            else
            {
                s.AddTransition(new Transition('0', '9', AnyOfRightLength(x, n + 1)));
            }

            return s;
        }

        /// <summary>
        /// Constructs sub-automaton corresponding to decimal numbers of value at least x.Substring(n)
        /// and length x.Substring(n).Length.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="n">The n.</param>
        /// <param name="initials">The initials.</param>
        /// <param name="zeros">if set to <c>true</c> [zeros].</param>
        /// <returns></returns>
        private static State AtLeast(string x, int n, ICollection<State> initials, bool zeros)
        {
            var s = new State();
            if (x.Length == n)
            {
                s.Accept = true;
            }
            else
            {
                if (zeros)
                {
                    initials.Add(s);
                }

                char c = x[n];
                s.AddTransition(new Transition(c, AtLeast(x, n + 1, initials, zeros && c == '0')));
                if (c < '9')
                {
                    s.AddTransition(new Transition((char)(c + 1), '9', AnyOfRightLength(x, n + 1)));
                }
            }

            return s;
        }

        /// <summary>
        /// Constructs sub-automaton corresponding to decimal numbers of value at most x.Substring(n)
        /// and length x.Substring(n).Length.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        private static State AtMost(string x, int n)
        {
            var s = new State();

            if (x.Length == n)
            {
                s.Accept = true;
            }
            else
            {
                char c = x[n];
                s.AddTransition(new Transition(c, AtMost(x, (char)n + 1)));
                if (c > '0')
                {
                    s.AddTransition(new Transition('0', (char)(c - 1), AnyOfRightLength(x, n + 1)));
                }
            }

            return s;
        }

        /// <summary>
        /// Constructs sub-automaton corresponding to decimal numbers of value between x.Substring(n)
        /// and y.Substring(n) and of length x.Substring(n).Length (which must be equal to 
        /// y.Substring(n).Length).
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="n">The n.</param>
        /// <param name="initials">The initials.</param>
        /// <param name="zeros">if set to <c>true</c> [zeros].</param>
        /// <returns></returns>
        private static State Between(string x, string y, int n, ICollection<State> initials, bool zeros)
        {
            var s = new State();

            if (x.Length == n)
            {
                s.Accept = true;
            }
            else
            {
                if (zeros)
                {
                    initials.Add(s);
                }

                char cx = x[n];
                char cy = y[n];
                if (cx == cy)
                {
                    s.AddTransition(new Transition(cx, Between(x, y, n + 1, initials, zeros && cx == '0')));
                }
                else
                {
                    // cx < cy
                    s.AddTransition(new Transition(cx, BasicAutomata.AtLeast(x, n + 1, initials, zeros && cx == '0')));
                    s.AddTransition(new Transition(cy, BasicAutomata.AtMost(y, n + 1)));
                    if (cx + 1 < cy)
                    {
                        s.AddTransition(new Transition((char)(cx + 1), (char)(cy - 1), BasicAutomata.AnyOfRightLength(x, n + 1)));
                    }
                }
            }

            return s;
        }

        /// <summary>
        /// Returns a new (deterministic) automaton that accepts a single character in the given set.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This method has been ported as-is.")]
        internal static Automaton MakeCharSet(string set)
        {
            if (set.Length == 1)
            {
                return MakeChar(set[0]);
            }

            var a = new Automaton();
            var s1 = new State();
            var s2 = new State();

            a.Initial = s1;
            s2.Accept = true;

            foreach (char t in set)
            {
                s1.Transitions.Add(new Transition(t, s2));
            }

            a.IsDeterministic = true;
            a.Reduce();

            return a;
        }

        /// <summary>
        /// Constructs automaton that accept strings representing nonnegative integer that are not 
        /// larger than the given value.
        /// </summary>
        /// <param name="n">The n string representation of maximum value.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This method has been ported as-is.")]
        internal static Automaton MakeMaxInteger(String n)
        {
            int i = 0;
            while (i < n.Length && n[i] == '0')
            {
                i++;
            }

            var b = new StringBuilder();
            b.Append("0*(0|");
            if (i < n.Length)
            {
                b.Append("[0-9]{1," + (n.Length - i - 1) + "}|");
            }

            MaxInteger(n.Substring(i), 0, b);
            b.Append(")");
            return Automaton.Minimize((new RegExp(b.ToString())).ToAutomaton());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This method has been ported as-is.")]
        private static void MaxInteger(String n, int i, StringBuilder b) 
        {
            b.Append('(');
            if (i < n.Length)
            {
                char c = n[i];
                if (c != '0')
                {
                    b.Append("[0-" + (char)(c - 1) + "][0-9]{" + (n.Length - i - 1) + "}|");
                }

                b.Append(c);
                MaxInteger(n, i + 1, b);
            }

            b.Append(')');
        }

        /// <summary>
        /// Constructs automaton that accept strings representing nonnegative integers that are not
        /// less that the given value.
        /// </summary>
        /// <param name="n">The n string representation of minimum value.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This method has been ported as-is.")]
        internal static Automaton MakeMinInteger(String n) 
        {
            int i = 0;
            while (i + 1 < n.Length && n[i] == '0')
            {
                i++;
            }

            var b = new StringBuilder();
            b.Append("0*");
            MinInteger(n.Substring(i), 0, b);
            b.Append("[0-9]*");
            return Automaton.Minimize((new RegExp(b.ToString())).ToAutomaton());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This method has been ported as-is.")]
        private static void MinInteger(String n, int i, StringBuilder b) 
        {
            b.Append('(');
            if (i < n.Length)
            {
                char c = n[i];
                if (c != '9')
                {
                    b.Append("[" + (char)(c + 1) + "-9][0-9]{" + (n.Length - i - 1) + "}|");
                }

                b.Append(c);
                MinInteger(n, i + 1, b);
            }

            b.Append(')');
        }

        /// <summary>
        /// Constructs automaton that accept strings representing decimal numbers that can be 
        /// written with at most the given number of digits. Surrounding whitespace is permitted.
        /// </summary>
        /// <param name="i">The i max number of necessary digits.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This method has been ported as-is.")]
        internal static Automaton MakeTotalDigits(int i) 
        {
            return
               Automaton.Minimize(
                    new RegExp("[ \t\n\r]*[-+]?0*([0-9]{0," + i + "}|((([0-9]\\.*){0," + i + "})&@\\.@)0*)[ \t\n\r]*")
                       .ToAutomaton());
        }

        /// <summary>
        /// Constructs automaton that accept strings representing decimal numbers that can be 
        /// written with at most the given number of digits in the fraction part. Surrounding
        /// whitespace is permitted.
        /// </summary>
        /// <param name="i">The i max number of necessary fraction digits.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This method has been ported as-is.")]
        internal static Automaton MakeFractionDigits(int i) 
        {
            return
                Automaton.Minimize(
                    new RegExp("[ \t\n\r]*[-+]?[0-9]+(\\.[0-9]{0," + i + "}0*)?[ \t\n\r]*")
                        .ToAutomaton());
        }

        /// <summary>
        /// Constructs deterministic automaton that matches strings that contain the given substring.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "This method has been ported as-is.")]
        internal static Automaton MakeStringMatcher(String s) 
        {
            var a = new Automaton();
            var states = new State[s.Length + 1];
            states[0] = a.Initial;
            for (int i = 0; i < s.Length; i++)
            {
                states[i + 1] = new State();
            }

            State f = states[s.Length];
            f.Accept = true;
            f.Transitions.Add(new Transition(Char.MinValue, Char.MaxValue, f));
            for (int i = 0; i < s.Length; i++)
            {
                var done = new HashSet<char?>();
                char c = s[i];
                states[i].Transitions.Add(new Transition(c, states[i + 1]));
                done.Add(c);
                for (int j = i; j >= 1; j--)
                {
                    char d = s[j - 1];
                    if (!done.Contains(d) && s.Substring(0, j - 1).Equals(s.Substring(i - j + 1, i - (i - j + 1))))
                    {
                        states[i].Transitions.Add(new Transition(d, states[j]));
                        done.Add(d);
                    }
                }

                var da = new char[done.Count];
                int h = 0;
                foreach (char w in done)
                {
                    da[h++] = w;
                }

                Array.Sort(da);
                int from = Char.MinValue;
                int k = 0;
                while (from <= Char.MaxValue)
                {
                    while (k < da.Length && da[k] == from)
                    {
                        k++;
                        from++;
                    }

                    if (from <= Char.MaxValue)
                    {
                        int to = Char.MaxValue;
                        if (k < da.Length)
                        {
                            to = da[k] - 1;
                            k++;
                        }

                        states[i].Transitions.Add(new Transition((char)from, (char)to, states[0]));
                        from = to + 2;
                    }
                }
            }

            a.IsDeterministic = true;
            return a;
        }
    }
}