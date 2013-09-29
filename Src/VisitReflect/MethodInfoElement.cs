using System;
using System.Linq;
using System.Reflection;

namespace Ploeh.VisitReflect
{
    public class MethodInfoElement : IReflectionElement
    {
        public MethodInfo MethodInfo { get; private set; }

        public MethodInfoElement(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException("methodInfo");
            MethodInfo = methodInfo;
        }

        public IReflectionVisitor<T> Accept<T>(IReflectionVisitor<T> visitor)
        {
            if (visitor == null) throw new ArgumentNullException("visitor");
            var visitThis = visitor.EnterMethod(this);
            visitThis = this.MethodInfo.GetParameters()
                .Aggregate(visitThis, (current, parameterInfo) =>
                    new ParameterInfoElement(parameterInfo).Accept(current));
            return visitThis.ExitMethod(this);
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

            return this.MethodInfo.Equals(((MethodInfoElement)obj).MethodInfo);
        }

        public override int GetHashCode()
        {
            return this.MethodInfo.GetHashCode();
        }

    }
}