using Ploeh.TestTypeFoundation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace Ploeh.AutoFixture.Xunit2.UnitTest
{
    public class CompositeDataAttributeInsufficientDataTest : IEnumerable<object[]>
    {
        private readonly MethodInfo method;

        public CompositeDataAttributeInsufficientDataTest()
        {
            this.method = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
        }

        [Theory]
        [ClassData(typeof(CompositeDataAttributeInsufficientDataTest))]
        public void GetDataThrows(IEnumerable<DataAttribute> attributes)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(
                () => { new CompositeDataAttribute(attributes).GetData(this.method).ToList(); }
                );
            // Teardown
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method,  new[] { new object[] { 1, 2 } }),
                    new FakeDataAttribute(this.method,  new[] { new object[] { 3, 4 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method,  new[] { new object[] { 1    } }),
                    new FakeDataAttribute(this.method,  new[] { new object[] { 2, 3 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method,  new[] { new object[] { 1    } }),
                    new FakeDataAttribute(this.method,  new[] { new object[] {      } }),
                    new FakeDataAttribute(this.method,  new[] { new object[] { 2, 3 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method,  new[] { new object[] { 1 } }),
                    new FakeDataAttribute(this.method,  new[] { new object[] { 2 } }),
                    new FakeDataAttribute(this.method,  new[] { new object[] { 3 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method,  new[] {  new object[] { 1, 2    },  new object[] {  3,  4     }, new object[] { 5, 6 } }),
                    new FakeDataAttribute(this.method,  new[] {  new object[] { 7, 8, 9 },  new object[] { 10, 11, 12 }                        })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method,  new[] { new object[] {  1,  2     }, new object[] {  3,  4     }, new object[] { 5, 6 } }),
                    new FakeDataAttribute(this.method,  new[] { new object[] {  7,  8,  9 }, new object[] { 10, 11, 12 }                        }),
                    new FakeDataAttribute(this.method,  new[] { new object[] { 13, 14, 15 }                                                     })
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