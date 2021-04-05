using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Selects a method from an instance.
    /// </summary>
    public class InstanceMethodQuery : IMethodQuery
    {
        /// <summary>
        /// Constructs an instance of an <see cref="InstanceMethodQuery"/>, used to select a particular method
        /// from an instance.
        /// </summary>
        /// <param name="owner">The instance that should be selected from.</param>
        /// <param name="methodName">The name of the method that should be selected.</param>
        public InstanceMethodQuery(object owner, string methodName)
        {
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this.MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
        }

        /// <summary>
        /// Gets the instance that should be selected from.
        /// </summary>
        public object Owner { get; }

        /// <summary>
        /// Gets the name of the method that should be selected.
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// Selects <see cref="MethodName"/> from <see cref="Owner"/>.
        /// </summary>
        /// <param name="type">Discarded.</param>
        /// <returns>Returns an empty enumerable if <see cref="MethodName"/> does not belong to <see cref="Owner"/>;
        /// returns an enumerable containing a single <see cref="InstanceMethod"/> otherwise.</returns>
        public IEnumerable<IMethod> SelectMethods(Type type = default)
        {
            var method = this.Owner.GetType().GetTypeInfo().GetMethod(this.MethodName);

            return method == null
                ? new IMethod[0]
                : new IMethod[] { new InstanceMethod(method, this.Owner) };
        }
    }
}