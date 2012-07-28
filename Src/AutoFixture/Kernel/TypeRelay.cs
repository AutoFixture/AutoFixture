using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class TypeRelay : ISpecimenBuilder
    {
        private readonly Type from;
        private readonly Type to;

        public TypeRelay(Type from, Type to)
        {
            if (from == null)
                throw new ArgumentNullException("from");
            if (to == null)
                throw new ArgumentNullException("to");

            this.from = from;
            this.to = to;
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            
            var t = request as Type;
            if (t == null || t != this.from)
                return new NoSpecimen(request);

            return context.Resolve(this.to);
        }
    }
}
