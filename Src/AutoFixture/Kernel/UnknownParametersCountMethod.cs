using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    ///     A decorator that decorates a method with one parameter of type <see cref="object"/> or an array of that type in such a fashion that it can be called with
    ///     multiple parameters that will all be passed as the single parameter of the decorated method.
    /// </summary>
    public class UnknownParametersCountMethod : IMethod, IEquatable<UnknownParametersCountMethod>
    {
        private IMethod decoratedMethod;
        private List<ParameterInfo> parameterInfos;

        /// <summary>Initializes a new instance of the <see cref="UnknownParametersCountMethod" /> class.</summary>
        /// <param name="decoratedMethod">The decorated method.</param>
        /// <param name="parameters">The parameters.</param>
        public UnknownParametersCountMethod(IMethod decoratedMethod, IEnumerable<ParameterInfo> parameters)
        {
            if (decoratedMethod == null)
            {
                throw new ArgumentNullException("decoratedMethod");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (decoratedMethod.Parameters.Count() != 1)
            {
                throw new ArgumentException("Only methods with exactly one parameter can be decorated by this class.", "decoratedMethod");
            }

            var parameterType = decoratedMethod.Parameters.First().ParameterType;
            if (parameterType != typeof(object) && !typeof(IEnumerable<object>).IsAssignableFrom(parameterType))
            {
                throw new ArgumentException(
                    "Only methods with exactly one parameter of type 'object' or type 'IEnumerable<object>' or a type implementing this interface can be decorated by this class.",
                    "decoratedMethod");
            }

            this.decoratedMethod = decoratedMethod;
            this.parameterInfos = parameters.ToList();
        }

        /// <summary>Gets information about the parameters of the method.</summary>
        /// <value>The parameters.</value>
        public IEnumerable<ParameterInfo> Parameters
        {
            get { return parameterInfos; }
        }

        /// <summary>Invokes the method with the supplied parameters.</summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The result of the method call.</returns>
        public object Invoke(IEnumerable<object> parameters)
        {
            return decoratedMethod.Invoke(new[] { parameters });
        }

        /// <summary>Determines whether the specified <see cref="System.Object" /> is equal to this instance.</summary>
        /// <param name="other">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(UnknownParametersCountMethod other)
        {
            if (other == null)
            {
                return false;
            }

            return decoratedMethod.Equals(other.decoratedMethod) && Parameters.SequenceEqual(other.Parameters);
        }

        /// <summary>Determines whether the specified <see cref="System.Object" /> is equal to this instance.</summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as UnknownParametersCountMethod);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return decoratedMethod.GetHashCode() ^ this.Parameters.Aggregate(0, (current, parameter) => current + parameter.GetHashCode());
        }
    }
}