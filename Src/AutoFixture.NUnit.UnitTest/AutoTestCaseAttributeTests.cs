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
            // Fixture setup
            // Exercise system
            var sut = new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), "SutIsTestCaseAttribute");
            // Verify the outcome
            Assert.IsInstanceOf<TestCaseAttribute>(sut);
            //Teardown
        }

        [Test]
        public void InitializedWithDefaultConstructorHasCorrectFixture()
        {
            // Fixture setup
            var sut = new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), 
                "InitializedWithDefaultConstructorHasCorrectFixture");
            // Exercise system
            IFixture result = sut.Fixture;
            // Verify outcome
            Assert.IsInstanceOf<Fixture>(result);
            // Teardown
        }

        [Test]
        public void InitializeWithNullFixtureThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), 
                "InitializeWithNullFixtureThrows", 
                (IFixture)null, new object[0]));
            // Teardown
        }

        [Test]
        public void InitializedWithComposerHasCorrectComposer()
        {
            // Fixture setup
            var expectedComposer = new DelegatingFixture();
            var sut = new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), 
                "InitializedWithComposerHasCorrectComposer", 
                expectedComposer, new object[0]);
            // Exercise system
            var result = sut.Fixture;
            // Verify outcome
            Assert.That(expectedComposer, Is.EqualTo(result));
            // Teardown
        }

        [Test]
        public void InitializeWithNullTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), 
                "InitializeWithNullTypeThrows", 
                (Type)null));
            // Teardown
        }

        [Test]
        public void InitializeWithNonComposerTypeThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), 
                "InitializeWithNonComposerTypeThrows", 
                typeof(object)));
            // Teardown
        }

        [Test]
        public void InitializeWithComposerTypeWithoutDefaultConstructorThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), 
                "InitializeWithComposerTypeWithoutDefaultConstructorThrows", 
                typeof(ComposerWithoutADefaultConstructor)));
            // Teardown
        }

        [Test]
        public void InitializedWithCorrectComposerTypeHasCorrectComposer()
        {
            // Fixture setup
            var composerType = typeof (DelegatingFixture);
            var sut = new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), 
                "InitializedWithCorrectComposerTypeHasCorrectComposer", 
                composerType);
            // Exercise system
            var result = sut.Fixture;
            // Verify outcome
            Assert.IsInstanceOf(composerType,result);
            // Teardown
        }

        [Test]
        public void FixtureTypeIsCorrect()
        {
            // Fixture setup
            var composerType = typeof (DelegatingFixture);
            var sut = new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), 
                "FixtureTypeIsCorrect", 
                composerType);
            // Exercise system
            var result = sut.FixtureType;
            // Verify outcome
            Assert.That(composerType, Is.EqualTo(result));
            // Teardown
        }

        [Test]
        public void GetDataWithInvalidMethodThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentException>(() => new AutoTestCaseAttribute(typeof(TypeWithMembers),"Foo"));
            // Teardown
        }

        [Test]
        public void GetDataReturnsCorrectResult()
        {
            // Fixture setup
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

            var sut = new AutoTestCaseAttribute(typeof(TypeWithMembers),"DoSomething",composer,new object[0]);
            // Exercise system
            var result = sut.Arguments;
            // Verify outcome
            Assert.That(result.Single(), Is.EqualTo(expectedResult));
            // Teardown
        }

        [Test]
        public void ManualParameterReturnsCorrectResult()
        {
            // Fixture setup
            var attribute = new AutoTestCaseAttribute(typeof(MyTestClass), "TwoStringParameters", "foo");
            // Exercise system
            var firstArg = attribute.Arguments[0];
            var secondArg = attribute.Arguments[1];
            // Verify outcome
            Assert.That(firstArg,Is.EqualTo("foo"));
            Assert.That(secondArg, Is.Not.Null);
            Assert.That(secondArg, Is.Not.EqualTo("foo"));
            // Teardown
        }

        [Test]
        public void TwoManualParametersReturnsCorrectResult()
        {
            // Fixture setup
            var attribute = new AutoTestCaseAttribute(typeof(MyTestClass), "TwoStringParameters", "foo", "bar");
            // Exercise system
            var firstArg = attribute.Arguments[0];
            var secondArg = attribute.Arguments[1];
            // Verify outcome
            Assert.That(firstArg, Is.EqualTo("foo"));
            Assert.That(secondArg, Is.EqualTo("bar"));
            // Teardown
        }

        [Test]
        public void TestClassTypePropertyIsSetCorrectly()
        {
            // Fixture setup
            var sut = new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), 
                "TestClassTypePropertyIsSetCorrectly");
            // Exercise system and verify outcome
            Assert.That(sut.TestClassType, Is.EqualTo(typeof(AutoTestCaseAttributeTests)));
            // Teardown
        }

        [Test]
        public void MethodNamePropertyIsSetCorrectly()
        {
            // Fixture setup
            var sut = new AutoTestCaseAttribute(typeof(AutoTestCaseAttributeTests), 
                "MethodNamePropertyIsSetCorrectly");
            // Exercise system and verify outcome
            Assert.That(sut.MethodName, Is.EqualTo("MethodNamePropertyIsSetCorrectly"));
            // Teardown
        }
    }
}
