using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates a sequence of printable ASCII characters (Dec 33-126), starting at '!' (Dec 33).
    /// </summary>
    public class CharSequenceGenerator : ISpecimenBuilder
    {
        private char c = '!';
        private readonly object syncRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharSequenceGenerator"/> class.
        /// </summary>
        public CharSequenceGenerator()
        {
            this.syncRoot = new object();
        }

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(char).Equals(request))
            {
                return new NoSpecimen();
            }

            return this.Create();
        }

        private char Create()
        {
            lock (this.syncRoot)
            {
                if (this.c > 126)
                {
                    this.c = '!';
                }

                return this.c++;
            }
        }
    }
}