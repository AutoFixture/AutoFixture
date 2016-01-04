﻿using System;
using System.ComponentModel;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates a range for values of a given type.
    /// </summary>
    public class RangedNumberRequest : IEquatable<RangedNumberRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RangedNumberRequest"/> class.
        /// </summary>
        /// <param name="operandType">Type of the operand.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        public RangedNumberRequest(Type operandType, object minimum, object maximum)
        {
            if (operandType == null)
            {
                throw new ArgumentNullException(nameof(operandType));
            }

            if (minimum == null)
            {
                throw new ArgumentNullException(nameof(minimum));
            }

            if (maximum == null)
            {
                throw new ArgumentNullException(nameof(maximum));
            }

            if (((IComparable)minimum).CompareTo((IComparable)maximum) >= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minimum), "Minimum must be lower than Maximum.");
            }

            this.OperandType = operandType;
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        /// <summary>
        /// Gets the type of the operand.
        /// </summary>
        /// <value>
        /// The type of the operand.
        /// </value>
        public Type OperandType { get; }

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        public object Minimum { get; }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        public object Maximum { get; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        ///   </exception>
        public override bool Equals(object obj)
        {
            var other = obj as RangedNumberRequest;
            if (other != null)
            {
                return this.Equals(other);
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.OperandType.GetHashCode()
                ^ this.Minimum.GetHashCode()
                ^ this.Maximum.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(RangedNumberRequest other)
        {
            if (other == null)
            {
                return false;
            }

            return this.OperandType == other.OperandType
                && object.Equals(this.Minimum, other.Minimum)
                && object.Equals(this.Maximum, other.Maximum);
        }
    }
}