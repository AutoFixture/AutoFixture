using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.NUnit2.Addins;
using NUnit.Framework;
using TestTypeFoundation;

namespace AutoFixture.NUnit2.UnitTest;

[TestFixture]
public class CompositeDataAttributeInsufficientArgumentsTest : IEnumerable<object[]>
{
    private readonly MethodInfo _method;

    public CompositeDataAttributeInsufficientArgumentsTest()
    {
        _method = typeof(TypeWithOverloadedMembers)
            .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
    }

    [TestCaseSource(typeof(CompositeDataAttributeInsufficientArgumentsTest))]
    public void GetArgumentsThrows(IEnumerable<DataAttribute> attributes)
    {
        // Arrange
        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => { new CompositeDataAttribute(attributes).GetData(_method).ToList(); });
    }

    public IEnumerator<object[]> GetEnumerator()
    {
#pragma warning disable SA1025 // Code should not contain multiple whitespace in a row
        yield return CreateTestCase(
            data: new[]
            {
                new FakeDataAttribute(_method, new[] { new object[] { 1, 2 } }),
                new FakeDataAttribute(_method, new[] { new object[] { 3, 4 } })
            });

        yield return CreateTestCase(
            data: new[]
            {
                new FakeDataAttribute(_method, new[] { new object[] { 1    } }),
                new FakeDataAttribute(_method, new[] { new object[] { 2, 3 } })
            });

        yield return CreateTestCase(
            data: new[]
            {
                new FakeDataAttribute(_method, new[] { new object[] { 1    } }),
                new FakeDataAttribute(_method, new[] { new object[] {      } }),
                new FakeDataAttribute(_method, new[] { new object[] { 2, 3 } })
            });

        yield return CreateTestCase(
            data: new[]
            {
                new FakeDataAttribute(_method, new[] { new object[] { 1 } }),
                new FakeDataAttribute(_method, new[] { new object[] { 2 } }),
                new FakeDataAttribute(_method, new[] { new object[] { 3 } })
            });

        yield return CreateTestCase(
            data: new[]
            {
                new FakeDataAttribute(_method, new[] {  new object[] { 1, 2    },  new object[] {  3,  4     }, new object[] { 5, 6 } }),
                new FakeDataAttribute(_method, new[] {  new object[] { 7, 8, 9 },  new object[] { 10, 11, 12 }                        })
            });

        yield return CreateTestCase(
            data: new[]
            {
                new FakeDataAttribute(_method, new[] { new object[] {  1,  2     }, new object[] {  3,  4     }, new object[] { 5, 6 } }),
                new FakeDataAttribute(_method, new[] { new object[] {  7,  8,  9 }, new object[] { 10, 11, 12 }                        }),
                new FakeDataAttribute(_method, new[] { new object[] { 13, 14, 15 }                                                     })
            });
#pragma warning restore SA1025 // Code should not contain multiple whitespace in a row
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private static object[] CreateTestCase(object[] data)
    {
        return new object[] { data };
    }
}