namespace Ploeh.SemanticComparison.UnitTest
{
    using System;

    using Fluent;

    using TestTypeFoundation;

    using Xunit;

    public class MutableProxyTest : ProxyTestBase
    {
        protected override TDestination CreateProxy<TSource, TDestination>(Likeness<TSource, TDestination> likeness)
        {
            return likeness.ToMutableProxy();
        }

        [Fact(Skip = "Unsupported with mutable proxy")]
        public new void ProxyAndLikenessHaveSameBehavior2(){}

        [Fact(Skip = "Unsupported with mutable proxy")]
        public new void ProxyAndLikenessHaveSameBehavior4(){}

        [Fact]
        public void MutableProxyOmitAutoComparisonFollowedByWithDefaultEqualityWillOptInOnThosePropertiesOnlyAndReturnTrueWhenTheyMatch()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");

            var sut = value.AsSource().OfLikeness<ConcreteType>()
                           .OmitAutoComparison()
                           .WithDefaultEquality(d => d.Property1)
                           .WithDefaultEquality(d => d.Property3)
                           .ToMutableProxy();
            sut.Property1 = "Lorem";
            sut.Property2 = "ploeh";
            sut.Property3 = "dolor";
            sut.Property4 = "fnaah";
            // Exercise system
            bool result = sut.Equals(value);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void MutableProxyOmitAutoComparisonFollowedByWithDefaultEqualityWillOptInOnThosePropertiesOnlyAndReturnFalseWhenTheyDoNotMatch()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");

            var sut = value.AsSource().OfLikeness<AbstractType>()
                           .OmitAutoComparison()
                           .WithDefaultEquality(d => d.Property1)
                           .WithDefaultEquality(d => d.Property4)
                           .ToMutableProxy();
            sut.Property1 = "Lorem";
            sut.Property2 = "ploeh";
            sut.Property3 = "dolor";
            sut.Property4 = "fnaah";
            // Exercise system
            bool result = sut.Equals(value);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void MutableProxyOmitAutoComparisonFollowedByCorrectComboOfDefaultEqualityAndExplicitWithReturnsTrue()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");

            var sut = value.AsSource().OfLikeness<AbstractType>()
                           .OmitAutoComparison()
                           .WithDefaultEquality(d => d.Property1)
                           .With(d => d.Property2).EqualsWhen((s, d) => s.Property2.ToString().ToUpper() == d.Property2.ToString())
                           .ToMutableProxy();
            sut.Property1 = "Lorem";
            sut.Property2 = "IPSUM";
            sut.Property3 = "dolor";
            sut.Property4 = "fnaah";
            // Exercise system
            bool result = sut.Equals(value);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void MutableProxyCanCorrectlyAssignsFieldValues()
        {
            // Fixture setup
            var expected = 3;
            var value = new FieldHolder<int>();
            value.Field = expected;

            var sut = value.AsSource().OfLikeness<FieldHolder<int>>()
                           .ToMutableProxy();
            // Exercise system
            var result = sut.Field;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void MutableProxyCanCorrectlyAssignsFieldValuesToTypeWithPublicFieldsAndProperties()
        {
            // Fixture setup
            var value = new TypeWithPublicFieldsAndProperties();
            value.AutomaticProperty = 1m;
            value.Field = "2";
            value.Number = 3;

            var sut = value.AsSource().OfLikeness<TypeWithPublicFieldsAndProperties>();

            // Exercise system
            var proxy = sut.ToMutableProxy();
            var result =
                value.AutomaticProperty == proxy.AutomaticProperty
                && value.Field == proxy.Field
                && value.Number == proxy.Number;
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void MutableProxyCanCorrectlyAssignFieldValuesFromAnonymousType()
        {
            // Fixture setup
            var expected = 3.0;
            var value = new { Field = expected };
            var sut = value.AsSource().OfLikeness<FieldHolder<double>>()
                           .ToMutableProxy();
            // Exercise system
            var result = sut.Field;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void MutableProxyCanCorrectlyAssignFieldValuesFromAnonymousTypeToTypeWithPublicFieldsAndProperties()
        {
            // Fixture setup
            var value = new
            {
                AutomaticProperty = 1m,
                Field = "2",
                Number = Int64.MaxValue
            };
            var sut = value.AsSource()
                           .OfLikeness<TypeWithPublicFieldsAndProperties>()
                           .ToMutableProxy();
            // Exercise system
            var result =
                value.AutomaticProperty == sut.AutomaticProperty
                && value.Field == sut.Field
                && value.Number == sut.Number;
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void MutableProxyOfAbstractTypeDoesNotEqualConcreteInstanceWhenPropertyDiffers()
        {
            // Fixture setup
            var other = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            other.Property4 = "ploeh";

            var sut = other.AsSource().OfLikeness<AbstractType>().ToMutableProxy();
            sut.Property4 = "Fnaah";

            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }
    }
}