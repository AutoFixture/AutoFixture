using System;
using System.Linq;
using NUnit.Framework;

namespace Ploeh.AutoFixture.NUnit.UnitTest.TestCases
{
    public class AutoTestCaseAttributeTests
    {
        [Test]
        public void SutIsTestCaseAttribute()
        {
            var sut = new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), "SutIsTestCaseAttribute"); 

            Assert.IsInstanceOf<TestCaseAttribute>(sut);
        }

        [Test]
        public void InitializedWithDefaultConstructorHasCorrectFixture()
        {
            var sut = new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), "InitializedWithDefaultConstructorHasCorrectFixture");
            IFixture result = sut.Fixture;
            Assert.IsInstanceOf<Fixture>(result);
        }

        [Test]
        public void InitializeWithNullFixtureThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new SubclassAutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), "InitializeWithNullFixtureThrows", (IFixture)null));
        }

        [Test]
        public void InitializedWithComposerHasCorrectComposer()
        {
            var expectedComposer = new DelegatingFixture();
            var sut = new SubclassAutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), "InitializedWithComposerHasCorrectComposer", expectedComposer);

            var result = sut.Fixture;

            Assert.That(expectedComposer, Is.EqualTo(result));
        }

        [Test]
        public void InitializeWithNullTypeThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), "InitializeWithNullTypeThrows", (Type)null));
        }

        [Test]
        public void InitializeWithNonComposerTypeThrows()
        {
            Assert.Throws<ArgumentException>(() => new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), "InitializeWithNonComposerTypeThrows", typeof(object)));
        }

        [Test]
        public void InitializeWithComposerTypeWithoutDefaultConstructorThrows()
        {
            Assert.Throws<ArgumentException>(() => new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), "InitializeWithComposerTypeWithoutDefaultConstructorThrows", typeof(ComposerWithoutADefaultConstructor)));
        }

        [Test]
        public void InitializedWithCorrectComposerTypeHasCorrectComposer()
        {
            var composerType = typeof (DelegatingFixture);
            var sut = new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), "InitializedWithCorrectComposerTypeHasCorrectComposer", composerType);

            var result = sut.Fixture;

            Assert.IsInstanceOf(composerType,result);
        }

        [Test]
        public void FixtureTypeIsCorrect()
        {
            var composerType = typeof (DelegatingFixture);
            var sut = new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), "FixtureTypeIsCorrect", composerType);

            var result = sut.FixtureType;

            Assert.That(composerType, Is.EqualTo(result));
        }

        [Test]
        public void GetDataWithInvalidMethodThrows()
        {
            Assert.Throws<ArgumentException>(() => new AutoTestCaseAttribute(typeof(TypeWithMembers),"Foo"));
        }

        [Test]
        public void GetDataReturnsCorrectResult()
        {
            var method = typeof(TypeWithMembers).GetMethod("DoSomething");
            var parameters = method.GetParameters();
            
            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder
            {
                OnCreate = (r, c) =>
                {
                    Assert.That(parameters.Single(), Is.EqualTo(r));
                    Assert.That(c, Is.Not.Null);
                    return expectedResult;
                }
            };
            var composer = new DelegatingFixture { OnCreate = builder.OnCreate };

            var sut = new SubclassAutoTestCaseAttribute(typeof(TypeWithMembers),"DoSomething",composer);
            
            var result = sut.Arguments;

            Assert.That(result.Single(), Is.EqualTo(expectedResult));
        }

        [Test]
        public void InlineDataReturnsCorrectResult()
        {
            var attribute = new AutoTestCaseAttribute(typeof(MyTestClass), "TwoStringParameters", "foo");
            var firstArg = attribute.Arguments[0];
            var secondArg = attribute.Arguments[1];

            Assert.That(firstArg,Is.EqualTo("foo"));
            Assert.That(secondArg, Is.Not.Null);
            Assert.That(secondArg, Is.Not.EqualTo("foo"));
        }

        [Test]
        public void InlineDataTwoParametersReturnsCorrectResult()
        {
            var attribute = new AutoTestCaseAttribute(typeof(MyTestClass), "TwoStringParameters", "foo", "bar");
            var firstArg = attribute.Arguments[0];
            var secondArg = attribute.Arguments[1];

            Assert.That(firstArg, Is.EqualTo("foo"));
            Assert.That(secondArg, Is.EqualTo("bar"));
        }

        [Test]
        public void TestClassTypePropertyIsSetCorrectly()
        {
            var sut = new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), "TestClassTypePropertyIsSetCorrectly");

            Assert.That(sut.TestClassType, Is.EqualTo(typeof(AutoTestCaseAttributeTests)));
        }

        [Test]
        public void MethodNamePropertyIsSetCorrectly()
        {
            var sut = new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), "MethodNamePropertyIsSetCorrectly");

            Assert.That(sut.MethodName, Is.EqualTo("MethodNamePropertyIsSetCorrectly"));
        }
    }
}
