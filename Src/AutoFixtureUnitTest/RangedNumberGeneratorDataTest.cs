using System;
using System.Collections;
using System.Collections.Generic;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RangedNumberGeneratorDataTest : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue:  1, expectedResult: 11);
            yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue:  2, expectedResult: 12);
            yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue:  3, expectedResult: 13);
            yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 10, expectedResult: 10);
            yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 20, expectedResult: 20);
            yield return CreateTestCase(operandType: typeof(int), minimum: 10, maximum: 20, contextValue: 21, expectedResult: 10);

            yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue:  1.0, expectedResult: 11.0);
            yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue:  2.0, expectedResult: 12.0);
            yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue:  3.0, expectedResult: 13.0);
            yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 10.0, expectedResult: 10.0);
            yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 20.0, expectedResult: 20.0);
            yield return CreateTestCase(operandType: typeof(double), minimum: 10.0, maximum: 20.0, contextValue: 21.0, expectedResult: 10.0);

            yield return CreateTestCase(operandType: typeof(long), minimum: 10, maximum: 20, contextValue:  1, expectedResult: 11);
            yield return CreateTestCase(operandType: typeof(long), minimum: 10, maximum: 20, contextValue:  2, expectedResult: 12);
            yield return CreateTestCase(operandType: typeof(long), minimum: 10, maximum: 20, contextValue:  3, expectedResult: 13);
            yield return CreateTestCase(operandType: typeof(long), minimum: 10, maximum: 20, contextValue: 10, expectedResult: 10);
            yield return CreateTestCase(operandType: typeof(long), minimum: 10, maximum: 20, contextValue: 20, expectedResult: 20);
            yield return CreateTestCase(operandType: typeof(long), minimum: 10, maximum: 20, contextValue: 21, expectedResult: 10);

            yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0, maximum: 20.0, contextValue:  1.0, expectedResult: 11.0);
            yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0, maximum: 20.0, contextValue:  2.0, expectedResult: 12.0);
            yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0, maximum: 20.0, contextValue:  3.0, expectedResult: 13.0);
            yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0, maximum: 20.0, contextValue: 10.0, expectedResult: 10.0);
            yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0, maximum: 20.0, contextValue: 20.0, expectedResult: 20.0);
            yield return CreateTestCase(operandType: typeof(decimal), minimum: 10.0, maximum: 20.0, contextValue: 21.0, expectedResult: 10.0);
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
