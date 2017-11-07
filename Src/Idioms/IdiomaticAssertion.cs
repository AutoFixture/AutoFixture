using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Base implementation of <see cref="IIdiomaticAssertion" />.
    /// </summary>
    /// <remarks>
    /// <para>
    /// IdiomaticAssertion provides default implementations of all methods of
    /// <see cref="IIdiomaticAssertion" />, making sure that higher-order methods call into
    /// lower-order methods; e.g. that <see cref="Verify(Assembly)" /> calls
    /// <see cref="Verify(Type[])" /> with all public types in the assembly.
    /// </para>
    /// <para>
    /// Implementers can override the appropriate methods instead of creating an implementation of
    /// IIdiomaticAssertion completely from scratch.
    /// </para>
    /// </remarks>
    public abstract class IdiomaticAssertion : IIdiomaticAssertion
    {
        /// <summary>
        /// Calls <see cref="Verify(Assembly)" /> for each Assembly in
        /// <paramref name="assemblies" />.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public virtual void Verify(params Assembly[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            foreach (var a in assemblies)
            {
                this.Verify(a);
            }
        }

        /// <summary>
        /// Calls <see cref="Verify(Assembly)" /> for each Assembly in
        /// <paramref name="assemblies" />.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public virtual void Verify(IEnumerable<Assembly> assemblies)
        {
            this.Verify(assemblies.ToArray());
        }

        /// <summary>
        /// Calls <see cref="Verify(Type[])" /> for each public Type in
        /// <paramref name="assembly" />.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public virtual void Verify(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            this.Verify(assembly.GetExportedTypes());
        }

        /// <summary>
        /// Calls <see cref="Verify(Type)" /> for each Type in <paramref name="types" />.
        /// </summary>
        /// <param name="types">The types.</param>
        public virtual void Verify(params Type[] types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            foreach (var t in types)
            {
                this.Verify(t);
            }
        }

        /// <summary>
        /// Calls <see cref="Verify(Type)" /> for each Type in <paramref name="types" />.
        /// </summary>
        /// <param name="types">The types.</param>
        public virtual void Verify(IEnumerable<Type> types)
        {
            this.Verify(types.ToArray());
        }

        /// <summary>
        /// Calls <see cref="Verify(ConstructorInfo[])" />, <see cref="Verify(MethodInfo[])" /> and
        /// <see cref="Verify(PropertyInfo[])" /> for each constructor, method and property in
        /// <paramref name="type" />.
        /// </summary>
        /// <param name="type">The type.</param>
        public virtual void Verify(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            this.Verify(type.GetConstructors());
            this.Verify(GetMethodsForAssertion(type));
            this.Verify(type.GetProperties());
            this.Verify(type.GetFields());
        }

        /// <summary>
        /// Calls <see cref="Verify(MemberInfo)" /> for each member in
        /// <paramref name="memberInfos" />.
        /// </summary>
        /// <param name="memberInfos">The members.</param>
        public virtual void Verify(params MemberInfo[] memberInfos)
        {
            if (memberInfos == null) throw new ArgumentNullException(nameof(memberInfos));

            foreach (var m in memberInfos)
            {
                this.Verify(m);
            }
        }

        /// <summary>
        /// Calls <see cref="Verify(MemberInfo)" /> for each member in
        /// <paramref name="memberInfos" />.
        /// </summary>
        /// <param name="memberInfos">The members.</param>
        public virtual void Verify(IEnumerable<MemberInfo> memberInfos)
        {
            this.Verify(memberInfos.ToArray());
        }

        /// <summary>
        /// Calls <see cref="Verify(ConstructorInfo)" />, <see cref="MethodInfo" />,
        /// <see cref="PropertyInfo" />, or <see cref="FieldInfo"/>, depending on the subtype of
        /// <paramref name="memberInfo" />.
        /// </summary>
        /// <param name="memberInfo">The member.</param>
        public virtual void Verify(MemberInfo memberInfo)
        {
            switch (memberInfo)
            {
                case ConstructorInfo c:
                    this.Verify(c);
                    break;

                case MethodInfo m:
                    this.Verify(m);
                    break;

                case PropertyInfo p:
                    this.Verify(p);
                    break;

                case FieldInfo f:
                    this.Verify(f);
                    break;
            }
        }

        /// <summary>
        /// Calls <see cref="Verify(ConstructorInfo)" /> for each ConstructorInfo in
        /// <paramref name="constructorInfos" />.
        /// </summary>
        /// <param name="constructorInfos">The constructors.</param>
        public virtual void Verify(params ConstructorInfo[] constructorInfos)
        {
            if (constructorInfos == null) throw new ArgumentNullException(nameof(constructorInfos));

            foreach (var c in constructorInfos)
            {
                this.Verify(c);
            }
        }

        /// <summary>
        /// Calls <see cref="Verify(ConstructorInfo)" /> for each ConstructorInfo in
        /// <paramref name="constructorInfos" />.
        /// </summary>
        /// <param name="constructorInfos">The constructors.</param>
        public virtual void Verify(IEnumerable<ConstructorInfo> constructorInfos)
        {
            this.Verify(constructorInfos.ToArray());
        }

        /// <summary>
        /// Does nothing. Override to implement.
        /// </summary>
        /// <param name="fieldInfo">The field.</param>
        public virtual void Verify(FieldInfo fieldInfo)
        {
        }

        /// <summary>
        /// Calls <see cref="Verify(FieldInfo)" /> for each FieldInfo in
        /// <paramref name="fieldInfos" />.
        /// </summary>
        /// <param name="fieldInfos">The fields.</param>
        public virtual void Verify(params FieldInfo[] fieldInfos)
        {
            if (fieldInfos == null) throw new ArgumentNullException(nameof(fieldInfos));

            foreach (var f in fieldInfos)
            {
                this.Verify(f);
            }
        }

        /// <summary>
        /// Calls <see cref="Verify(FieldInfo)" /> for each FieldInfo in
        /// <paramref name="fieldInfos" />.
        /// </summary>
        /// <param name="fieldInfos">The Fields.</param>
        public virtual void Verify(IEnumerable<FieldInfo> fieldInfos)
        {
            this.Verify(fieldInfos.ToArray());
        }

        /// <summary>
        /// Does nothing. Override to implement.
        /// </summary>
        /// <param name="constructorInfo">The constructor.</param>
        public virtual void Verify(ConstructorInfo constructorInfo)
        {
        }

        /// <summary>
        /// Calls <see cref="Verify(MethodInfo)" /> for each MethodInfo in
        /// <paramref name="methodInfos" />.
        /// </summary>
        /// <param name="methodInfos">The methods.</param>
        public virtual void Verify(params MethodInfo[] methodInfos)
        {
            if (methodInfos == null) throw new ArgumentNullException(nameof(methodInfos));

            foreach (var m in methodInfos)
            {
                this.Verify(m);
            }
        }

        /// <summary>
        /// Calls <see cref="Verify(MethodInfo)" /> for each MethodInfo in
        /// <paramref name="methodInfos" />.
        /// </summary>
        /// <param name="methodInfos">The methods.</param>
        public virtual void Verify(IEnumerable<MethodInfo> methodInfos)
        {
            this.Verify(methodInfos.ToArray());
        }

        /// <summary>
        /// Does nothing. Override to implement.
        /// </summary>
        /// <param name="methodInfo">The method.</param>
        public virtual void Verify(MethodInfo methodInfo)
        {
        }

        /// <summary>
        /// Calls <see cref="Verify(PropertyInfo)" /> for each PropertyInfo in
        /// <paramref name="propertyInfos" />.
        /// </summary>
        /// <param name="propertyInfos">The properties.</param>
        public virtual void Verify(params PropertyInfo[] propertyInfos)
        {
            if (propertyInfos == null) throw new ArgumentNullException(nameof(propertyInfos));

            foreach (var p in propertyInfos)
            {
                this.Verify(p);
            }
        }

        /// <summary>
        /// Calls <see cref="Verify(PropertyInfo)" /> for each PropertyInfo in
        /// <paramref name="propertyInfos" />.
        /// </summary>
        /// <param name="propertyInfos">The properties.</param>
        public virtual void Verify(IEnumerable<PropertyInfo> propertyInfos)
        {
            this.Verify(propertyInfos.ToArray());
        }

        /// <summary>
        /// Does nothing. Override to implement.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        public virtual void Verify(PropertyInfo propertyInfo)
        {
        }

        private static IEnumerable<MethodInfo> GetMethodsForAssertion(Type type)
        {
            return IsStaticClass(type) 
                ? GetMethodsExceptPropertyAccessors(type).Where(m => m.IsStatic) 
                : GetMethodsExceptPropertyAccessors(type);
        }

        private static bool IsStaticClass(Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        private static IEnumerable<MethodInfo> GetMethodsExceptPropertyAccessors(Type type)
        {
            return type.GetMethods().Except(type.GetProperties().SelectMany(p => p.GetAccessors()));
        }
    }
}
