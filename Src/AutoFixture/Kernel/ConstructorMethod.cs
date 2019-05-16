using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates a constructor as a method.
    /// </summary>
    public class ConstructorMethod : IMethod, IEquatable<ConstructorMethod>
    {
        private readonly ParameterInfo[] paramInfos;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorMethod"/> class.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        public ConstructorMethod(ConstructorInfo constructor)
        {
            this.Constructor = constructor ?? throw new ArgumentNullException(nameof(constructor));
            this.paramInfos = this.Constructor.GetParameters();
        }

        /// <summary>
        /// Gets the constructor.
        /// </summary>
        public ConstructorInfo Constructor { get; }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="object"/> is equal to this instance;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is ConstructorMethod other)
            {
                return this.Equals(other);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data
        /// structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Constructor.GetHashCode();
        }

        /// <summary>
        /// Gets information about the parameters of the method.
        /// </summary>
        public IEnumerable<ParameterInfo> Parameters => this.paramInfos;

        /// <summary>
        /// Invokes the method with the supplied parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The result of the method call.</returns>
        public object Invoke(IEnumerable<object> parameters)
        {
            if (this.Constructor.DeclaringType.GetTypeInfo().IsAbstract && this.Constructor.IsPublic)
            {
                throw new ObjectCreationException(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    @"AutoFixture was unable to create an instance of {0}, since it's an abstract class with a public constructor. Please consider changing the definition of {0}; while technically possible, an abstract class with a public constructor represents a design error. For more information please refer to: http://tinyurl.com/lg38t3g. If you are unable to modify the {0} class, you can customize AutoFixture by mapping the class to a concrete type. As an example, imagine that AbstractClassWithPublicConstructor is an abstract class with a public constructor, and that you can't change the definition of that class. In order to work around that issue, you can add a test-specific concrete class that derives from AbstractClassWithPublicConstructor:

class TestDouble : AbstractClassWithPublicConstructor

You can map AbstractClassWithPublicConstructor to TestDouble:

fixture.Customizations.Add(
    new TypeRelay(
        typeof(AbstractClassWithPublicConstructor),
        typeof(TestDouble)));

This will cause AutoFixture to create an instance of TestDouble every time AbstractClassWithPublicConstructor is requested. However, please keep in mind that this is only a workaround for the case where you can't address the root cause, which is that an abstract class has a public constructor.",
                                    this.Constructor.DeclaringType.Name));
            }
            return this.Constructor.Invoke(parameters.ToArray());
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to the <paramref name="other"/>
        /// parameter; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(ConstructorMethod other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Constructor.Equals(other.Constructor);
        }
    }
}
