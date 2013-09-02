using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.NUnit.UnitTest
{
    public class CompositeTestCaseDataAttributeInsufficientArgumentsTest : IEnumerable<object[]>
    {
        private readonly MethodInfo method;
        
        public CompositeTestCaseDataAttributeInsufficientArgumentsTest()
        {
            this.method = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
        }

        [Theory]
        [ClassData(typeof(CompositeTestCaseDataAttributeInsufficientArgumentsTest))]
        public void GetArgumentsThrows(IEnumerable<TestCaseDataAttribute> attributes)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(
                () => { new CompositeTestCaseDataAttribute(attributes).GetArguments(this.method).ToList(); }
                );
            // Teardown
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return CreateTestCase(
                data: new[]
                {
                    new FakeTestCaseDataAttribute(this.method, new[] { new object[] { 1, 2 } }),
                    new FakeTestCaseDataAttribute(this.method, new[] { new object[] { 3, 4 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeTestCaseDataAttribute(this.method, new[] { new object[] { 1    } }),
                    new FakeTestCaseDataAttribute(this.method, new[] { new object[] { 2, 3 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeTestCaseDataAttribute(this.method, new[] { new object[] { 1    } }),
                    new FakeTestCaseDataAttribute(this.method, new[] { new object[] {      } }),
                    new FakeTestCaseDataAttribute(this.method, new[] { new object[] { 2, 3 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeTestCaseDataAttribute(this.method, new[] { new object[] { 1 } }),
                    new FakeTestCaseDataAttribute(this.method, new[] { new object[] { 2 } }),
                    new FakeTestCaseDataAttribute(this.method, new[] { new object[] { 3 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeTestCaseDataAttribute(this.method, new[] {  new object[] { 1, 2    },  new object[] {  3,  4     }, new object[] { 5, 6 } }),
                    new FakeTestCaseDataAttribute(this.method, new[] {  new object[] { 7, 8, 9 },  new object[] { 10, 11, 12 }                        })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeTestCaseDataAttribute(this.method, new[] { new object[] {  1,  2     }, new object[] {  3,  4     }, new object[] { 5, 6 } }),
                    new FakeTestCaseDataAttribute(this.method, new[] { new object[] {  7,  8,  9 }, new object[] { 10, 11, 12 }                        }),
                    new FakeTestCaseDataAttribute(this.method, new[] { new object[] { 13, 14, 15 }                                                     })
                }
            );
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
}
