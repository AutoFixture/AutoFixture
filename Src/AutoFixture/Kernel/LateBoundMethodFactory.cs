using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class LateBoundMethodFactory : IMethodFactory
    {
        private readonly object owner;

        public LateBoundMethodFactory(object owner)
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
            return new LateBoundMethod(new InstanceMethod(methodInfo, Owner));
        }
    }
}