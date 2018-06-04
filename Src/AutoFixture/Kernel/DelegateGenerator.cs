using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Creates new <see cref="Delegate"/> instances.
    /// </summary>
    public class DelegateGenerator : ISpecimenBuilder
    {
        /// <summary>
        /// The specification used to verify that request is for the supported delegate type.
        /// </summary>
        public IRequestSpecification Specification { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="DelegateGenerator"/> type.
        /// </summary>
        public DelegateGenerator()
            : this(new DelegateSpecification())
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DelegateGenerator"/> type.
        /// </summary>
        public DelegateGenerator(IRequestSpecification delegateSpecification)
        {
            this.Specification = delegateSpecification ?? throw new ArgumentNullException(nameof(delegateSpecification));
        }

        /// <summary>
        /// Creates a new <see cref="Delegate"/> instance.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
        /// <returns>
        /// A new <see cref="Delegate"/> instance, if <paramref name="request"/> is a request for a
        /// <see cref="Delegate"/>; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var delegateType = request as Type;
            if (delegateType == null)
                return new NoSpecimen();

            if (!this.Specification.IsSatisfiedBy(delegateType))
                return new NoSpecimen();

            var delegateMethod = delegateType.GetTypeInfo().GetMethod("Invoke");
            var methodSpecimenParams = CreateMethodSpecimenParameters(delegateMethod);
            var methodSpecimenBody = CreateMethodSpecimenBody(delegateMethod, context);

            var delegateSpecimen = Expression.Lambda(delegateType, methodSpecimenBody, methodSpecimenParams).Compile();

            return delegateSpecimen;
        }

        private static IEnumerable<ParameterExpression> CreateMethodSpecimenParameters(MethodInfo request)
        {
            var parameters = request.GetParameters()
                .Select((param, i) => Expression.Parameter(param.ParameterType, string.Concat("arg", i)));

            return parameters;
        }

        private static Expression CreateMethodSpecimenBody(MethodInfo request, ISpecimenContext context)
        {
            var returnType = request.ReturnType;
            Expression body;

            if (returnType == typeof(void))
            {
                body = Expression.Empty();
            }
            else
            {
                var returnValue = context.Resolve(returnType);
                body = Expression.Convert(Expression.Constant(returnValue), returnType);
            }

            return body;
        }
    }
}