﻿using System;
using System.Globalization;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates a strictly increasing sequence of ranged numbers, starting at range minimum.  
    /// Sequence restarts at range minimum when range maximum is exceeded. 
    /// </summary>
    public class RangedNumberGenerator : ISpecimenBuilder
    {
        private readonly object syncRoot;
        private object rangedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangedNumberGenerator"/> class.
        /// </summary>
        public RangedNumberGenerator()
        {
            this.syncRoot = new object();
            this.rangedValue = null;
        }

        /// <summary>
        /// Creates a new number based on a RangedNumberRequest.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested number if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
            {
                return new NoSpecimen();
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var range = request as RangedNumberRequest;
            if (range == null)
            {
                return new NoSpecimen(request);
            }

            var value = context.Resolve(range.OperandType) as IComparable;
            if (value == null)
            {
                return new NoSpecimen(request);
            }

            try
            {
                this.CreateAnonymous(range, value);
            }
            catch (InvalidOperationException)
            {
                return new NoSpecimen(request);
            }

            return this.rangedValue;
        }

        private void CreateAnonymous(RangedNumberRequest range, IComparable value)
        {
            lock (this.syncRoot)
            {
                var minimum = (IComparable)range.Minimum;
                var maximum = (IComparable)range.Maximum;

                if (this.rangedValue != null)
                {
                    object target;
                    if (range.OperandType == typeof(byte) &&
                        Convert.ToInt32(
                            this.rangedValue, 
                            CultureInfo.CurrentCulture) > byte.MaxValue ||
                        range.OperandType == typeof(short) &&
                        Convert.ToInt32(
                            this.rangedValue,
                            CultureInfo.CurrentCulture) > short.MaxValue)
                        target = minimum;
                    else
                        target = this.rangedValue;

                    this.rangedValue = Convert.ChangeType(target, range.OperandType, CultureInfo.CurrentCulture);
                }

                if (this.rangedValue != null && (minimum.CompareTo(this.rangedValue) <= 0 && maximum.CompareTo(this.rangedValue) > 0))
                {
                    this.rangedValue =
                        Convert.ChangeType(
                            RangedNumberGenerator.Add(
                                this.rangedValue,
                                Convert.ChangeType(
                                    1,
                                    range.OperandType,
                                    CultureInfo.CurrentCulture)),
                            range.OperandType,
                            CultureInfo.CurrentCulture);

                    if (maximum.CompareTo(this.rangedValue) < 0)
                    {
                        this.rangedValue = Convert.ChangeType(
                            maximum, 
                            range.OperandType,
                            CultureInfo.CurrentCulture);
                    }
                }
                else if (minimum.CompareTo(value) == 0)
                {
                    this.rangedValue = minimum;
                }
                else if (maximum.CompareTo(value) == 0)
                {
                    this.rangedValue = maximum;
                }
                else if (minimum.CompareTo(value) <= 0 && maximum.CompareTo(value) <= 0)
                {
                    this.rangedValue = minimum;
                }
                else if (minimum.CompareTo(this.rangedValue) <= 0 && maximum.CompareTo(this.rangedValue) <= 0)
                {
                    this.rangedValue = minimum;
                }
                else if (minimum.CompareTo(value) < 0)
                {
                    this.rangedValue = value;
                }
                else
                {
                    this.rangedValue = RangedNumberGenerator.Add(minimum, value);

                    if (minimum.CompareTo(this.rangedValue) > 0 ||
                        maximum.CompareTo(this.rangedValue) < 0)
                    {
                        this.rangedValue = minimum;
                    }
                }

                this.rangedValue = Convert.ChangeType(this.rangedValue, range.OperandType, CultureInfo.CurrentCulture);
            }
        }

        private static object Add(object a, object b)
        {
            switch (Type.GetTypeCode(a.GetType()))
            {
                case TypeCode.Int16:
                    return (short)((short)a + (short)b);

                case TypeCode.UInt16:
                    return (ushort)((ushort)a + (ushort)b);

                case TypeCode.Int32:
                    return (int)a + (int)b;

                case TypeCode.UInt32:
                    return (uint)a + (uint)b;

                case TypeCode.Int64:
                    return (long)a + (long)b;

                case TypeCode.UInt64:
                    return (ulong)a + (ulong)b;

                case TypeCode.Decimal:
                    return (decimal)a + (decimal)b;

                case TypeCode.Double:
                    return (double)a + (double)b;

                case TypeCode.Byte:
                    return (byte)a + (byte)b;

                case TypeCode.SByte:
                    return (sbyte)((sbyte)a + (sbyte)b);

                case TypeCode.Single:
                    return (float)a + (float)b;

                default:
                    throw new InvalidOperationException("The underlying type code of the specified types is not supported.");
            }
        }
    }
}
