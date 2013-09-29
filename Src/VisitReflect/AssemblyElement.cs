using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ploeh.VisitReflect
{
    public class AssemblyElement : IReflectionElement
    {
        public Assembly Assembly { get; private set; }

        public AssemblyElement(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            this.Assembly = assembly;
        }

        public IReflectionVisitor<T> Accept<T>(IReflectionVisitor<T> visitor)
        {
            if (visitor == null) throw new ArgumentNullException("visitor");
            var visitThis = visitor.EnterAssembly(this);
            visitThis = this.Assembly.GetTypes()
                .Aggregate(visitThis, (current, type) =>
                    new TypeElement(type).Accept(current));
            return visitThis.ExitAssembly(this);
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

            return this.Assembly.Equals(((AssemblyElement)obj).Assembly);
        }

        public override int GetHashCode()
        {
            return this.Assembly.GetHashCode();
        }

    }
}
