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

        public int? NumberOfTimesToCall { get; } = null;

        public bool IsApplicableTo(IFakeObjectCall fakeObjectCall)
        {
            return fakeObjectCall != null && IsProperty(fakeObjectCall.Method);
        }

        public void Apply(IInterceptedFakeObjectCall fakeObjectCall)
        {
            if (fakeObjectCall == null) throw new ArgumentNullException(nameof(fakeObjectCall));

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

        private static bool IsSetter(IFakeObjectCall fakeObjectCall) =>
            fakeObjectCall.Method.ReturnType == typeof(void);

        private class PropertyCall
        {
            private readonly string methodName;
            private readonly IList<Type> argumentTypes;
            private readonly IList<object> arguments;

            public PropertyCall(IFakeObjectCall fakeCall)
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
                    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.methodName);
                    foreach (var argument in this.arguments)
                    {
                        hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(argument);
                    }
                    foreach (var argumentType in this.argumentTypes)
                    {
                        hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(argumentType);
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

            public void ApplyToCall(IInterceptedFakeObjectCall fakeObjectCall)
            {
                fakeObjectCall.SetReturnValue(this.returnValue);
            }
        }
    }
}