using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class FieldTypeAndNameCriterionTests
    {
        [Fact]
        public void SutIsFieldInfoEquatable()
        {
            var sut = new FieldTypeAndNameCriterion(
                new DelegatingCriterion<Type>(),
                new DelegatingCriterion<string>());
            Assert.IsAssignableFrom<IEquatable<FieldInfo>>(sut);
        }
    }
}
