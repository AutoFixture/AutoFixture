using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class ThrowingRecursionHandler : IRecursionHandler
    {
        public object HandleRecursiveRequest(
            object request,
            IEnumerable<object> recordedRequests)
        {
            throw new ObjectCreationException(string.Format(
                CultureInfo.InvariantCulture,
                "AutoFixture was unable to create an instance of type {0} because the traversed object graph contains a circular reference. Information about the circular path follows below. This is the correct behavior when a Fixture is equipped with a ThrowingRecursionBehavior, which is the default. This ensures that you are being made aware of circular references in your code. Your first reaction should be to redesign your API in order to get rid of all circular references. However, if this is not possible (most likely because parts or all of the API is delivered by a third party), you can replace this default behavior with a different behavior: on the Fixture instance, remove the ThrowingRecursionBehavior from Fixture.Behaviors, and instead add an instance of OmitOnRecursionBehavior.{2}\tPath:{2}{1}",
                recordedRequests.Cast<object>().First().GetType(),
                GetFlattenedRequests(request, recordedRequests),
                Environment.NewLine));
        }

        private static string GetFlattenedRequests(
            object request,
            IEnumerable<object> recordedRequests)
        {
            var requestInfos = new StringBuilder();
            foreach (object r in recordedRequests)
            {
                Type type = r.GetType();
                if (type.Assembly != typeof(RecursionGuard).Assembly)
                {
                    requestInfos.Append("\t\t");
                    requestInfos.Append(r);
                    requestInfos.AppendLine(" --> ");
                }
            }

            requestInfos.Append("\t\t");
            requestInfos.AppendLine(request.ToString());

            return requestInfos.ToString();
        }
    }
}
