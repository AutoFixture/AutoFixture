using System;
using System.Reflection;

namespace Ploeh.VisitReflect
{
    public class FieldInfoElement : IReflectionElement
    {
        public FieldInfo FieldInfo { get; private set; }

        public FieldInfoElement(FieldInfo fieldInfo)
        {
            FieldInfo = fieldInfo;
        }

        public IReflectionVisitor<T> Accept<T>(IReflectionVisitor<T> visitor)
        {
            throw new NotImplementedException();
        }
    }
}