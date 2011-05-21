using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public abstract class IdiomaticAssertion : IIdiomaticAssertion
    {
        #region IIdiomaticAssertion Members

        public virtual void Verify(System.Reflection.Assembly assembly)
        {
        }

        public virtual void Verify(params Type[] types)
        {
        }

        public virtual void Verify(IEnumerable<Type> types)
        {
        }

        public virtual void Verify(Type type)
        {
        }

        public virtual void Verify(params System.Reflection.MemberInfo[] memberInfos)
        {
        }

        public virtual void Verify(IEnumerable<System.Reflection.MemberInfo> memberInfos)
        {
        }

        public virtual void Verify(System.Reflection.MemberInfo memberInfo)
        {
        }

        public virtual void Verify(params System.Reflection.ConstructorInfo[] constructorInfos)
        {
        }

        public virtual void Verify(IEnumerable<System.Reflection.ConstructorInfo> constructorInfos)
        {
        }

        public virtual void Verify(System.Reflection.ConstructorInfo constructorInfo)
        {
        }

        public virtual void Verify(params System.Reflection.MethodInfo[] methodInfos)
        {
        }

        public virtual void Verify(IEnumerable<System.Reflection.MethodInfo> methodInfos)
        {
        }

        public virtual void Verify(System.Reflection.MethodInfo methodInfo)
        {
        }

        public virtual void Verify(params System.Reflection.PropertyInfo[] propertyInfos)
        {
        }

        public virtual void Verify(IEnumerable<System.Reflection.PropertyInfo> propertyInfos)
        {
        }

        public virtual void Verify(System.Reflection.PropertyInfo propertyInfo)
        {
        }

        #endregion
    }
}
