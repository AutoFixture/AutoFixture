using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class PropertyTypeAndNameCriterionTests
    {
        [Fact]
        public void SutIsPropertyInfoEquatable()
        {
            var sut = new PropertyTypeAndNameCriterion(
                new DelegatingCriterion<Type>(),
                new DelegatingCriterion<string>());
            Assert.IsAssignableFrom<IEquatable<PropertyInfo>>(sut);
        }
    }
}
