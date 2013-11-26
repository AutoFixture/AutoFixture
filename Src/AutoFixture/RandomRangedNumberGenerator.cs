using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    
     /// <summary>
     /// Creates a random sequence within a given range without repeating in the range until all values are exhausted.
     /// Once exhausted, will automatically reset the set and continue choosing randomly within the range
     /// </summary>
    public class RandomRangedNumberGenerator : ISpecimenBuilder
    {
        private readonly IDictionary<RangedNumberRequest, RandomNumericSequenceGenerator> generatorMap;
       
        public RandomRangedNumberGenerator()
        {
            this.generatorMap = new Dictionary<RangedNumberRequest, RandomNumericSequenceGenerator>();          
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
                return new NoSpecimen();

            if (context == null)
                throw new ArgumentNullException("context");

            var rangedNumberRequest = request as RangedNumberRequest;
            
            if (rangedNumberRequest == null)
                return new NoSpecimen(request);

            if (!LimitsAreNumeric(rangedNumberRequest))
                return new NoSpecimen(request);

            try
            {
                return SelectGenerator(rangedNumberRequest).Create(rangedNumberRequest.OperandType, context);                
            }
            catch (Exception ex)
            {
                return new NoSpecimen(request);
            }
        }

        /// <summary>
        /// Choose the RandomNumericSequenceGenerator to fulfill the request.  Will add the request as a new key
        /// to generatorMap if the request does not already have a generator for it.  Uses minimum and maximum
        /// from the request to set up the generator or [0, Byte.MaxValue] if either is non-numeric. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private RandomNumericSequenceGenerator SelectGenerator(RangedNumberRequest request)
        {
            if (!this.generatorMap.ContainsKey(request))
            {
                var limits = ConvertLimits(request.Minimum, request.Maximum);
                this.generatorMap.Add(request, new RandomNumericSequenceGenerator(limits));
            }

            return this.generatorMap[request];

        }

        /// <summary>
        /// Returns true if both Minimum and Maximum for the request are numeric types, false otherwise. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool LimitsAreNumeric(RangedNumberRequest request)
        {
            var validTypes = new List<Type> { typeof(int), typeof(byte), typeof(short), typeof(long), typeof(decimal), 
               typeof(double), typeof(ushort), typeof(uint), typeof(ulong), typeof(sbyte), typeof(float) };


            return (validTypes.Contains(request.Minimum.GetType()) && validTypes.Contains(request.Maximum.GetType()));
        }


        /// <summary>
        /// Converts provided minimum and maximum into a long array of size 2.  Returns [0, Byte.MaxValue] if
        /// minimum or maximum are non-numeric. 
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        private long[] ConvertLimits(object minimum, object maximum)
        {
            try
            {
                return new long[] { ConvertLimit(minimum), ConvertLimit(maximum) };
            }
            catch
            {
                return new long[] { 0, Byte.MaxValue };
            }

        }

        /// <summary>
        /// Converts the provided limit into an Int64.  Throws ArgumentException if limit is a non-numeric type.
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        private long ConvertLimit(object limit)
        {
            var limitType = limit.GetType();

            switch (Type.GetTypeCode(limitType))
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

            throw new ArgumentException("limit");           
        }               
        
    }

    
}
