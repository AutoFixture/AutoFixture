using System;
using System.Collections.Generic;
using System.Linq;
using Grean.Exude;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class MultidimensionalArrayRelayTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            var sut = new MultidimensionalArrayRelay();
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
        }

        [Fact]
        public void CreateWithNullContextThrows()
        {
            var sut = new MultidimensionalArrayRelay();
            var dummyRequest = new object();
            Assert.Throws<ArgumentNullException>(() => sut.Create(dummyRequest, null));
        }

        [FirstClassTests]
        public static IEnumerable<ITestCase> CreateWithInvalidRequestReturnsNoSpecimen()
        {
            var invalidRequests = new[]
            {
                null,
                new object(),
                string.Empty,
                123,
                typeof(int),
                typeof(object[]),
                typeof(string[][])
            };
            return invalidRequests.Select(r => new TestCase(() =>
            {
                var sut = new MultidimensionalArrayRelay();
                var dummyContext = new DelegatingSpecimenContext();
                var expected = new NoSpecimen(r);

                var actual = sut.Create(r, dummyContext);

                Assert.Equal(expected, actual);
            }));
        }

        [FirstClassTests]
        public static IEnumerable<ITestCase> Create2DimensionalArrayReturnsCorrectResult()
        {
            var testCase = new[]
            {
                new // 2 dimension - 0 length
                {
                    Jagged = new[]
                    {
                        new int[0]
                    },
                    Expected = new int[,]
                    {
                    }
                },
                new // 2 dimension - 1 length
                {
                    Jagged = new[]
                    {
                        new[] { 1 }
                    },
                    Expected = new[,]
                    {
                        { 1 }
                    }
                },
                new // 2 dimension - 2 length
                {
                    Jagged = new[]
                    {
                        new[] { 1, 2 },
                        new[] { 3, 4 }
                    },
                    Expected = new[,]
                    {
                        { 1, 2 },
                        { 3, 4 }
                    }
                },
                new // 2 dimension - 3 length
                {
                    Jagged = new[]
                    {
                        new[] { 1, 2, 3 },
                        new[] { 11, 12, 13 },
                        new[] { 211, 212, 213 }
                    },
                    Expected = new[,]
                    {
                        { 1, 2, 3 },
                        { 11, 12, 13 },
                        { 211, 212, 213 }
                    }
                },
                new // 2 dimension - 4 length
                {
                    Jagged = new[]
                    {
                        new[] { 1, 2, 3, 4 },
                        new[] { 11, 12, 13, 34 },
                        new[] { 211, 212, 213, 23 },
                        new[] { 3, 4, 5, 6 }
                    },
                    Expected = new[,]
                    {
                        { 1, 2, 3, 4 },
                        { 11, 12, 13, 34 },
                        { 211, 212, 213, 23 },
                        { 3, 4, 5, 6 }
                    }
                }
            };
            return testCase.Select(c => new TestCase(() =>
            {
                var sut = new MultidimensionalArrayRelay();
                var context = new DelegatingSpecimenContext
                {
                    OnResolve = r =>
                    {
                        Assert.Equal(c.Jagged.GetType(), r);
                        return c.Jagged;
                    }
                };

                var actual = sut.Create(typeof(int[,]), context);

                Assert.Equal(c.Expected, actual);
            }));
        }

        [FirstClassTests]
        public static IEnumerable<ITestCase> Create3DimensionalArrayReturnsCorrectResult()
        {
            var testCase = new[]
            {
                new // 3 dimension - 0 length
                {
                    Jagged = new[]
                    {
                        new[]
                        {
                            new int[0]
                        }
                    },
                    Expected = new int[,,]
                    {
                    }
                },
                new // 3 dimension - 1 length
                {
                    Jagged = new[]
                    {
                        new[]
                        {
                            new[]
                            {
                                12
                            }
                        }
                    },
                    Expected = new[,,]
                    {
                        {
                            { 12 }
                        }
                    }
                },
                new // 3 dimension - 2 length
                {
                    Jagged = new[]
                    {
                        new[]
                        {
                            new[] { 1, 2 },
                            new[] { 3, 4 }
                        },
                        new[]
                        {
                            new[] { 31, 32 },
                            new[] { 33, 34 }
                        }
                    },
                    Expected = new[,,]
                    {
                        {
                            { 1, 2 },
                            { 3, 4 }
                        },
                        {
                            { 31, 32 },
                            { 33, 34 }
                        }
                    }
                },
                new // 3 dimension - 3 length
                {
                    Jagged = new[]
                    {
                        new[]
                        {
                            new[] { 1, 2, 4 },
                            new[] { 2, 4, 6 },
                            new[] { 3, 5, 7 }
                        },
                        new[]
                        {
                            new[] { 31, 32, 2 },
                            new[] { 33, 34, 4 },
                            new[] { 45, 34, 342 }
                        },
                        new[]
                        {
                            new[] { 1, 2, 3 },
                            new[] { 4, 5, 6 },
                            new[] { 7, 8, 9 }
                        }
                    },
                    Expected = new[,,]
                    {
                        {
                            { 1, 2, 4 },
                            { 2, 4, 6 },
                            { 3, 5, 7 }
                        },
                        {
                            { 31, 32, 2 },
                            { 33, 34, 4 },
                            { 45, 34, 342 }
                        },
                        {
                            { 1, 2, 3 },
                            { 4, 5, 6 },
                            { 7, 8, 9 }
                        }
                    }
                }
            };
            return testCase.Select(c => new TestCase(() =>
            {
                var sut = new MultidimensionalArrayRelay();
                var context = new DelegatingSpecimenContext
                {
                    OnResolve = r =>
                    {
                        Assert.Equal(c.Jagged.GetType(), r);
                        return c.Jagged;
                    }
                };

                var actual = sut.Create(typeof(int[,,]), context);

                Assert.Equal(c.Expected, actual);
            }));
        }

        [Fact]
        public void Create4DimensionalArrayReturnsCorrectResult()
        {
            // 4 dimension - 2 length
            var jagged = new[]
            {
                new[]
                {
                    new[]
                    {
                        new[] { 11, 22 },
                        new[] { 33, 44 }
                    },
                    new[]
                    {
                        new[] { 531, 632 },
                        new[] { 733, 834 }
                    }
                },
                new[]
                {
                    new[]
                    {
                        new[] { 1, 2 },
                        new[] { 3, 4 }
                    },
                    new[]
                    {
                        new[] { 31, 32 },
                        new[] { 33, 34 }
                    }
                }
            };

            var expected = new[,,,]
            {
                {
                    {
                        { 11, 22 },
                        { 33, 44 }
                    },
                    {
                        { 531, 632 },
                        { 733, 834 }
                    }
                },
                {
                    {
                        { 1, 2 },
                        { 3, 4 }
                    },
                    {
                        { 31, 32 },
                        { 33, 34 }
                    }
                }
            };

            var sut = new MultidimensionalArrayRelay();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(jagged.GetType(), r);
                    return jagged;
                }
            };

            var actual = sut.Create(typeof(int[,,,]), context);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateComplexArrayReturnsCorrectResult()
        {
            int[][][] jagged =
            {
                new[]
                {
                    new[] { 1, 2 },
                    new[] { 3, 4 }
                },
                new[]
                {
                    new[] { 31, 32 },
                    new[] { 33, 34 }
                }
            };

            int[,][] expected =
            {
                {
                    new[] { 1, 2 },
                    new[] { 3, 4 }
                },
                {
                    new[] { 31, 32 },
                    new[] { 33, 34 }
                }
            };

            var sut = new MultidimensionalArrayRelay();
            var context = new DelegatingSpecimenContext
            {
                OnResolve = r =>
                {
                    Assert.Equal(jagged.GetType(), r);
                    return jagged;
                }
            };

            var actual = sut.Create(typeof(int[,][]), context);

            Assert.Equal(expected, actual);
        }
    }
}