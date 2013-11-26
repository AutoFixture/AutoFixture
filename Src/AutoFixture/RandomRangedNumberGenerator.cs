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


        private RandomNumericSequenceGenerator SelectGenerator(RangedNumberRequest request)
        {
            if (!this.generatorMap.ContainsKey(request))
            {  
                long minimum = ConvertLimit(request.Minimum);
                long maximum = ConvertLimit(request.Maximum);

                this.generatorMap.Add(request, new RandomNumericSequenceGenerator(minimum, maximum));
            }

            return this.generatorMap[request];

        }


        private bool LimitsAreNumeric(RangedNumberRequest request)
        {
            var validTypes = new List<Type> { typeof(int), typeof(byte), typeof(short), typeof(long), typeof(decimal), 
               typeof(double), typeof(ushort), typeof(uint), typeof(ulong), typeof(sbyte), typeof(float) };


            return (validTypes.Contains(request.Minimum.GetType()) && validTypes.Contains(request.Maximum.GetType()));
        }

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

            return 0;
           
        }
        
    }

    
}
