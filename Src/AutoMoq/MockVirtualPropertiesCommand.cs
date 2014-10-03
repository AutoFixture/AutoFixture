using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Moq;
using Ploeh.AutoFixture.AutoMoq.Extensions;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoMoq
{
    /// <summary>
    /// Stubs a mocked object's virtual properties, giving them "property behavior".
    /// This means setting a property's value will cause it to be saved and later returned when the property is accessed.
    /// The initial value will be lazily retrieved from a fixture.
    /// </summary>
    /// <remarks>
    /// This will setup any virtual properties with public get *and* set accessors.
    /// </remarks>
    public class MockVirtualPropertiesCommand : ISpecimenCommand
    {
        /// <summary>
        /// Stubs a mocked object's virtual properties, giving them "property behavior".
        /// This means setting a property's value will cause it to be saved and later returned when the property is accessed.
        /// The initial value will be lazily retrieved from a fixture.
        /// </summary>
        /// <param name="specimen">The mock to setup.</param>
        /// <param name="context">The context of the mock.</param>
        public void Execute(object specimen, ISpecimenContext context)
        {
            if (specimen == null) throw new ArgumentNullException("specimen");
            if (context == null) throw new ArgumentNullException("context");

            var mock = specimen as Mock;
            if (mock == null)
                return;

            var mockType = mock.GetType();
            var mockedType = mockType.GetMockedType();
            var properties = GetConfigurableProperties(mockedType);

            foreach (var property in properties)
            {
                var propertyAccessExpression = MakePropertyAccessExpression(mockedType, property);
                var propertyAssignmentAction = MakePropertyAssignmentAction(mockedType, property);

                this.GetType()
                    .GetMethod("SetupProperty", BindingFlags.NonPublic | BindingFlags.Instance)
                    .MakeGenericMethod(mockedType, property.PropertyType)
                    .Invoke(this, new object[] {mock, propertyAccessExpression, propertyAssignmentAction, context});
            }
        }

        /// <summary>
        /// Stubs a property giving it "property behavior".
        /// Setting its value will cause it to be saved and later returned when the property is accessed.
        /// The initial value for the property will be lazily resolved using <paramref name="context"/>.
        /// </summary>
        /// <typeparam name="TMock">The type of the object being mocked.</typeparam>
        /// <typeparam name="TResult">The type of the property being stubbed</typeparam>
        /// <param name="mock">The mock being setup.</param>
        /// <param name="propertyAccessExpression">An expression representing access to the property to be stubbed.</param>
        /// <param name="propertyAssignmentAction">An action delegate that assigns a value to the property being stubbed.</param>
        /// <param name="context">The context that will be used to lazily resolve the property's initial value.</param>
        protected virtual void SetupProperty<TMock, TResult>(Mock<TMock> mock,
            Expression<Func<TMock, TResult>> propertyAccessExpression, Action<TMock> propertyAssignmentAction,
            ISpecimenContext context) where TMock : class
        {
            var lazy = new Lazy<TResult>(() => (TResult) context.Resolve(typeof (TResult)));

            mock.SetupGet(propertyAccessExpression)
                .Returns(() => lazy.Value);

            mock.SetupSet(propertyAssignmentAction)
                .Callback<TResult>(value => lazy = new Lazy<TResult>(() => value));
        }

        private static IEnumerable<PropertyInfo> GetConfigurableProperties(Type type)
        {
            // If "type" is an interface, "GetProperties" does not return properties declared on other interfaces extended by "type".
            // In these cases, we use the "GetInterfaceProperties" extension method instead.
            var properties = type.IsInterface
                ? type.GetInterfaceProperties()
                : type.GetProperties();

            return properties.Where(CanBeConfigured);
        }

        private static bool CanBeConfigured(PropertyInfo property)
        {
            return property.GetSetMethod() != null &&
                   property.GetGetMethod() != null &&
                   property.GetSetMethod().IsOverridable() &&
                   property.GetIndexParameters().Length == 0;
        }

        /// <summary>
        /// Returns a lambda expression thats represents access to an object's property.
        /// E.g., <![CDATA[ x => x.Property ]]> 
        /// </summary>
        private static Expression MakePropertyAccessExpression(Type mockedType, PropertyInfo property)
        {
            var lambdaParam = Expression.Parameter(mockedType, "x");

            //e.g. "x.Property"
            var propertyAccess = Expression.Property(lambdaParam, property);

            //e.g. "x => x.Property"
            return Expression.Lambda(propertyAccess, lambdaParam);
        }

        /// <summary>
        /// Returns a delegate that assigns a value to an object's property.
        /// E.g., <![CDATA[ x => x.Property = It.IsAny<string>() ]]> 
        /// </summary>
        private static Delegate MakePropertyAssignmentAction(Type mockedType, PropertyInfo property)
        {
            var lambdaParam = Expression.Parameter(mockedType, "x");

            //e.g. "x.Property"
            var propertyAccess = Expression.Property(lambdaParam, property);

            //e.g. "It.IsAny<T>()"
            var isAnyMethod = typeof (It).GetMethod("IsAny")
                .MakeGenericMethod(property.PropertyType);
            var isAnyCall = Expression.Call(isAnyMethod);

            //e.g. "x.Property = It.IsAny<T>()"
            var assignment = Expression.Assign(propertyAccess, isAnyCall);

            //e.g. "x => x.Property = It.IsAny<T>()"
            var delegateType = typeof (Action<>).MakeGenericType(mockedType);
            var lambdaExpression = Expression.Lambda(delegateType, assignment, lambdaParam);

            //compile expression into an Action<TMocked>
            return (Delegate) lambdaExpression.GetType()
                .GetMethod("Compile", new Type[0])
                .Invoke(lambdaExpression, new object[] {});
        }
    }
}
