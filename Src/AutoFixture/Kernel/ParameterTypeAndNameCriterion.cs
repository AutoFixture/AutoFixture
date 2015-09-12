using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class ParameterTypeAndNameCriterion : IEquatable<ParameterInfo>
    {
        public ParameterTypeAndNameCriterion(
            IEquatable<Type> typeCriterion,
            IEquatable<string> nameCriterion)
        {
        }

        public bool Equals(ParameterInfo other)
        {
            throw new NotImplementedException();
        }
    }
}