using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public abstract class IdiomaticAssertion : IIdiomaticAssertion
    {
        #region IIdiomaticAssertion Members

        public virtual void Verify(Assembly assembly)
        {
            this.Verify(assembly.GetExportedTypes());
        }

        public virtual void Verify(params Type[] types)
        {
            foreach (var t in types)
            {
                this.Verify(t);
            }
        }

        public virtual void Verify(IEnumerable<Type> types)
        {
            this.Verify(types.ToArray());
        }

        public virtual void Verify(Type type)
        {
            this.Verify(type.GetConstructors());
            this.Verify(IdiomaticAssertion.GetMethodsExceptPropertyAccessors(type));
            this.Verify(type.GetProperties());
        }

        public virtual void Verify(params MemberInfo[] memberInfos)
        {
        }

        public virtual void Verify(IEnumerable<MemberInfo> memberInfos)
        {
        }

        public virtual void Verify(MemberInfo memberInfo)
        {
        }

        public virtual void Verify(params ConstructorInfo[] constructorInfos)
        {
        }

        public virtual void Verify(IEnumerable<ConstructorInfo> constructorInfos)
        {
        }

        public virtual void Verify(ConstructorInfo constructorInfo)
        {
        }

        public virtual void Verify(params MethodInfo[] methodInfos)
        {
        }

        public virtual void Verify(IEnumerable<MethodInfo> methodInfos)
        {
        }

        public virtual void Verify(MethodInfo methodInfo)
        {
        }

        public virtual void Verify(params PropertyInfo[] propertyInfos)
        {
        }

        public virtual void Verify(IEnumerable<PropertyInfo> propertyInfos)
        {
        }

        public virtual void Verify(PropertyInfo propertyInfo)
        {
        }

        #endregion

        private static IEnumerable<MethodInfo> GetMethodsExceptPropertyAccessors(Type type)
        {
            return type.GetMethods().Except(type.GetProperties().SelectMany(p => p.GetAccessors()));
        }
    }
}
