using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    
     /// <summary>
     /// Creates a random sequence for a given type within a given range without repeating in the range until 
     /// all values are exhausted. Once exhausted, will automatically reset the set and continue choosing randomly 
     /// within the range.  Multiple requests (whether the same or different object) for the same
     /// operand type, minimum, and maximum are treated as being drawn from the same set. 
     /// </summary>
    public class RandomRangedNumberGenerator : ISpecimenBuilder
    {        
        private readonly ConcurrentDictionary<RangedNumberRequest, RandomNumericSequenceGenerator> generatorMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomRangedNumberGenerator" /> class       
        /// </summary>
        public RandomRangedNumberGenerator()
        {           
            this.generatorMap = new ConcurrentDictionary<RangedNumberRequest, RandomNumericSequenceGenerator>();                  
        }

        /// <summary>
        /// Creates a random number within the request range.
        /// </summary>
        /// <param name="request">The request that describes what to create. Other requests for same type and limits 
        /// denote the same set. </param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The next random number in a range <paramref name="request"/> is a request
        /// for a numeric value; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
                return new NoSpecimen();

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var rangedNumberRequest = request as RangedNumberRequest;
            
            if (rangedNumberRequest == null)
                return new NoSpecimen();

            try
            {
                return SelectGenerator(rangedNumberRequest).Create(rangedNumberRequest.OperandType, context);
            }
            catch (ArgumentException)
            {
                return new NoSpecimen();
            }
        }

        /// <summary>
        /// Choose the RandomNumericSequenceGenerator to fulfill the request.  Will add the request as a new key
        /// to generatorMap if the request does not already have a generator for it.  Throws ArgumentException
        /// if either of the limits in the request are non-numeric. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private RandomNumericSequenceGenerator SelectGenerator(RangedNumberRequest request)
        {  
            return this.generatorMap.GetOrAdd(request, _ => 
                {                    
                    return new RandomNumericSequenceGenerator(ConvertLimits(request.Minimum, request.Maximum));
                });
        }

       
        /// <summary>
        /// Converts provided minimum and maximum into a long array of size 2.  Throws ArgumentException
        /// if either value is non-numeric or otherwise fails conversion. 
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        private static long[] ConvertLimits(object minimum, object maximum)
        {            
            return new long[] { ConvertLimit(minimum), ConvertLimit(maximum) };           
        }

        /// <summary>
        /// Converts the provided limit into an Int64.  Throws ArgumentException if limit is a non-numeric type.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        private static long ConvertLimit(object limit)
        {
            switch (GetTypeCode(limit))
            {
                case TypeCode.Byte:                   
                        return (long)(byte)limit;

                case TypeCode.Decimal:
                        return (long)(decimal)limit;

                case TypeCode.Double:
                        return (long)(double)limit;

                case TypeCode.Int16:
                        return (long)(short)limit;

                case TypeCode.Int32:
                        return (long)(int)limit;

                case TypeCode.Int64:
                        return (long)limit;

                case TypeCode.SByte:
                        return (long)(sbyte)limit;

                case TypeCode.Single:
                        return (long)(float)limit;

                case TypeCode.UInt16:
                        return (long)(ushort)limit;

                case TypeCode.UInt32:
                        return (long)(uint)limit;

                case TypeCode.UInt64:
                        return (long)(ulong)limit;
            }

            throw new ArgumentException("Limit parameter is non-numeric ", nameof(limit));         
        }

        private static TypeCode GetTypeCode(object request)
        {
            var convertible = request as IConvertible;
            if (convertible == null)
                return TypeCode.Object;

            return convertible.GetTypeCode();
        }
    }
}
