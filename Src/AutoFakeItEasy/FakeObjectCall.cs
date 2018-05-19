using System;
using System.Collections.Generic;
using System.Reflection;
using FakeItEasy.Core;

namespace AutoFixture.AutoFakeItEasy
{
    using System.Globalization;

    /// <summary>
    /// A bridge class, required because the types representing fake object calls in 
    /// 1.7.4109.1 (which the .NET Framework version of AutoFixture.AutoFakeItEasy is compiled against)
    /// differ from those in 2.0.0+ in ways that prevent us from using them directly.
    /// If ever support for FakeItEasy versions below 2.0.0 is dropped, this class may be removed.
    /// </summary>
    internal class FakeObjectCall
    {
        private const string SetReturnValueMethodName = "SetReturnValue";
        private const string SetArgumentValueMethodName = "SetArgumentValue";
        private const string ArgumentsPropertyName = "Arguments";

        private readonly IFakeObjectCall wrappedCall;

        public FakeObjectCall(IFakeObjectCall wrappedCall)
        {
            this.wrappedCall = wrappedCall;
        }

        public MethodInfo Method => this.wrappedCall.Method;

        public IEnumerable<object> Arguments => (IEnumerable<object>)InvokeWrappedPropertyGetter(ArgumentsPropertyName);

        public void SetReturnValue(object value)
        {
            InvokeWrappedMethod(SetReturnValueMethodName, value);
        }

        public void SetArgumentValue(int index, object value) =>
            InvokeWrappedMethod(SetArgumentValueMethodName, index, value);

        private object InvokeWrappedPropertyGetter(string propertyName)
        {
            var callType = this.wrappedCall.GetType();
            var propertyInfo = callType.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new MissingMethodException(string.Format(CultureInfo.CurrentCulture,
                    "Property {0} cannot be found on {1}", propertyName, callType.FullName));
            }

            return propertyInfo.GetValue(wrappedCall);
        }

        private void InvokeWrappedMethod(string methodName, params object[] parameters)
        {
            var callType = this.wrappedCall.GetType();
            var methodInfo = callType.GetMethod(methodName);
            if (methodInfo == null)
            {
                throw new MissingMethodException(string.Format(CultureInfo.CurrentCulture,
                    "Method {0} cannot be found on {1}", methodName, callType.FullName));
            }

            methodInfo.Invoke(wrappedCall, parameters);
        }
    }
}