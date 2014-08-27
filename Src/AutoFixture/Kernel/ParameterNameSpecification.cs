using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class ParameterNameSpecification : IRequestSpecification
    {
        private readonly string parameterName;
        private ParameterInfo requestedParameter;

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
                   ParameterHasSpecifiedName();
        }

        private bool IsRequestForParameter(object request)
        {
            this.requestedParameter = request as ParameterInfo;
            return this.requestedParameter != null;
        }

        private bool ParameterHasSpecifiedName()
        {
            return this.requestedParameter.Name == this.parameterName;
        }
    }
}
