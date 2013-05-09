namespace Ploeh.SemanticComparison.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fluent;

    using TestTypeFoundation;

    using Xunit;

    public abstract class ProxyTestBase
    {
        protected abstract TDestination CreateProxy<TSource, TDestination>(Likeness<TSource, TDestination> likeness);

        [Fact]
        public void ProxyIsNotNull()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            // Exercise system
            var result = CreateProxy(sut);
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ProxyIsCorrectType()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            // Exercise system
            var result = CreateProxy(sut);
            // Verify outcome
            Assert.IsAssignableFrom<AbstractType>(result);
            // Teardown
        }

        [Fact]
        public void ProxyReturnsDifferentInstanceWhenAccessedMultipleTimes()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            var expectedProxies = new[] { CreateProxy(sut), CreateProxy(sut), CreateProxy(sut) };
            // Exercise system
            var result = Enumerable.Range(1, expectedProxies.Length)
                .Select(x => CreateProxy(sut))
                .ToArray();
            // Verify outcome
            Assert.False(expectedProxies.SequenceEqual(result, new ReferenceEqualityComparer()));
            // Teardown
        }

        [Fact]
        public void ProxyInstanceIsDifferentThanSourceInstance()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            // Exercise system
            var result = CreateProxy(sut);
            // Verify outcome
            Assert.NotSame(source, result);
            // Teardown
        }

        [Fact]
        public void ProxyDoesNotEqualNullObject()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            // Exercise system
            var result = CreateProxy(sut);
            // Verify outcome
            Assert.False(result.Equals(null));
            // Teardown
        }

        [Fact]
        public void ProxyDoesNotEqualNullSource()
        {
            // Fixture setup
            var source = new ConcreteType();
            var sut = source.AsSource().OfLikeness<AbstractType>();
            // Exercise system
            var result = CreateProxy(sut);
            // Verify outcome
            Assert.False(result.Equals((ConcreteType)null));
            // Teardown
        }

        [Fact]
        public void ProxyWhenCalledMultipleTimesForSemanticallyEqualObjectsReturnsTrue()
        {
            // Fixture setup
            var source = new ConcreteType("Lorem");

            var likeness = source.AsSource().OfLikeness<ConcreteType>();
            var sut = CreateProxy(likeness);
            var expectedResult = Enumerable.Range(1, 3).Select(x => true);

            // Exercise system
            var result = Enumerable.Range(1, 3).Select(x => sut.Equals(new ConcreteType("Lorem")));
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ProxyWhenCalledMultipleTimesForSemanticallyUnequalObjectsReturnsFalse()
        {
            // Fixture setup
            var source = new ConcreteType("Lorem");

            var likeness = source.AsSource().OfLikeness<ConcreteType>();
            var sut = CreateProxy(likeness);
            var expectedResult = Enumerable.Range(1, 3).Select(x => false);

            // Exercise system
            var result = Enumerable.Range(1, 3).Select(x => sut.Equals(new ConcreteType("ipsum")));
            // Verify outcome
            Assert.True(expectedResult.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ProxyOfAbstractTypeEqualsConcreteInstancesThatDifferOnlyOnMemberNotDefinedByAbstraction()
        {
            // Fixture setup
            var other = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            other.Property5 = "ploeh";

            var likeness = other.AsSource().OfLikeness<AbstractType>();
            var sut = CreateProxy(likeness);

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ProxyGetHashCodeDoesNotEqualRealGetHashCode()
        {
            // Fixture setup
            var source = new TypeOverridingGetHashCode();
            int expected = source.GetHashCode();
            var sut = source.AsSource().OfLikeness<TypeOverridingGetHashCode>();
            // Exercise system
            var result = CreateProxy(sut);
            // Verify outcome
            Assert.NotEqual(expected, result.GetHashCode());
            // Teardown
        }

        [Fact]
        public void ProxyThrowsWhenRealTypeIsSealed()
        {
            // Fixture setup
            var likeness = new ConcreteType().AsSource().OfLikeness<PublicSealedType>();
            // Exercise system and verify outcome
            Assert.Throws<ProxyCreationException>(() => CreateProxy(likeness));
            // Teardown
        }

        [Fact]
        public void ProxyOfTypeWithAccessibleConstructorDoesNotThrowWhenSourceTypeIsAnonymousType()
        {
            // Fixture setup
            var likeness = new {}.AsSource().OfLikeness<AbstractType>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => CreateProxy(likeness));
            // Teardown
        }

        [Fact]
        public void ProxyOfTypeWithNonDefaultConstructorThrowsWhenSourceConstructorValuesDoNotMapToDestinationConstructor()
        {
            // Fixture setup
            var likeness = new {}.AsSource().OfLikeness<AbstractTypeWithNonDefaultConstructor<string>>();
            // Exercise system and verify outcome
            Assert.Throws<ProxyCreationException>(() => CreateProxy(likeness));
            // Teardown
        }

        [Fact]
        public void ProxyOfAbstractTypeWithNonDefaultConstructorDoesNotThrow()
        {
            // Fixture setup
            var value = new PropertyHolder<string>();
            value.Property = "Foo";
            var sut = value.AsSource().OfLikeness<AbstractTypeWithNonDefaultConstructor<string>>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => CreateProxy(sut));
            // Teardown
        }

        [Fact]
        public void ProxyOfQuadrupleParameterTypeEqualsTripleParameterType()
        {
            // Fixture setup
            var original = new QuadrupleParameterType<int, double, long, string>(1, 2.0, 3, "4");
            var other = new TripleParameterType<int, double, long>(1, 2.0, 3);

            var likeness = original.AsSource().OfLikeness<TripleParameterType<int, double, long>>();
            var sut = CreateProxy(likeness);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
        }

        [Fact]
        public void ProxyOfQuadrupleParameterTypeEqualsDoubleParameterType()
        {
            // Fixture setup
            var original = new QuadrupleParameterType<int, double, long, string>(1, 2.0, 3, "4");
            var other = new DoubleParameterType<int, double>(1, 2.0);

            var likeness = original.AsSource().OfLikeness<DoubleParameterType<int, double>>();
            var sut = CreateProxy(likeness);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
        }

        [Fact]
        public void ProxyOfQuadrupleParameterTypeEqualsSingleParameterType()
        {
            // Fixture setup
            var original = new QuadrupleParameterType<int, double, long, string>(1, 2.0, 3, "4");
            var other = new SingleParameterType<int>(1);

            var likeness = original
                .AsSource().OfLikeness<SingleParameterType<int>>()
                .With(d => d.Parameter).EqualsWhen((s, d) => s.Parameter1 == d.Parameter);
            var sut = CreateProxy(likeness);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
        }

        [Fact]
        public void ProxyEqualsIsTrueWhenCorrectMappingHasBeenDefined()
        {
            // Fixture setup
            var original = new PropertyHolder<string> { Property = "Foo" };
            var other = new FieldHolder<string> { Field = "Foo" };

            var likeness = new Likeness<PropertyHolder<string>, FieldHolder<string>>(original)
                .With(d => d.Field).EqualsWhen((s, d) => s.Property == d.Field);
            var sut = CreateProxy(likeness);

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ProxyWithoutPropertyEqualsInstanceEvenIfItDiffersOnThatProperty()
        {
            // Fixture setup
            var other = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            other.Property5 = "amet";

            var likeness = other.AsSource().OfLikeness<ConcreteType>().Without(x => x.Property5);
            var sut = CreateProxy(likeness);
            
            sut.Property5 = "Fnaah";
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ProxyWithoutTwoPropertiesEqualsInstanceEvenThoughThosePropertyValuesDiffer()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property5 = "amet";

            var likeness = value
                .AsSource().OfLikeness<ConcreteType>()
                .Without(x => x.Property5)
                .Without(x => x.Property1);
            var sut = CreateProxy(likeness);
            sut.Property1 = "Ndøh";
            sut.Property5 = "Sqryt";
            // Exercise system
            var result = sut.Equals(value);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ProxyWithSemanticEqualityWillReturnTrue()
        {
            // Fixture setup
            var original = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            var other = new DoublePropertyHolder<object, object>
                {
                Property1 = "Lorem",
                Property2 = "ipsum"
            };

            var likeness = original.AsSource().OfLikeness<DoublePropertyHolder<object, object>>();
            var sut = CreateProxy(likeness);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void ProxyOfIdenticalParameterTypesCanBeCreated()
        {
            // Fixture setup
            var sut = new TripleParameterType<long, long, long>(1, 2, 3)
                .AsSource().OfLikeness<TripleParameterType<long, long, long>>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => CreateProxy(sut));
            // Teardown
        }

        [Fact]
        public void ProxyOfIdenticalParameterTypesWithPropertiesCanBeCreated()
        {
            // Fixture setup
            var sut = new TypeWithIdenticalParameterTypesAndProperties(1, 2, 3)
                .AsSource().OfLikeness<TypeWithIdenticalParameterTypesAndProperties>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => CreateProxy(sut));
            // Teardown
        }

        [Fact]
        public void ProxyOfTypeWithDifferentParameterTypesAndPropertiesCanBeCreated()
        {
            // Fixture setup
            var sut = new TypeWithDifferentParameterTypesAndProperties(1, "2", 3)
                .AsSource().OfLikeness<TypeWithDifferentParameterTypesAndProperties>();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => CreateProxy(sut));
            // Teardown
        }

        [Fact]
        public void ProxyOfTypeWithIncompatibleAndCompatibleConstructorCanBeCreated()
        {
            // Fixture setup
            var value = new TypeWithIncompatibleAndCompatibleConstructor(
               new ConcreteType());

            var sut = value
                .AsSource()
                .OfLikeness<TypeWithIncompatibleAndCompatibleConstructor>();
            // Exercise system
            var result = CreateProxy(sut);
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ProxyOfTypeWithIncompatibleAndCompatibleConstructorCanBeCreatedSecondOverload()
        {
            // Fixture setup
            var value = new TypeWithIncompatibleAndCompatibleConstructor(
                new ConcreteType(),
                new byte());

            var sut = value
                .AsSource()
                .OfLikeness<TypeWithIncompatibleAndCompatibleConstructor>();
            // Exercise system
            var result = CreateProxy(sut);
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ProxyOfTypeWithIncompatibleAndCompatibleConstructorCanBeCreatedThirdOverload()
        {
            // Fixture setup
            var value = new TypeWithIncompatibleAndCompatibleConstructor(
                new ConcreteType(),
                new ConcreteType(),
                new byte());

            var sut = value
                .AsSource()
                .OfLikeness<TypeWithIncompatibleAndCompatibleConstructor>();
            // Exercise system
            var result = CreateProxy(sut);
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ProxyOfTypeWithUnorderedPropertiesCanBeCreated()
        {
            // Fixture setup
            var value = new TypeWithUnorderedProperties(
                new ConcreteType(),
                new ConcreteType(),
                new byte());

            var sut = value
                .AsSource()
                .OfLikeness<TypeWithUnorderedProperties>();
            // Exercise system
            var result = CreateProxy(sut);
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ProxyAndLikenessHaveSameBehavior1()
        {
            // Fixture setup
            var original = new ConcreteType
            {
                Property1 = "value1",
                Property2 = "value2"
            };
            var other = new ConcreteType
            {
                Property1 = "abc",
                Property2 = "value2"
            };

            var likeness = original.AsSource().OfLikeness<ConcreteType>()
                .Without(x => x.Property1);

            var expected = likeness.Equals(other);

            var sut = CreateProxy(likeness);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(result);
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void ProxyAndLikenessHaveSameBehavior2()
        {
            // Fixture setup
            var original = new
            {
                Property1 = "value1",
                Property2 = new[] { "A", "B", "C" },
                Property3 = (object)null,
                Property4 = (object)null,
                Property5 = (object)null
            };

            var other = new ConcreteType
            {
                Property1 = "value1",
                Property2 = "A,B,C",
                Property3 = null,
                Property4 = null,
                Property5 = null
            };

            var likeness = original
                .AsSource().OfLikeness<ConcreteType>()
                .With(x => x.Property2).EqualsWhen((a, b) => String.Join(",", a.Property2) == (string)b.Property2);

            var sut = CreateProxy(likeness);

            var expected = likeness.Equals(other);

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(expected);
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void ProxyAndLikenessHaveSameBehavior3()
        {
            // Fixture setup
            var original = new ConcreteType
            {
                Property1 = "value1",
                Property2 = "abc",
            };

            var other = new ConcreteType
            {
                Property1 = "value1",
                Property2 = "cba"
            };

            var likeness = original
                .AsSource().OfLikeness<ConcreteType>()
                .With(x => x.Property2).EqualsWhen((a, b) =>
                {
                    return ((string)b.Property2).Reverse().SequenceEqual((IEnumerable<char>)a.Property2);
                });

            var sut = CreateProxy(likeness);

            var expected = likeness.Equals(other);

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.True(expected);
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void ProxyAndLikenessHaveSameBehavior4()
        {
            var obj = new
            {
                A = 1,
                B = "hello",
                C = new Dictionary<string, string>
                    {
                        {"A", "D"},
                        {"B", "E"},
                        {"C", "F"},
                    }
            };

            var likeness = obj
                .AsSource().OfLikeness<Test>()
                .With(x => x.C).EqualsWhen((a, b) => ToQueryString(a.C) == b.C);

            var other = new Test
            {
                A = 1,
                B = "hello",
                C = "A=D&B=E&C=F"
            };

            var result1 = likeness.Equals(other);
            var result2 = CreateProxy(likeness).Equals(other);

            // Verify outcome
            Assert.True(result1);
            Assert.Equal(result1, result2);
            // Teardown
        }

        public class Test
        {
            public int A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
        }

        public string ToQueryString(IDictionary<string, string> source)
        {
            return String.Join("&", source.Select(x => x.Key + "=" + x.Value));
        }

        public sealed class PublicSealedType
        {
        }

        public class TypeOverridingGetHashCode
        {
            public override int GetHashCode()
            {
                return 14;
            }
        }

        public class TypeWithIdenticalParameterTypesAndProperties
        {
            private readonly long field1;
            private readonly long field2;
            private readonly long field3;
            private readonly long field4;

            public TypeWithIdenticalParameterTypesAndProperties(
                long parameter1,
                long parameter2,
                long parameter3)
            {
                this.field1 = parameter1;
                this.field2 = parameter2;
                this.field3 = parameter3;
                this.field4 = 400;
            }

            protected TypeWithIdenticalParameterTypesAndProperties(object source)
            {
                if (source == null)
                {
                    throw new ArgumentNullException("source");
                }
            }

            public long Property1
            {
                get { return this.field1; }
            }

            public long Property2
            {
                get { return this.field2; }
            }

            public long Property3
            {
                get { return this.field3; }
            }

            public long Property4
            {
                get { return this.field4; }
            }
        }

        public class TypeWithDifferentParameterTypesAndProperties
        {
            private readonly double field1;
            private readonly string field2;
            private readonly int field3;
            private readonly Guid field4;

            public TypeWithDifferentParameterTypesAndProperties(
                double field1,
                string field2,
                int field3)
            {
                this.field1 = field1;
                this.field2 = field2;
                this.field3 = field3;
                this.field4 = Guid.NewGuid();
            }

            protected TypeWithDifferentParameterTypesAndProperties(object source)
            {
                if (source == null)
                {
                    throw new ArgumentNullException("source");
                }
            }

            public double Property1
            {
                get { return this.field1; }
            }

            public string Property2
            {
                get { return this.field2; }
            }

            public int Property3
            {
                get { return this.field3; }
            }

            public Guid Property4
            {
                get { return this.field4; }
            }
        }

        public class TypeWithIncompatibleAndCompatibleConstructor
        {
            private readonly AbstractType value1;
            private readonly AbstractType value2;
            private readonly byte value3;

            public TypeWithIncompatibleAndCompatibleConstructor(ConcreteType a)
                : this(new ConcreteType(), new CompositeType(a), new byte())
            {
            }

            public TypeWithIncompatibleAndCompatibleConstructor(ConcreteType a, byte b)
                : this(new ConcreteType(), new CompositeType(a), b)
            {
            }

            public TypeWithIncompatibleAndCompatibleConstructor(ConcreteType a, AbstractType b, byte c)
            {
                this.value1 = a;
                this.value2 = b;
                this.value3 = c;
            }

            public AbstractType Property1
            {
                get { return this.value1; }
            }

            public AbstractType Property2
            {
                get { return this.value2; }
            }

            public byte Property3
            {
                get { return this.value3; }
            }
        }

        public class TypeWithUnorderedProperties
        {
            private readonly AbstractType value1;
            private readonly AbstractType value2;
            private readonly byte value3;

            public TypeWithUnorderedProperties(ConcreteType a, AbstractType b, byte c)
            {
                this.value1 = a;
                this.value2 = b;
                this.value3 = c;
            }

            public byte Property3
            {
                get { return this.value3; }
            }

            public AbstractType Property1
            {
                get { return this.value1; }
            }

            public AbstractType Property2
            {
                get { return this.value2; }
            }
        }

        public class TypeWithPublicFieldsAndProperties
        {
            public string Field;

            private long number;

            public long Number
            {
                get { return this.number; }
                set { this.number = value; }
            }

            public decimal AutomaticProperty { get; set; }
        }

        private class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            bool IEqualityComparer<object>.Equals(object x, object y)
            {
                return ReferenceEquals(x, y);
            }

            int IEqualityComparer<object>.GetHashCode(object obj)
            {
                return obj != null ? obj.GetHashCode() : 0;
            }
        }
    }
}
