using System;
using System.Linq;
using System.Reflection;

namespace Ploeh.VisitReflect
{
    /// <summary>
    /// An <see cref="IReflectionElement"/> representing a <see cref="ConstructorInfo"/> which
    /// can be visited by an <see cref="IReflectionVisitor{T}"/> implementation.
    /// </summary>
    public class ConstructorInfoElement : IReflectionElement, IHierarchicalReflectionElement
    {
        public ConstructorInfo ConstructorInfo { get; private set; }

        public ConstructorInfoElement(ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null) throw new ArgumentNullException("constructorInfo");
            ConstructorInfo = constructorInfo;
        }

        public IReflectionVisitor<T> Accept<T>(IReflectionVisitor<T> visitor)
        {
            if (visitor == null) throw new ArgumentNullException("visitor");
            return visitor.Visit(this);
        }

        public IHierarchicalReflectionVisitor<T> Accept<T>(IHierarchicalReflectionVisitor<T> visitor)
        {
            if (visitor == null) throw new ArgumentNullException("visitor");
            var visitThis = visitor.EnterConstructor(this);
            visitThis = this.ConstructorInfo.GetParameters()
                .Aggregate(visitThis, (current, parameterInfo) =>
                    new ParameterInfoElement(parameterInfo).Accept(current));
            return visitThis.ExitConstructor(this);
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

            if (obj.GetType() != typeof(ConstructorInfoElement))
            {
                return false;
            }

            return this.ConstructorInfo.Equals(((ConstructorInfoElement)obj).ConstructorInfo);
        }

        public override int GetHashCode()
        {
            return this.ConstructorInfo.GetHashCode();
        }

    }
}