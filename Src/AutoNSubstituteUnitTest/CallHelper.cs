using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest
{
    public static class CallHelper
    {
        public static ICall CreateCallMock(Expression<Action> method, object[] originalArgs = null)
        {
            var methodExpression = (MethodCallExpression) method.Body;
            var args = new List<object>();
            foreach (var argExpression in methodExpression.Arguments)
            {
                var value = Expression.Lambda(argExpression).Compile().DynamicInvoke();
                args.Add(value);
            }

            var argsArray = args.ToArray();
            var methodInfo = methodExpression.Method;

            var call = Substitute.For<ICall>();
            call.GetMethodInfo().Returns(methodInfo);
            call.GetReturnType().Returns(methodInfo.ReturnType);
            call.GetArguments().Returns(argsArray);
            call.GetOriginalArguments().Returns(originalArgs ?? argsArray);

            return call;
        }

        public static ICall CreatePropertyCallMock<T>(Expression<Func<T>> getProp)
        {
            var propertyInfo = (PropertyInfo) ((MemberExpression) getProp.Body).Member;

            var call = Substitute.For<ICall>();
            call.GetMethodInfo().Returns(propertyInfo.GetMethod);
            call.GetReturnType().Returns(propertyInfo.PropertyType);
            call.GetArguments().Returns(new object[0]);
            call.GetOriginalArguments().Returns(new object[0]);

            return call;
        }
    }
}