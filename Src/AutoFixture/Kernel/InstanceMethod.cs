﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Encapsulates an instance method. This is essentially an Adapter over
    /// <see cref="MethodInfo"/>.
    /// </summary>
    public class InstanceMethod : IMethod, IEquatable<InstanceMethod>
    {
        private readonly MethodInfo method;
        private readonly ParameterInfo[] paramInfos;
        private readonly object owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceMethod"/> class.
        /// </summary>
        /// <param name="instanceMethod">The instance method.</param>
        /// <param name="owner">The owner.</param>
        /// <remarks>
        /// <para>
        /// The owner is expected to expose the method designated by
        /// <paramref name="instanceMethod" />. If not, the <see cref="Invoke" /> method will fail.
        /// </para>
        /// </remarks>
        /// <seealso cref="Invoke" />
        /// <seealso cref="Method" />
        /// <seealso cref="Owner" />
        public InstanceMethod(MethodInfo instanceMethod, object owner)
        {
            if (instanceMethod == null)
            {
                throw new ArgumentNullException(nameof(instanceMethod));
            }
            if (owner == null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            this.method = instanceMethod;
            this.paramInfos = this.method.GetParameters();
            this.owner = owner;
        }

        /// <summary>
        /// Gets the method originally supplied through the constructor.
        /// </summary>
        /// <seealso cref="InstanceMethod(MethodInfo, object)" />
        public MethodInfo Method
        {
            get { return this.method; }
        }

        /// <summary>
        /// Gets the owner originally supplied through the constructor.
        /// </summary>
        /// <seealso cref="InstanceMethod(MethodInfo, object)" />
        public object Owner
        {
            get { return this.owner; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <see cref="System.Object"/> is equal to this
        /// instance; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            var other = obj as InstanceMethod;
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
            return this.Method.GetHashCode() ^ this.Owner.GetHashCode();
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
            return this.method.Invoke(this.owner, parameters.ToArray());
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to the <paramref name="other"/>
        /// parameter; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(InstanceMethod other)
        {
            if (other == null)
            {
                return false;
            }

            return object.Equals(this.Method, other.Method) 
                && object.Equals(this.Owner, other.Owner);
        }
    }
}
