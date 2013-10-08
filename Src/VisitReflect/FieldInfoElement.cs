using System;
using System.Reflection;

namespace Ploeh.VisitReflect
{
    /// <summary>
    /// An <see cref="IReflectionElement"/> representing a <see cref="FieldInfo"/> which
    /// can be visited by an <see cref="IReflectionVisitor{T}"/> implementation.
    /// </summary>
    public class FieldInfoElement : IReflectionElement, IHierarchicalReflectionElement
    {
        public FieldInfo FieldInfo { get; private set; }

        public FieldInfoElement(FieldInfo fieldInfo)
        {
            if (fieldInfo == null) throw new ArgumentNullException("fieldInfo");
            FieldInfo = fieldInfo;
        }

        public IHierarchicalReflectionVisitor<T> Accept<T>(IHierarchicalReflectionVisitor<T> visitor)
        {
            if (visitor == null) throw new ArgumentNullException("visitor");
            return visitor.Visit(this);
        }

        public IReflectionVisitor<T> Accept<T>(IReflectionVisitor<T> visitor)
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

            return this.FieldInfo.Equals(((FieldInfoElement)obj).FieldInfo);
        }

        public override int GetHashCode()
        {
            return this.FieldInfo.GetHashCode();
        }

    }
}