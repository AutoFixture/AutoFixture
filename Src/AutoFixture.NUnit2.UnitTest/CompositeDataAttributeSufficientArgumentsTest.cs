﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit2.Addins;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixture.NUnit2.UnitTest
{
    [TestFixture]
    public class CompositeDataAttributeSufficientArgumentsTest : IEnumerable<object[]>
    {
        private readonly MethodInfo method;
        
        public CompositeDataAttributeSufficientArgumentsTest()
        {
            this.method = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
        }

        [TestCaseSource(typeof(CompositeDataAttributeSufficientArgumentsTest))]
        public void GetArgumentsReturnsCorrectResult(IEnumerable<DataAttribute> attributes, IEnumerable<object[]> expectedResult)
        {
            // Fixture setup
            // Exercise system
            var result = new CompositeDataAttribute(attributes).GetData(this.method).ToList();
            // Verify outcome 
            Assert.True(expectedResult.SequenceEqual(result, new TheoryComparer()));
            // Teardown
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1, 2, 3 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 2, 3 } 
                    }
            );

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1, 2, 3 } }),
                        new FakeDataAttribute(this.method, new[] { new object[] { 4, 5, 6 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 2, 3 }
                    }
            );

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1, 2, 3, 4 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 2, 3 }
                    }
            );

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1       } }),
                        new FakeDataAttribute(this.method, new[] { new object[] { 2, 3, 4 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 3, 4 }
                    }
            );

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1, 2    } }),
                        new FakeDataAttribute(this.method, new[] { new object[] { 3, 4, 5 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 2, 5 }
                    }
            );

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 }
                    }
            );

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1, 2, 3 }, new object[] { 4,  5, 6 }                          }),
                        new FakeDataAttribute(this.method, new[] { new object[] { 7, 8    }, new object[] { 9, 10    }, new object[] { 11, 12 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 }
                    }
            );

            yield return CreateTestCase(
                data: new[]
                    {
                        new FakeDataAttribute(this.method, new[] { new object[] { 1, 2    }, new object[] {  3,  4     }, new object[] {  5,  6     } }),
                        new FakeDataAttribute(this.method, new[] { new object[] { 7, 8, 9 }, new object[] { 10, 11, 12 }, new object[] { 13, 14, 15 } })
                    },
                expected: new[] 
                    {
                        new object[] { 1, 2, 9 }, new object[] { 3, 4, 12 }, new object[] { 5, 6, 15 }
                    }
            );
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static object[] CreateTestCase(object[] data, object[] expected)
        {
            return new object[] { data, expected };
        }

        private sealed class TheoryComparer : IEqualityComparer<object[]>
        {
            public bool Equals(object[] x, object[] y)
            {
                return x.SequenceEqual(y);
            }

            public int GetHashCode(object[] array)
            {
                return (from obj in array select obj.GetHashCode()).Aggregate((x, y) => x ^ y);
            }
        }
    }
}
