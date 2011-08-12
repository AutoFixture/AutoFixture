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
    public class CompositeDataAttributeTest
    {
        [Fact]
        public void SutIsDataAttribute()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeDataAttribute();
            // Verify outcome
            Assert.IsAssignableFrom<DataAttribute>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeDataAttribute(null));
            // Teardown
        }

        [Fact]
        public void AttributesIsCorrectWhenInitializedWithArray()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var attributes = new[]
            {
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeDataAttribute(attributes);
            // Exercise system
            IEnumerable<DataAttribute> result = sut.Attributes;
            // Verify outcome
            Assert.True(attributes.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeDataAttribute((IEnumerable<DataAttribute>)null));
            // Teardown
        }

        [Fact]
        public void AttributesIsCorrectWhenInitializedWithEnumerable()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var attributes = new[]
            {
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
                new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>())
            };

            var sut = new CompositeDataAttribute(attributes);
            // Exercise system
            var result = sut.Attributes;
            // Verify outcome
            Assert.True(attributes.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void GetDataWithNullMethodThrows()
        {
            // Fixture setup
            var sut = new CompositeDataAttribute();
            var dummyTypes = Type.EmptyTypes;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(null, dummyTypes).ToList());
            // Teardown
        }

        [Fact]
        public void GetDataWithNullTypesThrows()
        {
            // Fixture setup
            var sut = new CompositeDataAttribute();
            Action a = delegate { };
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.GetData(a.Method, null).ToList());
            // Teardown
        }

        public void GetDataOnMethodWithNoParametersReturnsNoTheory()
        {
            // Fixture setup
            Action a = delegate { };
            var method = a.Method;
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();

            var sut = new CompositeDataAttribute(
               new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>()),
               new FakeDataAttribute(method, parameterTypes, Enumerable.Empty<object[]>())
               );

            // Exercise system and verify outcome
            var result = sut.GetData(a.Method, Type.EmptyTypes);
            Array.ForEach(result.ToArray(), Assert.Empty);
            // Teardown
        }

        [Theory]
        [ClassData(typeof(SufficientData))]
        public void GetDataReturnsCorrectResult(IEnumerable<DataAttribute> attributes, IEnumerable<object[]> expectedResult)
        {
            // Fixture setup
            var method = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();
            // Exercise system
            var result = new CompositeDataAttribute(attributes).GetData(method, parameterTypes).ToList();
            // Verify outcome 
            Assert.True(expectedResult.SequenceEqual(result, new TheoryComparer()));
            // Teardown
        }

        [Theory]
        [ClassData(typeof(InsufficientData))]
        public void GetDataThrows(IEnumerable<DataAttribute> attributes)
        {
            // Fixture setup
            var method = typeof(TypeWithOverloadedMembers)
                .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
            var parameters = method.GetParameters();
            var parameterTypes = (from pi in parameters
                                  select pi.ParameterType).ToArray();
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(
                () => { new CompositeDataAttribute(attributes).GetData(method, parameterTypes).ToList(); }
                );
            // Teardown
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

        private sealed class SufficientData : IEnumerable<object[]>
        {
            private readonly MethodInfo method;
            private readonly Type[] parameterTypes;

            public SufficientData()
            {
                this.method = typeof(TypeWithOverloadedMembers)
                    .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
                var parameters = method.GetParameters();
                this.parameterTypes = (from pi in parameters
                                       select pi.ParameterType).ToArray();

            }

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return OneTestCase1();
                yield return OneTestCase2();
                yield return OneTestCase3();
                yield return OneTestCase4();
                yield return OneTestCase5();

                yield return ManyTestCase1();
                yield return ManyTestCase2();
                yield return ManyTestCase3();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private object[] OneTestCase1()
            {
                return new object[] 
                { 
                    new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2, 3 } })
                    }, 
                    new[] 
                    {
                        new object[] { 1, 2, 3 } 
                    } 
                };
            }

            private object[] OneTestCase2()
            {
                return new object[] 
                { 
                    new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2, 3 } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 4, 5, 6 } })
                    }, 
                    new[] 
                    {
                        new object[] { 1, 2, 3 }
                    } 
                };
            }

            private object[] OneTestCase3()
            {
                return new object[] 
                { 
                    new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2, 3, 4 } })
                    }, 
                    new[] 
                    {
                        new object[] { 1, 2, 3 }
                    } 
                };
            }

            private object[] OneTestCase4()
            {
                return new object[] 
                { 
                    new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1       } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 2, 3, 4 } })
                    }, 
                    new[] 
                    {
                        new object[] { 1, 3, 4 }
                    } 
                };
            }

            private object[] OneTestCase5()
            {
                return new object[] 
                { 
                    new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2    } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 3, 4, 5 } })
                    }, 
                    new[] 
                    {
                        new object[] { 1, 2, 5 }
                    } 
                };
            }

            private object[] ManyTestCase1()
            {
                return new object[] 
                { 
                    new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 } })
                    }, 
                    new[] 
                    {
                        new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 }
                    } 
                };
            }

            private object[] ManyTestCase2()
            {
                return new object[] 
                { 
                    new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2, 3 }, new object[] { 4,  5, 6 }                          }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 7, 8    }, new object[] { 9, 10    }, new object[] { 11, 12 } })
                    }, 
                    new[] 
                    {
                        new object[] { 1, 2, 3 }, new object[] { 4, 5, 6 }
                    } 
                };
            }

            private object[] ManyTestCase3()
            {
                return new object[] 
                { 
                    new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2    }, new object[] {  3,  4     }, new object[] {  5,  6     } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 7, 8, 9 }, new object[] { 10, 11, 12 }, new object[] { 13, 14, 15 } })
                    }, 
                    new[] 
                    {
                        new object[] { 1, 2, 9 }, new object[] { 3, 4, 12 }, new object[] { 5, 6, 15 }
                    } 
                };
            }
        }

        private sealed class InsufficientData : IEnumerable<object[]>
        {
            private readonly MethodInfo method;
            private readonly Type[] parameterTypes;

            public InsufficientData()
            {
                this.method = typeof(TypeWithOverloadedMembers)
                    .GetMethod("DoSomething", new[] { typeof(object), typeof(object), typeof(object) });
                var parameters = method.GetParameters();
                this.parameterTypes = (from pi in parameters
                                       select pi.ParameterType).ToArray();
            }

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return OneTestCase1();
                yield return OneTestCase2();
                yield return OneTestCase3();
                yield return OneTestCase4();

                yield return ManyTestCase1();
                yield return ManyTestCase2();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private object[] OneTestCase1()
            {
                return new object[] 
                { 
                    new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1, 2 } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 3, 4 } })
                    }
                };
            }

            private object[] OneTestCase2()
            {
                return new object[] 
                { 
                    new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1    } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 2, 3 } })
                    }
                };
            }

            private object[] OneTestCase3()
            {
                return new object[] 
                { 
                    new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1    } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] {      } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 2, 3 } })
                    }
                };
            }

            private object[] OneTestCase4()
            {
                return new object[] 
                { 
                    new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 1 } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 2 } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 3 } })
                    }
                };
            }

            private object[] ManyTestCase1()
            {
                return new object[] 
                { 
                    new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] {  new object[] { 1, 2    },  new object[] {  3,  4     }, new object[] { 5, 6 } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] {  new object[] { 7, 8, 9 },  new object[] { 10, 11, 12 }                        })
                    }
                };
            }

            private object[] ManyTestCase2()
            {
                return new object[] 
                { 
                    new[]
                    {
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] {  1,  2     }, new object[] {  3,  4     }, new object[] { 5, 6 } }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] {  7,  8,  9 }, new object[] { 10, 11, 12 }                        }),
                        new FakeDataAttribute(this.method, this.parameterTypes, new[] { new object[] { 13, 14, 15 }                                                     })
                    }
                };
            }
        }
    }
}