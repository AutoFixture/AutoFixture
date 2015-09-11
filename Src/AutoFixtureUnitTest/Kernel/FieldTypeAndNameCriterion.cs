using System;
using System.Reflection;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class FieldTypeAndNameCriterion : IEquatable<FieldInfo>
    {
        public FieldTypeAndNameCriterion(
            IEquatable<Type> typeCriterion, 
            IEquatable<string> nameCriterion)
        {
        }

        public bool Equals(FieldInfo other)
        {
            throw new NotImplementedException();
        }
    }
}