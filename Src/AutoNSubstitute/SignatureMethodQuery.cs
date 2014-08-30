using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoNSubstitute
{
    internal class SignatureMethodQuery : IMethodQuery
    {
        private readonly MethodInfo signature;

        public SignatureMethodQuery(MethodInfo signature)
        {
            if (signature == null)
                throw new ArgumentNullException("signature");

            this.signature = signature;
        }

        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                where method.Name == signature.Name
                let methodParameters = method.GetParameters()
                let signatureParameters = signature.GetParameters()
                where methodParameters.Length >= signatureParameters.Length
                orderby methodParameters.Length
                where methodParameters.All(p =>
                    p.Position >= signatureParameters.Length ?
                        p.IsOptional || p.IsDefined(typeof(ParamArrayAttribute), true) :
                        Compare(p.ParameterType, signatureParameters[p.Position].ParameterType))
                select new LateBoundMethod(new GenericStaticMethod(method));
        }

        private bool Compare(Type parameterType, Type signatureParameterType)
        {
            if (parameterType == signatureParameterType)
                return true;

            if (parameterType.IsGenericParameter)
                return signatureParameterType.IsGenericParameter && parameterType.GenericParameterPosition == signatureParameterType.GenericParameterPosition;

            if (parameterType.HasElementType && signatureParameterType.HasElementType)
                return Compare(parameterType.GetElementType(), signatureParameterType.GetElementType());

            var genericArguments = parameterType.GetGenericArguments();
            var signatureGenericArguments = signatureParameterType.GetGenericArguments();
            if (genericArguments.Length != signatureGenericArguments.Length)
                return false;

            return genericArguments.Zip(signatureGenericArguments, Compare).All(x => x);
        }
    }
}