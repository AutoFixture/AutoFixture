using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// The exception that is thrown when AutoFixture is unable to create an object.
    /// This exception is supposed to contain the full request path.
    /// </summary>
#if SYSTEM_RUNTIME_SERIALIZATION
    [Serializable]
#endif
    internal class ObjectCreationExceptionWithPath : ObjectCreationException
    {
        public ObjectCreationExceptionWithPath()
        {
        }

        public ObjectCreationExceptionWithPath(string message)
            : base(message)
        {
        }

        public ObjectCreationExceptionWithPath(string message, IEnumerable<object> requests)
            : base(message + FormatRequestsPath(requests))
        {
        }

        public ObjectCreationExceptionWithPath(string message, Exception innerException)
            : base(message + FormatInnerExceptionMessages(innerException), innerException)
        {
        }

        public ObjectCreationExceptionWithPath(string message, IEnumerable<object> requests, Exception innerException)
            : base(message + FormatRequestsPath(requests) + FormatInnerExceptionMessages(innerException),
                innerException)
        {
        }

#if SYSTEM_RUNTIME_SERIALIZATION
        protected ObjectCreationExceptionWithPath(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
#endif

        private static string FormatRequestsPath(IEnumerable<object> requests)
        {
            var prefix = string.Format(CultureInfo.InvariantCulture, "{0}{0}Request path:{0}", Environment.NewLine);

            var paddedRequests = requests
                .Where(ShouldDisplayRequestInPath)
                .Select((req, level) => string.Format(CultureInfo.CurrentCulture, "\t{0}{1}", Indent(level), req));

            return prefix + string.Join(Environment.NewLine, paddedRequests) + Environment.NewLine;
        }

        private static bool ShouldDisplayRequestInPath(object request)
        {
            if (request.GetType().GetTypeInfo().GetCustomAttribute<PreserveInRequestPathAttribute>() != null)
                return true;

            var autoFixtureAssembly = typeof(ObjectCreationExceptionWithPath).GetTypeInfo().Assembly;
            if (request.GetType().GetTypeInfo().Assembly.Equals(autoFixtureAssembly))
                return false;

            return true;
        }

        private static string FormatInnerExceptionMessages(Exception ex)
        {
            var messages = new StringBuilder();
            messages.AppendLine();
            messages.AppendLine("Inner exception messages:");

            var level = 0;
            while (ex != null)
            {
                messages.AppendFormat(
                    CultureInfo.InvariantCulture, "\t{0}{1}: {2}", Indent(level), ex.GetType().FullName, ex.Message);
                messages.AppendLine();

                level++;
                ex = ex.InnerException;
            }

            messages.AppendLine();

            return messages.ToString();
        }

        private static string Indent(int level) => string.Empty.PadLeft(level * 2);
    }
}