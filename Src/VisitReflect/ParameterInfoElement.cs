using System;
using System.Reflection;

namespace Ploeh.VisitReflect
{
    public class ParameterInfoElement : IReflectionElement
    {
        public ParameterInfo ParameterInfo { get; private set; }

        public ParameterInfoElement(ParameterInfo parameterInfo)
        {
            ParameterInfo = parameterInfo;
        }

        public IReflectionVisitor<T> Accept<T>(IReflectionVisitor<T> visitor)
        {
            throw new NotImplementedException();
        }
    }
}