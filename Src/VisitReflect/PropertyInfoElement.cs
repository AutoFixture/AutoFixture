using System;
using System.Reflection;

namespace Ploeh.VisitReflect
{
    /// <summary>
    /// An <see cref="IReflectionElement"/> representing a <see cref="PropertyInfo"/> which
    /// can be visited by an <see cref="IReflectionVisitor{T}"/> implementation.
    /// </summary>
    public class PropertyInfoElement : IReflectionElement, IHierarchicalReflectionElement
    {
        public PropertyInfo PropertyInfo { get; private set; }

        public PropertyInfoElement(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException("propertyInfo");
            PropertyInfo = propertyInfo;
        }

        public IReflectionVisitor<T> Accept<T>(IReflectionVisitor<T> visitor)
        {
            if (visitor == null) throw new ArgumentNullException("visitor");
            return visitor.Visit(this);
        }

        public IHierarchicalReflectionVisitor<T> Accept<T>(IHierarchicalReflectionVisitor<T> visitor)
        {
            if (visitor == null) throw new ArgumentNullException("visitor");
            return visitor.Visit(this);
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj))
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.PropertyInfo.Equals(((PropertyInfoElement)obj).PropertyInfo);
        }

        public override int GetHashCode()
        {
            return this.PropertyInfo.GetHashCode();
        }

    }
}