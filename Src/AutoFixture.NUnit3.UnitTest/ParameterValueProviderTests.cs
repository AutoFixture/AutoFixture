using System;
using System.Globalization;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class ParameterValueProviderTests : TestBase
    {
        private static readonly object[] TypedParameters = new object[]
        {
            (DateTime)DateTime.Now,
            (int)1,
            (CultureInfo)CultureInfo.CurrentCulture
        };

        [TestCaseSource("TypedParameters")]
        public void When_Get_Will_call_TypedValueProvider_to_create_value_for_return<T>(T expected)
        {
            //Arrange
            var typedValueProvider = Mock.Of<ITypedValueProvider>();
            var parameterValueProvider = new ParameterValueProvider(typedValueProvider);

            var parameterInfo = Any<IParameterInfo>();
            var parameterType = typeof(T);
            Mock.Get(parameterInfo).Setup(p => p.ParameterType).Returns(parameterType);
            
            Mock.Get(typedValueProvider).Setup(f => f.CreateValue(typeof(T))).Returns(expected);

            //Act
            var result = parameterValueProvider.Get(parameterInfo);

            //Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCaseSource("TypedParameters")]
        public void When_Get_Will_call_TypedValueProvider_to_create_frozen_value_if_paramter_has_frozen_attribute<T>(T expected)
        {
            //Arrange
            var typedValueProvider = Mock.Of<ITypedValueProvider>();
            var parameterValueProvider = new ParameterValueProvider(typedValueProvider);

            var parameterInfo = Any<IParameterInfo>();
            var parameterType = typeof(T);
            Mock.Get(parameterInfo).Setup(p => p.ParameterType).Returns(parameterType);

            Mock.Get(parameterInfo).Setup(p => p.GetCustomAttributes<FrozenAttribute>(true)).Returns(new [] { new FrozenAttribute() });
            Mock.Get(typedValueProvider).Setup(f => f.CreateFrozenValue(typeof(T))).Returns(expected);

            //Act
            var result = parameterValueProvider.Get(parameterInfo);

            //Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
