using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Selects a method by its name and parameters count.
    /// Takes into account all instance methods, also private ones.
    /// </summary>
    public class InstanceMethodByParametersCountQuery : IMethodQuery
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceMethodByParametersCountQuery"/> class.
        /// </summary>
        /// <param name="owner">The instance that should be selected from.</param>
        /// <param name="methodName">The name of the method to select.</param>
        /// <param name="parametersCount">The number of parameters of the method to select.</param>
        public InstanceMethodByParametersCountQuery(object owner, string methodName, int parametersCount)
        {
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this.MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
            this.ParametersCount = parametersCount;
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
        /// Gets the number of parameters that the method should accept.
        /// </summary>
        public int ParametersCount { get; }

        /// <summary>
        /// Selects <see cref="MethodName"/> with <see cref="ParametersCount"/> from <see cref="Owner"/>.
        /// </summary>
        /// <param name="type">Discarded.</param>
        /// <returns>Returns an empty enumerable if <see cref="MethodName"/> does not belong to <see cref="Owner"/>;
        /// returns an enumerable containing a single <see cref="InstanceMethod"/> otherwise.</returns>
        public IEnumerable<IMethod> SelectMethods(Type type = default)
        {
            var typeInfo = this.Owner.GetType().GetTypeInfo();
            var allMethods = typeInfo.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var method = allMethods
                .Where(m => m.Name == this.MethodName && m.GetParameters().Length == this.ParametersCount)
                .FirstOrDefault()
                ?? allMethods
                    .Where(m => m.Name.EndsWith($".{this.MethodName}", StringComparison.InvariantCultureIgnoreCase)
                        && m.GetParameters().Length == this.ParametersCount)
                    .FirstOrDefault();

            return method == null
                ? new IMethod[0]
                : new IMethod[] { new InstanceMethod(method, this.Owner) };
        }
    }
}