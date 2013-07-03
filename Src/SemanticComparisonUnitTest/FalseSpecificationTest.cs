using System.Reflection;
using Xunit;

namespace Ploeh.SemanticComparison.UnitTest
{
    public abstract class FalseSpecificationTest<T>
    {
        [Fact]
        public void SutIsSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new FalseSpecification<T>();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecification<T>>(sut);
            // Teardown
        }
    }

    public class FalseSpecificationTestOfFieldInfo : FalseSpecificationTest<FieldInfo> { }
    public class FalseSpecificationTestOfMemberInfo : FalseSpecificationTest<MemberInfo> { }
    public class FalseSpecificationTestOfPropertyInfo : FalseSpecificationTest<PropertyInfo> { }
}
