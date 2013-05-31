﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates a constructor as a method.
    /// </summary>
    public class ConstructorMethod : IMethod, IEquatable<ConstructorMethod>
    {
        private readonly ConstructorInfo constructor;
        private readonly ParameterInfo[] paramInfos;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorMethod"/> class.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        public ConstructorMethod(ConstructorInfo constructor)
        {
            if (constructor == null)
            {
                throw new ArgumentNullException("constructor");
            }

            this.constructor = constructor;
            this.paramInfos = this.constructor.GetParameters();
        }

        /// <summary>
        /// Gets the constructor.
        /// </summary>
        public ConstructorInfo Constructor
        {
            get { return this.constructor; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="object"/> is equal to this instance;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as ConstructorMethod;
            if (other != null)
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
        public IEnumerable<ParameterInfo> Parameters
        {
            get { return this.paramInfos; }
        }

        /// <summary>
        /// Invokes the method with the supplied parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The result of the method call.</returns>
        public object Invoke(IEnumerable<object> parameters)
        {
            if (this.Constructor.DeclaringType.IsAbstract && this.constructor.IsPublic)
                throw new ObjectCreationException("abstract classes should never have public constructors");
            return this.constructor.Invoke(parameters.ToArray());
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
