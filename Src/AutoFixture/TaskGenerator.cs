using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoFixture.Kernel;

namespace AutoFixture
{
    /// <summary>
    /// Creates instances of <see cref="Task"/> and <see cref="Task{TResult}"/>.
    /// The status will be set to <see cref="TaskStatus.RanToCompletion"/> and <see cref="Task{TResult}.Result"/> will be resolved by a given <see cref="ISpecimenContext"/>.
    /// </summary>
    public class TaskGenerator : ISpecimenBuilder
    {
        /// <summary>
        /// Creates instances of <see cref="Task"/> and <see cref="Task{TResult}"/>.
        /// The status will be set to <see cref="TaskStatus.RanToCompletion"/> and <see cref="Task{TResult}.Result"/> will be resolved by the <paramref name="context"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">The context that will be used to resolve the result value for generic tasks.</param>
        /// <returns>
        /// A task whose status is set to <see cref="TaskStatus.RanToCompletion"/>.
        /// If a generic task was requested, its result was resolved from the <paramref name="context"/>.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var type = request as Type;
            if (type == null)
                return new NoSpecimen();

            // check if type is a constructed generic type whose definition matches Task<>.
            if (type.TryGetSingleGenericTypeArgument(typeof(Task<>), out Type taskResultType))
                return CreateGenericTask(taskResultType, context);

            // check if type is non-generic Task.
            if (type == typeof(Task))
                return CreateNonGenericTask();

            return new NoSpecimen();
        }

        private static object CreateGenericTask(Type resultType, ISpecimenContext context)
        {
            var result = context.Resolve(resultType);
            return CreateTask(resultType, result);
        }

        private static object CreateNonGenericTask()
        {
            return CreateTask(typeof(object), null);
        }

        private static object CreateTask(Type resultType, object result)
        {
            var task = typeof(Task).GetTypeInfo()
                .GetMethod(nameof(Task.FromResult))
                .MakeGenericMethod(resultType)
                .Invoke(null, new[] { result });

            return task;
        }
    }
}
