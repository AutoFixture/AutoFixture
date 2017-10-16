using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Idioms
{
    /// <summary>
    /// Composes an arbitrary number of <see cref="IIdiomaticAssertion" /> instances.
    /// </summary>
    public class CompositeIdiomaticAssertion : IIdiomaticAssertion
    {
        private readonly IEnumerable<IIdiomaticAssertion> assertions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeIdiomaticAssertion"/> class with
        /// the supplied <see cref="IIdiomaticAssertion" /> instances.
        /// </summary>
        /// <param name="assertions">The encapsulated assertions.</param>
        public CompositeIdiomaticAssertion(params IIdiomaticAssertion[] assertions)
        {
            this.assertions = assertions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeIdiomaticAssertion"/> class with
        /// the supplied <see cref="IIdiomaticAssertion" /> instances.
        /// </summary>
        /// <param name="assertions">The encapsulated assertions.</param>
        public CompositeIdiomaticAssertion(IEnumerable<IIdiomaticAssertion> assertions)
            : this(assertions.ToArray())
        {
        }

        /// <summary>
        /// Gets the assertions supplied via the constructor.
        /// </summary>
        public IEnumerable<IIdiomaticAssertion> Assertions
        {
            get { return this.assertions; }
        }

        /// <summary>
        /// Verifies the behavior of the constructor by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="constructorInfo">The constructor whose behavior must be verified.</param>
        public void Verify(ConstructorInfo constructorInfo)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(constructorInfo);
            }
        }

        /// <summary>
        /// Verifies the behavior of the constructors by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="constructorInfos">The constructors whose behavior must be verified.</param>
        public void Verify(IEnumerable<ConstructorInfo> constructorInfos)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(constructorInfos);
            }
        }

        /// <summary>
        /// Verifies the behaviour of the assemblies by delegating the implementation to
        /// all <see cref="Assertions"/>.
        /// </summary>
        /// <param name="assemblies">The assemblies whose behaviour must be verified.</param>
        public void Verify(params Assembly[] assemblies)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(assemblies);
            }
        }

        /// <summary>
        /// Verifies the behaviour of the assemblies by delegating the implementation to
        /// all <see cref="Assertions"/>.
        /// </summary>
        /// <param name="assemblies">The assemblies whose behaviour must be verified.</param>
        public void Verify(IEnumerable<Assembly> assemblies)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(assemblies);
            }
        }

        /// <summary>
        /// Verifies the behaviour of the assembly by delegating the implementation to
        /// all <see cref="Assertions"/>.
        /// </summary>
        /// <param name="assembly">The assembly whose behaviour must be verified.</param>
        public void Verify(Assembly assembly)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(assembly);
            }
        }
        
        /// <summary>
        /// Verifies the behaviour of the types by delegating the implementation to
        /// all <see cref="Assertions"/>.
        /// </summary>
        /// <param name="types">The types whose behaviour must be verified.</param>
        public void Verify(params Type[] types)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(types);
            }
        }

        /// <summary>
        /// Verifies the behaviour of the types by delegating the implementation to
        /// all <see cref="Assertions"/>.
        /// </summary>
        /// <param name="types">The types whose behaviour must be verified.</param>
        public void Verify(IEnumerable<Type> types)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(types);
            }
        }

        /// <summary>
        /// Verifies the behaviour of the type by delegating the implementation to
        /// all <see cref="Assertions"/>.
        /// </summary>
        /// <param name="type">The type whose behaviour must be verified.</param>
        public void Verify(Type type)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(type);
            }
        }

        /// <summary>
        /// Verifies the behaviour of the members by delegating the implementation to
        /// all <see cref="Assertions"/>.
        /// </summary>
        /// <param name="memberInfos">The members whose behaviour must be verified.</param>
        public void Verify(params MemberInfo[] memberInfos)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(memberInfos);
            }
        }

        /// <summary>
        /// Verifies the behaviour of the members by delegating the implementation to
        /// all <see cref="Assertions"/>.
        /// </summary>
        /// <param name="memberInfos">The members whose behaviour must be verified.</param>
        public void Verify(IEnumerable<MemberInfo> memberInfos)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(memberInfos);
            }
        }

        /// <summary>
        /// Verifies the behaviour of the member by delegating the implementation to
        /// all <see cref="Assertions"/>.
        /// </summary>
        /// <param name="memberInfo">The member whose behaviour must be verified.</param>
        public void Verify(MemberInfo memberInfo)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(memberInfo);
            }
        }

        /// <summary>
        /// Verifies the behavior of the constructors by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="constructorInfos">The constructors whose behavior must be verified.</param>
        public void Verify(params ConstructorInfo[] constructorInfos)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(constructorInfos);
            }
        }

        /// <summary>
        /// Verifies the behavior of the constructor by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="methodInfo">The method whose behavior must be verified.</param>
        public void Verify(MethodInfo methodInfo)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(methodInfo);
            }
        }

        /// <summary>
        /// Verifies the behavior of the methods by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="methodInfos">The methods whose behavior must be verified.</param>
        public void Verify(IEnumerable<MethodInfo> methodInfos)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(methodInfos);
            }
        }

        /// <summary>
        /// Verifies the behavior of the methods by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="methodInfos">The methods whose behavior must be verified.</param>
        public void Verify(params MethodInfo[] methodInfos)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(methodInfos);
            }
        }

        /// <summary>
        /// Verifies the behavior of the property by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="propertyInfo">The property whose behavior must be verified.</param>
        public void Verify(PropertyInfo propertyInfo)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(propertyInfo);
            }
        }

        /// <summary>
        /// Verifies the behavior of the properties by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="propertyInfos">The properties whose behavior must be verified.</param>
        public void Verify(IEnumerable<PropertyInfo> propertyInfos)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(propertyInfos);
            }
        }

        /// <summary>
        /// Verifies the behavior of the properties by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="propertyInfos">The properties whose behavior must be verified.</param>
        public void Verify(params PropertyInfo[] propertyInfos)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(propertyInfos);
            }
        }

        /// <summary>
        /// Verifies the behavior of the field by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="fieldInfo">The field whose behavior must be verified.</param>
        public void Verify(FieldInfo fieldInfo)
        {
 	        foreach(var assertion in this.assertions)
            {
                assertion.Verify(fieldInfo);
            }
        }

        /// <summary>
        /// Verifies the behavior of the fields by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="fieldInfos">The fields whose behavior must be verified.</param>
        public void Verify(IEnumerable<FieldInfo> fieldInfos)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(fieldInfos);
            }
        }

        /// <summary>
        /// Verifies the behavior of the fields by delegating the implementation to
        /// all <see cref="Assertions" />.
        /// </summary>
        /// <param name="fieldInfos">The fields whose behavior must be verified.</param>
        public void Verify(params FieldInfo[] fieldInfos)
        {
            foreach (var assertion in this.assertions)
            {
                assertion.Verify(fieldInfos);
            }
        }
    }
}
