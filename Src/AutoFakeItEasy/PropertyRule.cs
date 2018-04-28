using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture.Kernel;
using FakeItEasy.Core;

namespace AutoFixture.AutoFakeItEasy
{
    /// <summary>
    /// A rule that intercepts property calls. Set values will be saved and
    /// provided as the return value from the matching get methods when the latter are called.
    /// If a get method is called before the corresponding set, the return value will
    /// be resolved from the fixture and similarly saved for later.
    /// </summary>
    internal class PropertyRule : IFakeObjectCallRule
    {
        private readonly ISpecimenContext context;

        private readonly ConcurrentDictionary<PropertyCall, PropertyReturnValue> behaviors =
            new ConcurrentDictionary<PropertyCall, PropertyReturnValue>();

        public PropertyRule(ISpecimenContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the number of times this call rule is valid.
        /// </summary>
        /// <returns><c>null</c>, indicating that the rule has no expiration.</returns>
        public int? NumberOfTimesToCall => null;

        /// <summary>
        /// Gets whether this rule is applicable to the specified
        /// call. If <c>true</c> is returned then <see cref="Apply" /> will be called.
        /// </summary>
        /// <param name="fakeObjectCall">The call to check for applicability.</param>
        /// <returns><c>true</c> if the call is a call to a property.</returns>
        public bool IsApplicableTo(IFakeObjectCall fakeObjectCall)
        {
            return fakeObjectCall != null && IsProperty(fakeObjectCall.Method);
        }

        /// <summary>
        /// Applies an action to the call. If the call is a property setter, stores a value for later
        /// calls to the getter. If a getter, sets the return value to the stored value (if no stored value,
        /// obtains one and stores it first).
        /// </summary>
        /// <param name="interceptedFakeObjectCall">The call to apply the rule to.</param>
        public void Apply(IInterceptedFakeObjectCall interceptedFakeObjectCall)
        {
            if (interceptedFakeObjectCall == null) throw new ArgumentNullException(nameof(interceptedFakeObjectCall));

            var fakeObjectCall = new FakeObjectCall(interceptedFakeObjectCall);
            var propertyCall = new PropertyCall(fakeObjectCall);
            var methodReturnType = fakeObjectCall.Method.ReturnType;
            if (IsSetter(fakeObjectCall))
            {
                this.behaviors[propertyCall] = new PropertyReturnValue(fakeObjectCall.Arguments.Last());
            }
            else
            {
                var returnValueAction = this.behaviors.GetOrAdd(
                    propertyCall,
                    _ => new PropertyReturnValue(this.context.Resolve(methodReturnType)));
                returnValueAction.ApplyToCall(fakeObjectCall);
            }
        }

        private static bool IsProperty(MethodInfo method) =>
            method.IsSpecialName && (method.Name.StartsWith("get_", StringComparison.Ordinal) ||
                                     method.Name.StartsWith("set_", StringComparison.Ordinal));

        private static bool IsSetter(FakeObjectCall fakeObjectCall) =>
            fakeObjectCall.Method.IsSpecialName &&
            fakeObjectCall.Method.Name.StartsWith("set_", StringComparison.Ordinal);

        private class PropertyCall
        {
            private readonly string methodName;
            private readonly IList<Type> argumentTypes;
            private readonly IList<object> arguments;

            public PropertyCall(FakeObjectCall fakeCall)
            {
                this.methodName = fakeCall.Method.Name.Substring(4);
                var numberOfArguments = fakeCall.Arguments.Count();
                if (IsSetter(fakeCall))
                {
                    --numberOfArguments;
                }

                this.arguments = fakeCall.Arguments.Take(numberOfArguments).ToList();
                this.argumentTypes = fakeCall.Method.GetParameters()
                    .Take(numberOfArguments)
                    .Select(p => p.ParameterType)
                    .ToList();
            }

            public override bool Equals(object obj)
            {
                return obj is PropertyCall call &&
                       this.methodName.Equals(call.methodName, StringComparison.Ordinal) &&
                       this.argumentTypes.SequenceEqual(call.argumentTypes) &&
                       this.arguments.SequenceEqual(call.arguments);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = -712421553;
                    hashCode = hashCode * -1521134295 + this.methodName.GetHashCode();
                    foreach (var argument in this.arguments)
                    {
                        hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(argument);
                    }
                    foreach (var argumentType in this.argumentTypes)
                    {
                        hashCode = hashCode * -1521134295 + argumentType.GetHashCode();
                    }
                    return hashCode;
                }
            }
        }

        private class PropertyReturnValue
        {
            private readonly object returnValue;

            public PropertyReturnValue(object returnValue)
            {
                this.returnValue = returnValue;
            }

            public void ApplyToCall(FakeObjectCall fakeObjectCall)
            {
                fakeObjectCall.SetReturnValue(this.returnValue);
            }
        }
    }
}