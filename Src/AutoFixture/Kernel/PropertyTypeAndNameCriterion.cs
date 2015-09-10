using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class PropertyTypeAndNameCriterion : IEquatable<PropertyInfo>
    {
        public PropertyTypeAndNameCriterion(
            IEquatable<Type> typeCriterion,
            IEquatable<string> nameCriterion)
        {
        }

        public bool Equals(PropertyInfo other)
        {
            throw new NotImplementedException();
        }
    }
}