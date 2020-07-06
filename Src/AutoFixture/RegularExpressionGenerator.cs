using System;
using System.Text.RegularExpressions;
using System.Threading;
using AutoFixture.Kernel;
using Fare;

namespace AutoFixture
{
    /// <summary>
    /// Creates a string that is guaranteed to match a RegularExpressionRequest.
    /// </summary>
    public class RegularExpressionGenerator : ISpecimenBuilder, IDisposable
    {
        private readonly Random random;
        private readonly object syncRoot;
        private readonly ThreadLocal<Random> threadLocalRandom;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegularExpressionGenerator"/> class.
        /// </summary>
        public RegularExpressionGenerator()
        {
            this.random = new Random();
            this.syncRoot = new object();
            this.threadLocalRandom = new ThreadLocal<Random>(() => new Random(this.GenerateSeed()));
        }

        /// <summary>
        /// Creates a string that is guaranteed to match a RegularExpressionRequest.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null) return new NoSpecimen();

            var regularExpressionRequest = request as RegularExpressionRequest;
            if (regularExpressionRequest == null)
            {
                return new NoSpecimen();
            }

            return this.GenerateRegularExpression(regularExpressionRequest);
        }

        private object GenerateRegularExpression(RegularExpressionRequest request)
        {
            string pattern = request.Pattern;

            try
            {
                // Use the Xeger constructor overload that that takes an instance of Random.
                // Otherwise identically strings can be generated, if regex are generated within short time.
                // Use shared random - but one for each thread to avoid threadsafety issues with Random.
                string regex = new Xeger(pattern, this.threadLocalRandom.Value).Generate();
                if (Regex.IsMatch(regex, pattern))
                {
                    return regex;
                }
            }
            catch (InvalidOperationException)
            {
                return new NoSpecimen();
            }
            catch (ArgumentException)
            {
                return new NoSpecimen();
            }

            return new NoSpecimen();
        }

        private int GenerateSeed()
        {
            lock (this.syncRoot)
            {
                return this.random.Next();
            }
        }

        /// <summary>
        /// Disposes  <see cref="ThreadLocal&lt;Random&gt;"/>.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.threadLocalRandom?.Dispose();
                }

                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Disposes  <see cref="ThreadLocal&lt;Random&gt;"/>.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}