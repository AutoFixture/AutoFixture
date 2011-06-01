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
            foreach (var m in memberInfos)
            {
                this.Verify(m);
            }
        }

        public virtual void Verify(IEnumerable<MemberInfo> memberInfos)
        {
            this.Verify(memberInfos.ToArray());
        }

        public virtual void Verify(MemberInfo memberInfo)
        {
            var c = memberInfo as ConstructorInfo;
            if (c != null)
            {
                this.Verify(c);
                return;
            }

            var m = memberInfo as MethodInfo;
            if (m != null)
            {
                this.Verify(m);
                return;
            }

            var p = memberInfo as PropertyInfo;
            if (p != null)
            {
                this.Verify(p);
            }
        }

        public virtual void Verify(params ConstructorInfo[] constructorInfos)
        {
            foreach (var c in constructorInfos)
            {
                this.Verify(c);
            }
        }

        public virtual void Verify(IEnumerable<ConstructorInfo> constructorInfos)
        {
            this.Verify(constructorInfos.ToArray());
        }

        public virtual void Verify(ConstructorInfo constructorInfo)
        {
        }

        public virtual void Verify(params MethodInfo[] methodInfos)
        {
            foreach (var m in methodInfos)
            {
                this.Verify(m);
            }
        }

        public virtual void Verify(IEnumerable<MethodInfo> methodInfos)
        {
            this.Verify(methodInfos.ToArray());
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
