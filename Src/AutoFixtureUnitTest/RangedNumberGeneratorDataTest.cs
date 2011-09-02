using System;
using System.Collections;
using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RangedNumberGeneratorDataTest : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return CreateTestCase(operandType: typeof(int), minimum: -5, maximum: -1, contextValue: 1, expectedResult: -5);
            yield return CreateTestCase(operandType: typeof(int), minimum: -5, maximum: -1, contextValue: -1, expectedResult: -1);
            yield return CreateTestCase(operandType: typeof(int), minimum: -5, maximum: -1, contextValue: 0, expectedResult: -5);
            yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 1, expectedResult: 11);
            yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 2, expectedResult: 12);
            yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 3, expectedResult: 13);
            yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 10, expectedResult: 10);
            yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 20, expectedResult: 20);
            yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 21, expectedResult: 10);
            yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: new object(), 
                expectedResult: new NoSpecimen(new RangedNumberRequest(typeof(int), 10, 20)));

            yield return CreateTestCase(operandType: typeof(double), minimum: -5.0, maximum: -1.0, contextValue:  1.0, expectedResult: -5.0);
            yield return CreateTestCase(operandType: typeof(double), minimum: -5.0, maximum: -1.0, contextValue: -1.0, expectedResult: -1.0);
            yield return CreateTestCase(operandType: typeof(double), minimum: -5.0, maximum: -1.0, contextValue:  0.0, expectedResult: -5.0);
            yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue:  1.0, expectedResult: 11.0);
            yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue:  2.0, expectedResult: 12.0);
            yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue:  3.0, expectedResult: 13.0);
            yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 10.0, expectedResult: 10.0);
            yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 20.0, expectedResult: 20.0);
            yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 21.0, expectedResult: 10.0);
            yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: new object(), 
                expectedResult: new NoSpecimen(new RangedNumberRequest(typeof(double), 10.0, 20.0)));

            yield return CreateTestCase(operandType: typeof(long), minimum: -50000000000, maximum: -10000000000, contextValue:  10000000000, expectedResult: -50000000000);
            yield return CreateTestCase(operandType: typeof(long), minimum: -50000000000, maximum: -10000000000, contextValue: -10000000000, expectedResult: -10000000000);
            yield return CreateTestCase(operandType: typeof(long), minimum: -50000000000, maximum: -10000000000, contextValue:  10000000000, expectedResult: -50000000000);
            yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue:  10000000000, expectedResult: 110000000000);
            yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue:  20000000000, expectedResult: 120000000000);
            yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue:  30000000000, expectedResult: 130000000000);
            yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: 100000000000, expectedResult: 100000000000);
            yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: 200000000000, expectedResult: 200000000000);
            yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: 210000000000, expectedResult: 100000000000);
            yield return CreateTestCase(operandType: typeof(long), minimum: 100000000000, maximum: 200000000000, contextValue: new object(), 
                expectedResult: new NoSpecimen(new RangedNumberRequest(typeof(long), 100000000000, 200000000000)));

            yield return CreateTestCase(operandType: typeof(decimal), minimum: -5.0m, maximum: -1.0m, contextValue:  1.0m, expectedResult: -5.0m);
            yield return CreateTestCase(operandType: typeof(decimal), minimum: -5.0m, maximum: -1.0m, contextValue: -1.0m, expectedResult: -1.0m);
            yield return CreateTestCase(operandType: typeof(decimal), minimum: -5.0m, maximum: -1.0m, contextValue:  0.0m, expectedResult: -5.0m);
            yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue:  1.0m, expectedResult: 11.0m);
            yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue:  2.0m, expectedResult: 12.0m);
            yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue:  3.0m, expectedResult: 13.0m);
            yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: 10.0m, expectedResult: 10.0m);
            yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: 20.0m, expectedResult: 20.0m);
            yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: 21.0m, expectedResult: 10.0m);
            yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0m, maximum: 20.0m, contextValue: new object(), 
                expectedResult: new NoSpecimen(new RangedNumberRequest(typeof(decimal), 10.0m, 20.0m)));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static object[] CreateTestCase(Type operandType, object minimum, object maximum, object contextValue, object expectedResult)
        {
            return new object[] { operandType, minimum, maximum, contextValue, expectedResult };
        }
    }
}
