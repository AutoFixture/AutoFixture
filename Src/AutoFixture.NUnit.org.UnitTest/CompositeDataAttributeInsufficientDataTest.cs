using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit.org;
using Ploeh.TestTypeFoundation;

namespace Ploe.AutoFixture.NUnit.org.UnitTest
{
    [TestFixture]
    public class CompositeDataAttributeInsufficientDataTest : IEnumerable<object[]>
    {
        private readonly MethodInfo _method;
        private readonly Type[] _parameterTypes;

        public CompositeDataAttributeInsufficientDataTest()
        {
            _method = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
            var parameters = _method.GetParameters();
            _parameterTypes = (from pi in parameters
                              select pi.ParameterType).ToArray();
        }

        [Test]
        [TestCaseSource(typeof(CompositeDataAttributeInsufficientDataTest))]
        public void GetDataThrows(IEnumerable<DataAttribute> attributes)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(
                () => { new CompositeDataAttribute(attributes).GetData(_method, _parameterTypes).ToList(); }
                );
            // Teardown
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(_method, _parameterTypes, new[] { new object[] { 1, 2 } }),
                    new FakeDataAttribute(_method, _parameterTypes, new[] { new object[] { 3, 4 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(_method, _parameterTypes, new[] { new object[] { 1    } }),
                    new FakeDataAttribute(_method, _parameterTypes, new[] { new object[] { 2, 3 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(_method, _parameterTypes, new[] { new object[] { 1    } }),
                    new FakeDataAttribute(_method, _parameterTypes, new[] { new object[] {      } }),
                    new FakeDataAttribute(_method, _parameterTypes, new[] { new object[] { 2, 3 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(_method, _parameterTypes, new[] { new object[] { 1 } }),
                    new FakeDataAttribute(_method, _parameterTypes, new[] { new object[] { 2 } }),
                    new FakeDataAttribute(_method, _parameterTypes, new[] { new object[] { 3 } })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(_method, _parameterTypes, new[] {  new object[] { 1, 2    },  new object[] {  3,  4     }, new object[] { 5, 6 } }),
                    new FakeDataAttribute(_method, _parameterTypes, new[] {  new object[] { 7, 8, 9 },  new object[] { 10, 11, 12 }                        })
                }
            );

            yield return CreateTestCase(
                data: new[]
                {
                    new FakeDataAttribute(_method, _parameterTypes, new[] { new object[] {  1,  2     }, new object[] {  3,  4     }, new object[] { 5, 6 } }),
                    new FakeDataAttribute(_method, _parameterTypes, new[] { new object[] {  7,  8,  9 }, new object[] { 10, 11, 12 }                        }),
                    new FakeDataAttribute(_method, _parameterTypes, new[] { new object[] { 13, 14, 15 }                                                     })
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
