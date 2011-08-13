using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.Xunit.UnitTest
{
    public class CompositeDataAttributeInsufficientData : IEnumerable<object[]>
    {
        private readonly MethodInfo method;
        private readonly Type[] parameterTypes;

        public CompositeDataAttributeInsufficientData()
        {
            this.method = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
            var parameters = method.GetParameters();
            this.parameterTypes = (from pi in parameters
                                    select pi.ParameterType).ToArray();
        }

        [Theory]
        [ClassData(typeof(CompositeDataAttributeInsufficientData))]
        public void GetDataThrows(IEnumerable<DataAttribute> attributes)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(
                () => { new CompositeDataAttribute(attributes).GetData(this.method, this.parameterTypes).ToList(); }
                );
            // Teardown
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2 } }),
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 3, 4 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1    } }),
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 2, 3 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1    } }),
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] {      } }),
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 2, 3 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1 } }),
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 2 } }),
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 3 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] {  new object[] { 1, 2    },  new object[] {  3,  4     }, new object[] { 5, 6 } }),
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] {  new object[] { 7, 8, 9 },  new object[] { 10, 11, 12 }                        })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] {  1,  2     }, new object[] {  3,  4     }, new object[] { 5, 6 } }),
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] {  7,  8,  9 }, new object[] { 10, 11, 12 }                        }),
                    new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 13, 14, 15 }                                                     })
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
