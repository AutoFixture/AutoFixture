using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class ParameterNameSpecification : IRequestSpecification
    {
        private readonly string parameterName;

        public ParameterNameSpecification(string parameterName)
        {
            Require.IsNotNull(parameterName, "parameterName");
            this.parameterName = parameterName;
        }

        public string ParameterName
        {
            get { return this.parameterName; }
        }

        public bool IsSatisfiedBy(object request)
        {
            Require.IsNotNull(request, "request");

            return IsRequestForParameter(request) &&
                   ParameterHasSpecifiedName(request);
        }

        private static bool IsRequestForParameter(object request)
        {
            return request is ParameterInfo;
        }

        private bool ParameterHasSpecifiedName(object request)
        {
            return ((ParameterInfo)request).Name == this.parameterName;
        }
    }
}
