using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ParameterTypeAndNameCriterionTests
    {
        [Fact]
        public void SutIsParameterInfoEquatable()
        {
            var sut = new ParameterTypeAndNameCriterion(
                new DelegatingCriterion<Type>(),
                new DelegatingCriterion<string>());
            Assert.IsAssignableFrom<IEquatable<ParameterInfo>>(sut);
        }
    }
}
