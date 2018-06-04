using System;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates new <see cref="DomainName"/> instances.
    /// </summary>
    public class DomainNameGenerator : ISpecimenBuilder
    {
        private readonly ElementsBuilder<string> fictitiousDomainBuilder = new ElementsBuilder<string>(
                "example.com",
                "example.net",
                "example.org");

        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (request == null || !typeof(DomainName).Equals(request))
                return new NoSpecimen();

            var domainName = this.fictitiousDomainBuilder.Create(typeof(string), context) as string;
            if (domainName == null)
                return new NoSpecimen();

            return new DomainName(domainName);
        }
    }
}