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
using System.Globalization;
using System.Text;

namespace Fare
{
    ///<summary>
    ///  <tt>Automaton</tt> transition. 
    ///  <p>
    ///    A transition, which belongs to a source state, consists of a Unicode character interval
    ///    and a destination state.
    ///  </p>
    ///</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Documentation", "RS0010: Avoid using cref tags with a prefix",
        Justification = "This code has been copied from another project and is used as-is.")]
    internal sealed class Transition : IEquatable<Transition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transition"/> class.
        /// (Constructs a new singleton interval transition).
        /// </summary>
        /// <param name="c">The transition character.</param>
        /// <param name="to">The destination state.</param>
        internal Transition(char c, State to)
        {
            this.Min = this.Max = c;
            this.To = to;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transition"/> class.
        /// (Both end points are included in the interval).
        /// </summary>
        /// <param name="min">The transition interval minimum.</param>
        /// <param name="max">The transition interval maximum.</param>
        /// <param name="to">The destination state.</param>
        internal Transition(char min, char max, State to)
        {
            if (max < min)
            {
                char t = max;
                max = min;
                min = t;
            }

            this.Min = min;
            this.Max = max;
            this.To = to;
        }

        /// <summary>
        /// Gets the minimum of this transition interval.
        /// </summary>
        internal char Min { get; }

        /// <summary>
        /// Gets the maximum of this transition interval.
        /// </summary>
        internal char Max { get; }

        /// <summary>
        /// Gets the destination of this transition.
        /// </summary>
        internal State To { get; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Transition left, Transition right)
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
        public static bool operator !=(Transition left, Transition right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();
            Transition.AppendCharString(this.Min, sb);
            if (this.Min != this.Max)
            {
                sb.Append("-");
                Transition.AppendCharString(this.Max, sb);
            }

            sb.Append(" -> ").Append(this.To.Number);
            return sb.ToString();
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

            if (obj.GetType() != typeof(Transition))
            {
                return false;
            }

            return this.Equals((Transition)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.Min.GetHashCode();
                result = (result*397) ^ this.Max.GetHashCode();
                result = (result*397) ^ (this.To != null ? this.To.GetHashCode() : 0);
                return result;
            }
        }

        /// <inheritdoc />
        public bool Equals(Transition other)
        {
            if (object.ReferenceEquals(null, other))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Min == this.Min
                   && other.Max == this.Max
                   && object.Equals(other.To, this.To);
        }

        private static void AppendCharString(char c, StringBuilder sb)
        {
            if (c >= 0x21 && c <= 0x7e && c != '\\' && c != '"')
            {
                sb.Append(c);
            }
            else
            {
                sb.Append("\\u");
                string s = ((int)c).ToString("x", CultureInfo.CurrentCulture);
                if (c < 0x10)
                {
                    sb.Append("000").Append(s);
                }
                else if (c < 0x100)
                {
                    sb.Append("00").Append(s);
                }
                else if (c < 0x1000)
                {
                    sb.Append("0").Append(s);
                }
                else
                {
                    sb.Append(s);
                }
            }
        }
    }
}