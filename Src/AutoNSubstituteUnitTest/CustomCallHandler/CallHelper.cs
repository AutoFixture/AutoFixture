using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;

namespace AutoFixture.AutoNSubstitute.UnitTest.CustomCallHandler
{
    public static class CallHelper
    {
        public static ICall CreateCallMock(Expression<Action> method)
        {
            var methodExpression = (MethodCallExpression) method.Body;
            var args = new List<object>();
            foreach (var argExpression in methodExpression.Arguments)
            {
                var value = Expression.Lambda(argExpression).Compile().DynamicInvoke();
                args.Add(value);
            }

            var methodInfo = methodExpression.Method;

            var call = Substitute.For<ICall>();
            call.GetMethodInfo().Returns(methodInfo);
            call.GetReturnType().Returns(methodInfo.ReturnType);
            call.GetArguments().Returns(args.ToArray());
            call.GetOriginalArguments().Returns(args.ToArray());
            call.Target().Returns(ResolveCallTarget(methodExpression.Object));

            return call;
        }

        public static ICall CreatePropertyGetterCallMock<T>(Expression<Func<T>> getProp)
        {
            var propertyExpression = (MemberExpression)getProp.Body;
            var propertyInfo = (PropertyInfo)propertyExpression.Member;

            var call = Substitute.For<ICall>();
            call.GetMethodInfo().Returns(propertyInfo.GetMethod);
            call.GetReturnType().Returns(propertyInfo.PropertyType);
            call.GetArguments().Returns(new object[0]);
            call.GetOriginalArguments().Returns(new object[0]);
            call.Target().Returns(ResolveCallTarget(propertyExpression.Expression));
            return call;
        }

        private static object ResolveCallTarget(Expression targetExpression)
        {
            return ((ConstantExpression)((MemberExpression)targetExpression).Expression).Value;
        }
    }
}