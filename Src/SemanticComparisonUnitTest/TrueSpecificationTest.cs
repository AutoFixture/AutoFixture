using System.Reflection;
using Xunit;

namespace Ploeh.SemanticComparison.UnitTest
{
    public abstract class TrueSpecificationTest<T>
    {
        [Fact]
        public void SutIsSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new TrueSpecification<T>();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecification<T>>(sut);
            // Teardown
        }

        [Fact]
        public void IsSatisfiedByReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new TrueSpecification<T>();
            // Exercise system
            var dummyRequest = default(T);
            var result = sut.IsSatisfiedBy(dummyRequest);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }
    }

    public class TrueSpecificationTestOfFieldInfo : TrueSpecificationTest<FieldInfo> { }
    public class TrueSpecificationTestOfMemberInfo : TrueSpecificationTest<MemberInfo> { }
    public class TrueSpecificationTestOfPropertyInfo : TrueSpecificationTest<PropertyInfo> { }
}
