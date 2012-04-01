using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Handles recursion in the specimen creation process by throwing an exception at recursion point.
    /// </summary>
    public class ThrowingRecursionGuard : RecursionGuard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowingRecursionGuard"/> class.
        /// </summary>
        /// <param name="builder">The intercepting builder to decorate.</param>
        /// <param name="comparer">An IEqualitycomparer implementation to use when comparing requests to determine recursion.</param>
        public ThrowingRecursionGuard(ISpecimenBuilder builder, IEqualityComparer comparer)
            : base(builder, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowingRecursionGuard"/> class.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public ThrowingRecursionGuard(ISpecimenBuilder builder)
            : base(builder)
        {
        }

        /// <summary>
        /// Handles a request that would cause recursion by throwing an exception.
        /// </summary>
        /// <param name="request">The recursion causing request.</param>
        /// <returns>Nothing. Always throws.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AutoFixture", Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
        public override object HandleRecursiveRequest(object request)
        {
            throw new ObjectCreationException(string.Format(
                CultureInfo.InvariantCulture,
                "AutoFixture was unable to create an instance of type {0} because the traversed object graph contains a circular reference. Path: {1}.",
                this.RecordedRequests.Cast<object>().First().GetType(),
                this.GetFlattenedRequests(request)));
        }

        public override ISpecimenBuilder Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var builder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new ThrowingRecursionGuard(builder, this.Comparer);
        }

        private string GetFlattenedRequests(object finalRequest)
        {
            var requestInfos = new StringBuilder();
            foreach (object request in this.RecordedRequests)
            {
                Type type = request.GetType();
                if (type.Assembly != typeof(RecursionGuard).Assembly)
                {
                    requestInfos.Append(request + " --> ");
                }
            }

            return requestInfos.ToString() + finalRequest;
        }
    }
}