using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Satisfies a request for <see cref="IAsyncEnumerable{T}"/> by wrapping a <see cref="IEnumerable{T}"/>
    /// with an implementation of <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    public class AsyncEnumerableRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates an implementation of <see cref="IAsyncEnumerable{T}"/> wrapping a <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A finite sequence of the requested type if possible; otherwise a <see cref="NoSpecimen"/>
        /// instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (request is not Type type) return new NoSpecimen();

            if (!type.TryGetSingleGenericTypeArgument(typeof(IAsyncEnumerable<>), out Type enumerableType))
            {
                return new NoSpecimen();
            }

            var specimen = context.Resolve(new MultipleRequest(enumerableType));
            if (specimen is OmitSpecimen) return specimen;

            if (specimen is not IEnumerable<object> enumerable)
                return new NoSpecimen();

            var typedAdapterType = typeof(SynchronousAsyncEnumerable<>).MakeGenericType(enumerableType);
            return Activator.CreateInstance(typedAdapterType, enumerable);
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
            Justification = "It's activated via reflection.")]
        private class SynchronousAsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            private readonly IEnumerable<T> enumerable;

            public SynchronousAsyncEnumerable(IEnumerable<object> enumerable)
            {
                if (enumerable is null) throw new ArgumentNullException(nameof(enumerable));

                this.enumerable = enumerable.OfType<T>().ToList();
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return new SynchronousAsyncEnumerator<T>(this.enumerable.GetEnumerator());
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses",
            Justification = "It's activated via reflection.")]
        private class SynchronousAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> enumerator;

            public T Current => this.enumerator.Current;

            public SynchronousAsyncEnumerator(IEnumerator<T> enumerator)
            {
                this.enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
            }

            public ValueTask DisposeAsync()
            {
                return new ValueTask(Task.CompletedTask);
            }

            public ValueTask<bool> MoveNextAsync()
            {
                return new ValueTask<bool>(Task.FromResult(this.enumerator.MoveNext()));
            }
        }
    }
}
