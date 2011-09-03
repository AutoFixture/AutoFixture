using System.Collections;
using System.Collections.Generic;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    public class Generator<T> : IEnumerable<T>
    {
        private readonly ISpecimenBuilderComposer composer;

        public Generator(ISpecimenBuilderComposer composer)
        {
            this.composer = composer;
        }

        public IEnumerator<T> GetEnumerator()
        {
            while (true)
                yield return this.composer.CreateAnonymous<T>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
