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
            throw new NotImplementedException();
        }

        public void Verify(params Type[] types)
        {
            throw new NotImplementedException();
        }

        public void Verify(IEnumerable<Type> types)
        {
            throw new NotImplementedException();
        }

        public void Verify(Type type)
        {
            throw new NotImplementedException();
        }

        public void Verify(params System.Reflection.MemberInfo[] memberInfos)
        {
            throw new NotImplementedException();
        }

        public void Verify(IEnumerable<System.Reflection.MemberInfo> memberInfos)
        {
            throw new NotImplementedException();
        }

        public void Verify(System.Reflection.MemberInfo memberInfo)
        {
            throw new NotImplementedException();
        }

        public void Verify(params System.Reflection.ConstructorInfo[] constructorInfos)
        {
            throw new NotImplementedException();
        }

        public void Verify(IEnumerable<System.Reflection.ConstructorInfo> constructorInfos)
        {
            throw new NotImplementedException();
        }

        public void Verify(System.Reflection.ConstructorInfo constructorInfo)
        {
            throw new NotImplementedException();
        }

        public void Verify(params System.Reflection.MethodInfo[] methodInfos)
        {
            throw new NotImplementedException();
        }

        public void Verify(IEnumerable<System.Reflection.MethodInfo> methodInfos)
        {
            throw new NotImplementedException();
        }

        public void Verify(System.Reflection.MethodInfo methodInfo)
        {
            throw new NotImplementedException();
        }

        public void Verify(params System.Reflection.PropertyInfo[] propertyInfos)
        {
            throw new NotImplementedException();
        }

        public void Verify(IEnumerable<System.Reflection.PropertyInfo> propertyInfos)
        {
            throw new NotImplementedException();
        }

        public void Verify(System.Reflection.PropertyInfo propertyInfo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
