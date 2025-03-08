using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture.TUnit.UnitTest.TestTypes;
using TestTypeFoundation;

namespace AutoFixture.TUnit.UnitTest;

public class CompositeDataAttributeSufficientDataTest
{
    private readonly MethodInfo method = typeof(TypeWithOverloadedMembers)
        .GetMethod(nameof(TypeWithOverloadedMembers.DoSomething),
            [typeof(object), typeof(object), typeof(object)]);

    [Test]
    [MethodDataSource(typeof(CompositeDataAttributeSufficientDataTest), nameof(GetEnumerator))]
    public async Task GetDataReturnsCorrectResult(IEnumerable<AutoFixtureDataSourceAttribute> attributes,
        IEnumerable<object[]> expectedResult)
    {
        // Arrange
        var attribute = new CompositeDataAttribute(attributes.ToArray());

        // Act
        var result = attribute.GenerateDataSources(DataGeneratorMetadataHelper.CreateDataGeneratorMetadata(this.method))
            .Select(x => x())
            .ToArray();

        // Assert
        await Assert.That(result).IsEquivalentTo(expectedResult);
    }

    public IEnumerable<(IEnumerable<AutoFixtureDataSourceAttribute> attributes,
        IEnumerable<object[]> expectedResult)> GetEnumerator()
    {
        yield return CreateTestData(
            data:
            [
                new FakeDataAttribute(this.method, [[1, 2, 3]])
            ],
            expected:
            [
                [1, 2, 3]
            ]);

        yield return CreateTestData(
            data:
            [
                new FakeDataAttribute(this.method, [[1, 2, 3]]),
                new FakeDataAttribute(this.method, [[4, 5, 6]])
            ],
            expected:
            [
                [1, 2, 3]
            ]);

        yield return CreateTestData(
            data:
            [
                new FakeDataAttribute(this.method, [[1]]),
                new FakeDataAttribute(this.method, [[2, 3, 4]])
            ],
            expected:
            [
                [1, 3, 4]
            ]);

        yield return CreateTestData(
            data:
            [
                new FakeDataAttribute(this.method, [[1, 2]]),
                new FakeDataAttribute(this.method, [[3, 4, 5]])
            ],
            expected:
            [
                [1, 2, 5]
            ]);

        yield return CreateTestData(
            data:
            [
                new FakeDataAttribute(this.method, [[1, 2, 3], [4, 5, 6]])
            ],
            expected:
            [
                [1, 2, 3],
                [4, 5, 6]
            ]);

        yield return CreateTestData(
            data:
            [
                new FakeDataAttribute(this.method, [[1, 2, 3], [4, 5, 6]]),
                new FakeDataAttribute(this.method,
                    [[7, 8], [9, 10], [11, 12]])
            ],
            expected:
            [
                [1, 2, 3],
                [4, 5, 6]
            ]);

        yield return CreateTestData(
            data:
            [
                new FakeDataAttribute(this.method,
                    [[1, 2], [3, 4], [5, 6]]),
                new FakeDataAttribute(this.method,
                    [[7, 8, 9], [10, 11, 12], [13, 14, 15]])
            ],
            expected:
            [
                [1, 2, 9],
                [3, 4, 12],
                [5, 6, 15]
            ]);

        // Second attribute restricts
        yield return CreateTestData(
            data:
            [
                new FakeDataAttribute(this.method, [[1, 2, 3], [4, 5, 6]]),
                new FakeDataAttribute(this.method, [[7, 8, 9]])
            ],
            expected:
            [
                [1, 2, 3]
            ]);

        // Shortest data provider is limiting factor
        yield return CreateTestData(
            data:
            [
                new FakeDataAttribute(this.method, [[1, 2, 3]]),
                new FakeDataAttribute(this.method, [[4, 5, 6], [7, 8, 9]])
            ],
            expected:
            [
                [1, 2, 3]
            ]);

        // Test incorrect number of parameters - should just return what it's given
        // and let xUnit deal with counting parameters
        yield return CreateTestData(
            data:
            [
                new FakeDataAttribute(this.method, [[1, 2]]),
                new FakeDataAttribute(this.method, [[3, 4]])
            ],
            expected:
            [
                [1, 2]
            ]);

        yield return CreateTestData(
            data:
            [
                new FakeDataAttribute(this.method, [[1]]),
                new FakeDataAttribute(this.method, [[2, 3]])
            ],
            expected:
            [
                [1, 3]
            ]);

        yield return CreateTestData(
            data:
            [
                new FakeDataAttribute(this.method, [[1]]),
                new FakeDataAttribute(this.method, [[]]),
                new FakeDataAttribute(this.method, [[2, 3]])
            ],
            expected:
            [
                [1, 3]
            ]);

        yield return CreateTestData(
            data:
            [
                new FakeDataAttribute(this.method, [[1]]),
                new FakeDataAttribute(this.method, [[2]]),
                new FakeDataAttribute(this.method, [[3]])
            ],
            expected:
            [
                [1]
            ]);

        yield return CreateTestData(
            data:
            [
                new FakeDataAttribute(this.method, [[1, 2, 3, 4]])
            ],
            expected:
            [
                [1, 2, 3, 4]
            ]);
    }

    private static (IEnumerable<AutoFixtureDataSourceAttribute> attributes,
        IEnumerable<object[]> expectedResult) CreateTestData(AutoFixtureDataSourceAttribute[] data, object[][] expected)
    {
        return (data, expected);
    }

    private sealed class TheoryComparer : IEqualityComparer<object[]>
    {
        public bool Equals(object[] x, object[] y)
        {
            return x!.SequenceEqual(y!);
        }

        public int GetHashCode(object[] array)
        {
            return array.Select(obj => obj.GetHashCode()).Aggregate((x, y) => x ^ y);
        }
    }
}