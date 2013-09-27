using System;
using System.Reflection;

namespace Ploeh.VisitReflect
{
    public class MethodInfoElement : IReflectionElement
    {
        public MethodInfo MethodInfo { get; private set; }

        public MethodInfoElement(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;
        }

        public IReflectionVisitor<T> Accept<T>(IReflectionVisitor<T> visitor)
        {
            throw new NotImplementedException();
        }
    }
}