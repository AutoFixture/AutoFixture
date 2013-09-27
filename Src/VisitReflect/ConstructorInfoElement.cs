using System;
using System.Reflection;

namespace Ploeh.VisitReflect
{
    public class ConstructorInfoElement : IReflectionElement
    {
        public ConstructorInfo ConstructorInfo { get; private set; }

        public ConstructorInfoElement(ConstructorInfo constructorInfo)
        {
            ConstructorInfo = constructorInfo;
        }

        public IReflectionVisitor<T> Accept<T>(IReflectionVisitor<T> visitor)
        {
            throw new NotImplementedException();
        }
    }
}