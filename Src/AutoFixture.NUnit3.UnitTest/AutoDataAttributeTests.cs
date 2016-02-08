using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    [TestFixture]
    public class AutoDataAttributeTests : TestBase
    {
        [Test]
        public void BuildFrom_Calls_ParameterValueProvdier_To_Get_Value_For_Each_Parameter()
        {
            //Arrange
            var parameterValueProvider = Any<IParameterValueProvider>();
            var parameterInfos = Any<IList<IParameterInfo>>();
            var methodInfo = Any<IMethodInfo>();
            var testSuite = Any<TestSuite>();

            var autoDataAttribute = new AutoDataStubAttribute(parameterValueProvider);
            Mock.Get(methodInfo).Setup(m => m.GetParameters()).Returns(parameterInfos.ToArray());

            //Act
            //must call ToArray()
            autoDataAttribute.BuildFrom(methodInfo, testSuite).ToArray();

            //Assert
            foreach (var pi in parameterInfos)
            {
                var piLocal = pi;
                Mock.Get(parameterValueProvider).Verify(p => p.Get(piLocal));
            }
        }
    }

    /// <summary>
    /// With this stub we expose the protected constructor for dependency injection
    /// </summary>
    public class AutoDataStubAttribute : AutoDataAttribute
    {
        public AutoDataStubAttribute(IParameterValueProvider parameterValueProvider) : base(parameterValueProvider)
        {
        }
    }
}
