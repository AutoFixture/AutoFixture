using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class TypedNode : ISpecimenBuilderNode
    {
        private readonly Type targetType;
        private readonly ISpecimenBuilder factory;

        public TypedNode(Type targetType, ISpecimenBuilder factory)
        {
            if (targetType == null)
                throw new ArgumentNullException("targetType");
            if (factory == null)
                throw new ArgumentNullException("factory");

            this.targetType = targetType;
            this.factory = factory;
        }

        public virtual ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            throw new NotImplementedException();
        }

        public object Create(object request, ISpecimenContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ISpecimenBuilder> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Type TargetType
        {
            get { return this.targetType; }
        }

        public ISpecimenBuilder Factory
        {
            get { return this.factory; }
        }
    }
}
