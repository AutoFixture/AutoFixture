using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.AutoRhinoMock;
using Xunit;
using Xunit.Extensions;
using Ploeh.TestTypeFoundation;

namespace AutoRhinoMockUnitTest
{
    public class RhinoMockTypeTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Guid))]
        [InlineData(typeof(DateTimeOffset))]
        public void IsMockableReturnsFalseForNonTypeRequest(object request)
        {
            Assert.False(RhinoMockType.IsMockable(request));
        }

        [Fact]
        public void IsMockableReturnsFalseForNull()
        {
            Assert.False(RhinoMockType.IsMockable(null));
        }

        [Fact]
        public void IsMockableReturnsFalseForConcreteType()
        {
            Assert.False(RhinoMockType.IsMockable(typeof(ConcreteType)));
        }

        [Fact]
        public void IsMockableReturnsTrueForAbstractType()
        {
            Assert.True(RhinoMockType.IsMockable(typeof(AbstractType)));
        }

        [Fact]
        public void IsMockableReturnsTrueForInterface()
        {
            Assert.True(RhinoMockType.IsMockable(typeof(IInterface)));
        }

        [Fact]
        public void IsGenericReturnsFalseForNull()
        {
            Assert.False(RhinoMockType.IsGeneric(null));
        }

        [Fact]
        public void IsGenericReturnsFalseForConcreteType()
        {
            Assert.False(RhinoMockType.IsGeneric(typeof(ConcreteType)));
        }

        [Fact]
        public void IsGenericReturnsFalseForInterface()
        {
            Assert.False(RhinoMockType.IsGeneric(typeof(IInterface)));
        }

        [Fact]
        public void IsGenericReturnsTrueForGenericType()
        {
            Assert.True(RhinoMockType.IsGeneric(typeof(AbstractTypeWithNonDefaultConstructor<string>)));
        }

        [Fact]
        public void IsGenericReturnsTrueForGenericTypeParameter()
        {
            var genericTypeParameter =
                typeof(AbstractTypeWithNonDefaultConstructor<AbstractTypeWithNonDefaultConstructor<string>>).GetGenericArguments().
                    Single();
            Assert.True(RhinoMockType.IsGeneric(genericTypeParameter));
        }
    }
}
