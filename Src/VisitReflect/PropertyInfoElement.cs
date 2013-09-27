using System;
using System.Reflection;

namespace Ploeh.VisitReflect
{
    public class PropertyInfoElement : IReflectionElement
    {
        public PropertyInfo PropertyInfo { get; private set; }

        public PropertyInfoElement(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        public IReflectionVisitor<T> Accept<T>(IReflectionVisitor<T> visitor)
        {
            throw new NotImplementedException();
        }
    }
}