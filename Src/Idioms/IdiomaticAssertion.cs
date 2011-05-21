using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public abstract class IdiomaticAssertion : IIdiomaticAssertion
    {
        #region IIdiomaticAssertion Members

        public void Verify(System.Reflection.Assembly assembly)
        {
        }

        public void Verify(params Type[] types)
        {
        }

        public void Verify(IEnumerable<Type> types)
        {
        }

        public void Verify(Type type)
        {
        }

        public void Verify(params System.Reflection.MemberInfo[] memberInfos)
        {
        }

        public void Verify(IEnumerable<System.Reflection.MemberInfo> memberInfos)
        {
        }

        public void Verify(System.Reflection.MemberInfo memberInfo)
        {
        }

        public void Verify(params System.Reflection.ConstructorInfo[] constructorInfos)
        {
        }

        public void Verify(IEnumerable<System.Reflection.ConstructorInfo> constructorInfos)
        {
        }

        public void Verify(System.Reflection.ConstructorInfo constructorInfo)
        {
        }

        public void Verify(params System.Reflection.MethodInfo[] methodInfos)
        {
        }

        public void Verify(IEnumerable<System.Reflection.MethodInfo> methodInfos)
        {
        }

        public void Verify(System.Reflection.MethodInfo methodInfo)
        {
        }

        public void Verify(params System.Reflection.PropertyInfo[] propertyInfos)
        {
        }

        public void Verify(IEnumerable<System.Reflection.PropertyInfo> propertyInfos)
        {
        }

        public void Verify(System.Reflection.PropertyInfo propertyInfo)
        {
        }

        #endregion
    }
}
