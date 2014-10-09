using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class MissingParametersSupplyingMethodFactory : IMethodFactory
    {
        private readonly object owner;

        public MissingParametersSupplyingMethodFactory(object owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            this.owner = owner;
        }

        public object Owner
        {
            get { return owner; }
        }

        public IMethod Create(MethodInfo methodInfo)
        {
            return new MissingParametersSupplyingMethod(new InstanceMethod(methodInfo, Owner));
        }
    }
}