using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

#if NETSTANDARD2_1_OR_GREATER

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Satisfies a request for <see cref="IAsyncEnumerable{T}"/> by wrapping a <see cref="IEnumerable{T}"/> with an implementation of <see cref="IAsyncEnumerable{T}"/>.
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
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request is Type { IsGenericType: true } type && type.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>))
            {
                var typeArgument = type.GenericTypeArguments[0];

                var arrayType = typeArgument.MakeArrayType();

                var items = context.Resolve(arrayType);

                return Activator.CreateInstance(typeof(SynchronousAsyncEnumerable<>).MakeGenericType(typeArgument), args: new[] { items });
            }

            return new NoSpecimen();
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "It's activated via reflection.")]
        private class SynchronousAsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            private readonly IEnumerable<T> enumerable;

            public SynchronousAsyncEnumerable(IEnumerable<T> enumerable) => this.enumerable = enumerable;

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new SynchronousAsyncEnumerator<T>(this.enumerable.GetEnumerator());
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "It's activated via reflection.")]
        private class SynchronousAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> enumerator;

            public T Current => this.enumerator.Current;

            public SynchronousAsyncEnumerator(IEnumerator<T> enumerator) => this.enumerator = enumerator;

            public ValueTask DisposeAsync() => new ValueTask(Task.CompletedTask);

            public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(Task.FromResult(this.enumerator.MoveNext()));
        }
    }
}

#endif