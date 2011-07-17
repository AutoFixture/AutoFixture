using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    public interface IIdiomaticAssertion
    {
        void Verify(Assembly assembly);

        void Verify(params Type[] types);

        void Verify(IEnumerable<Type> types);

        void Verify(Type type);

        void Verify(params MemberInfo[] memberInfos);

        void Verify(IEnumerable<MemberInfo> memberInfos);

        void Verify(MemberInfo memberInfo);

        void Verify(params ConstructorInfo[] constructorInfos);

        void Verify(IEnumerable<ConstructorInfo> constructorInfos);

        void Verify(ConstructorInfo constructorInfo);

        void Verify(params MethodInfo[] methodInfos);

        void Verify(IEnumerable<MethodInfo> methodInfos);

        void Verify(MethodInfo methodInfo);

        void Verify(params PropertyInfo[] propertyInfos);

        void Verify(IEnumerable<PropertyInfo> propertyInfos);

        void Verify(PropertyInfo propertyInfo);
    }
}
