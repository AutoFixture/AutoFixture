namespace Ploeh.AutoFixture
{
    using System.Collections;
    using Kernel;

    public class InterceptingBuilder : ISpecimenBuilder
    {
        private Hashtable interceptions;
        private ISpecimenBuilder builder;

        public InterceptingBuilder(ISpecimenBuilder builderToDecorate)
        {
            interceptions = new Hashtable();
            builder = builderToDecorate;
        }

        public void SetInterception(object requestToIntercept, object intercept)
        {
            if (interceptions.Contains(requestToIntercept))
            {
                interceptions[requestToIntercept] = intercept;
            }
            else
            {
                interceptions.Add(requestToIntercept, intercept);
            }
        }

        public object Create(object request, ISpecimenContainer container)
        {
            if (interceptions.Contains(request))
            {
                object intercept = interceptions[request];
                interceptions.Remove(request);
                return intercept;
            }

            return builder.Create(request, container);
        }
    }
}