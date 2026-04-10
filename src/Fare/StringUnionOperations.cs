/*
* http://github.com/moodmosaic/Fare/
* Original Java code:
* http://www.brics.dk/automaton/
* 
* Operations for building minimal deterministic automata from sets of strings. 
* The algorithm requires sorted input data, but is very fast (nearly linear with the input size).
* 
* @author Dawid Weiss
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Fare
{
    public sealed class StringUnionOperations
    {
        private static readonly IComparer<char[]> lexicographicOrder = new LexicographicOrder();

        private readonly State root = new State();

        private StringBuilder previous;
        private IDictionary<State, State> register = new Dictionary<State, State>();

        public static IComparer<char[]> LexicographicOrderComparer
        {
            get { return lexicographicOrder; }
        }

        public static Fare.State Build(IEnumerable<char[]> input)
        {
            var builder = new StringUnionOperations();

            foreach (var chs in input)
            {
                builder.Add(chs);
            }

            return StringUnionOperations.Convert(builder.Complete(), new Dictionary<State, Fare.State>());
        }

        public void Add(char[] current)
        {
            Debug.Assert(this.register != null, "Automaton already built.");
            Debug.Assert(current.Length > 0, "Input sequences must not be empty.");
            Debug.Assert(
                this.previous == null ||
                LexicographicOrderComparer.Compare(this.previous.ToString().ToCharArray(), current) <= 0,
                "Input must be sorted: " + this.previous + " >= " + current);
            Debug.Assert(this.SetPrevious(current));

            // Descend in the automaton (find matching prefix). 
            int pos = 0;
            int max = current.Length;
            State next;
            State state = root;
            while (pos < max && (next = state.GetLastChild(current[pos])) != null)
            {
                state = next;
                pos++;
            }

            if (state.HasChildren)
            {
                this.ReplaceOrRegister(state);
            }

            StringUnionOperations.AddSuffix(state, current, pos);
        }

        private static void AddSuffix(State state, char[] current, int fromIndex)
        {
            for (int i = fromIndex; i < current.Length; i++)
            {
                state = state.NewState(current[i]);
            }

            state.IsFinal = true;
        }

        private static Fare.State Convert(State s, IDictionary<State, Fare.State> visited)
        {
            Fare.State converted = visited[s];
            if (converted != null)
            {
                return converted;
            }

            converted = new Fare.State();
            converted.Accept = s.IsFinal;

            visited.Add(s, converted);
            int i = 0;
            char[] labels = s.TransitionLabels;
            foreach (State target in s.States)
            {
                converted.AddTransition(new Transition(labels[i++], StringUnionOperations.Convert(target, visited)));
            }

            return converted;
        }

        private State Complete()
        {
            if (this.register == null)
            {
                throw new InvalidOperationException("register is null");
            }

            if (this.root.HasChildren)
            {
                this.ReplaceOrRegister(this.root);
            }

            this.register = null;
            return this.root;
        }

        private void ReplaceOrRegister(State state)
        {
            State child = state.LastChild;

            if (child.HasChildren)
            {
                this.ReplaceOrRegister(child);
            }

            State registered = this.register[child];
            if (registered != null)
            {
                state.ReplaceLastChild(registered);
            }
            else
            {
                this.register.Add(child, child);
            }
        }

        private bool SetPrevious(char[] current)
        {
            if (this.previous == null)
            {
                this.previous = new StringBuilder();
            }

            this.previous.Length = 0;
            this.previous.Append(current);

            return true;
        }

        private sealed class LexicographicOrder : IComparer<char[]>
        {
            public int Compare(char[] s1, char[] s2)
            {
                int lens1 = s1.Length;
                int lens2 = s2.Length;
                int max = Math.Min(lens1, lens2);

                for (int i = 0; i < max; i++)
                {
                    char c1 = s1[i];
                    char c2 = s2[i];
                    if (c1 != c2)
                    {
                        return c1 - c2;
                    }
                }

                return lens1 - lens2;
            }
        }

        private sealed class State
        {
            private static readonly  char[] noLabels = new char[0];
            private static readonly State[] noStates = new State[0];
            private bool isFinal;

            private  char[] labels = noLabels;
            private State[] states = noStates;

            public char[] TransitionLabels
            {
                get { return this.labels; }
            }

            public IEnumerable<State> States
            {
                get { return this.states; }
            }

            public bool HasChildren
            {
                get { return this.labels.Length > 0; }
            }

            public bool IsFinal
            {
                get { return this.isFinal; }
                set { this.isFinal = value; }
            }

            public State LastChild
            {
                get
                {
                    Debug.Assert(this.HasChildren, "No outgoing transitions.");
                    return this.states[this.states.Length - 1];
                }
            }

            public override bool Equals(object obj)
            {
                var other = obj as State;
                if (other == null)
                {
                    return false;
                }

                return this.isFinal == other.isFinal
                    && State.ReferenceEquals(states, other.states)
                    && object.Equals(labels, other.labels);
            }

            public override int GetHashCode()
            {
                int hash = this.isFinal ? 1 : 0;
                hash ^= (hash * 31) + this.labels.Length;
                hash = this.labels.Aggregate(hash, (current, c) => current ^ (current * 31) + c);

                // Compare the right-language of this state using reference-identity of
                // outgoing states. This is possible because states are interned (stored
                // in registry) and traversed in post-order, so any outgoing transitions
                // are already interned.
                return this.states.Aggregate(hash, (current, s) => current ^ RuntimeHelpers.GetHashCode(s));
            }

            public State NewState(char label)
            {
                Debug.Assert(
                    Array.BinarySearch(this.labels, label) < 0,
                    "State already has transition labeled: " + label);

                this.labels = CopyOf(this.labels, this.labels.Length + 1);
                this.states = CopyOf(this.states, this.states.Length + 1);

                this.labels[this.labels.Length - 1] = label;
                return states[this.states.Length - 1] = new State();
            }

            public State GetLastChild(char label)
            {
                int index = this.labels.Length - 1;
                State s = null;
                if (index >= 0 && this.labels[index] == label)
                {
                    s = this.states[index];
                }

                Debug.Assert(s == this.GetState(label));
                return s;
            }

            public void ReplaceLastChild(State state)
            {
                Debug.Assert(this.HasChildren, "No outgoing transitions.");
                this.states[this.states.Length - 1] = state;
            }

            private static char[] CopyOf(char[] original, int newLength)
            {
                var copy = new char[newLength];
                Array.Copy(original, 0, copy, 0, Math.Min(original.Length, newLength));
                return copy;
            }

            private static State[] CopyOf(State[] original, int newLength)
            {
                var copy = new State[newLength];
                Array.Copy(original, 0, copy, 0, Math.Min(original.Length, newLength));
                return copy;
            }

            private static bool ReferenceEquals(Object[] a1, Object[] a2)
            {
                if (a1.Length != a2.Length)
                {
                    return false;
                }

                return !a1.Where((t, i) => t != a2[i]).Any();
            }

            private State GetState(char label)
            {
                int index = Array.BinarySearch(this.labels, label);
                return index >= 0 ? states[index] : null;
            }
        }
    }
}
