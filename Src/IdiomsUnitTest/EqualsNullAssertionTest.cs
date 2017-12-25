using System;
using System.Reflection;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class EqualsNullAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Arrange
            var dummyComposer = new Fixture();
            // Act
            var sut = new EqualsNullAssertion(dummyComposer);
            // Assert
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Arrange
            var expectedComposer = new Fixture();
            var sut = new EqualsNullAssertion(expectedComposer);
            // Act
            ISpecimenBuilder result = sut.Builder;
            // Assert
            Assert.Equal(expectedComposer, result);
        }

        [Fact]
        public void ConstructWithNullComposerThrows()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new EqualsNullAssertion(null));
        }

        [Fact]
        public void VerifyNullMethodThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualsNullAssertion(dummyComposer);
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((MethodInfo)null));
        }

        [Fact]
        public void VerifyClassThatDoesNotOverrideObjectEqualsDoesNothing()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualsNullAssertion(dummyComposer);
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(ClassThatDoesNotOverrideObjectEquals))));
        }

        [Fact]
        public void VerifyWellBehavedEqualsNullOverrideDoesNotThrow()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualsNullAssertion(dummyComposer);
            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(typeof(WellBehavedEqualsNullOverride))));
        }

        [Fact]
        public void VerifyIllbehavedEqualsNullBehaviourThrows()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualsNullAssertion(dummyComposer);
            // Act & Assert
            Assert.Throws<EqualsOverrideException>(() =>
                sut.Verify(typeof(IllbehavedEqualsNullOverride)));
        }

        [Fact]
        public void VerifyAnonymousMethodWithNoDeclaringOrReflectedTypeDoesNothing()
        {
            // Arrange
            var dummyComposer = new Fixture();
            var sut = new EqualsNullAssertion(dummyComposer);
            var method = (MethodInfo)(new MethodInfoWithNullDeclaringAndReflectedType());

            // Act & Assert
            Assert.Null(Record.Exception(() =>
                sut.Verify(method)));
        }

        class MethodInfoWithNullDeclaringAndReflectedType : MethodInfo
        {
            public override Type ReflectedType
            {
                get { return null; }
            }

            public override Type DeclaringType
            {
                get { return null; }
            }

            #region Other

            public override MethodInfo GetBaseDefinition()
            {
                throw new NotImplementedException();
            }

            public override ICustomAttributeProvider ReturnTypeCustomAttributes
            {
                get { throw new NotImplementedException(); }
            }

            public override MethodAttributes Attributes
            {
                get { throw new NotImplementedException(); }
            }

            public override MethodImplAttributes GetMethodImplementationFlags()
            {
                throw new NotImplementedException();
            }

            public override ParameterInfo[] GetParameters()
            {
                throw new NotImplementedException();
            }

            public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            public override RuntimeMethodHandle MethodHandle
            {
                get { throw new NotImplementedException(); }
            }

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }

            public override object[] GetCustomAttributes(bool inherit)
            {
                throw new NotImplementedException();
            }

            public override bool IsDefined(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }

            public override string Name
            {
                get { throw new NotImplementedException(); }
            }

            #endregion
        }

#pragma warning disable 659
        class IllbehavedEqualsNullOverride
        {
            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return true;
                }
                throw new Exception();
            }
        }

        class WellBehavedEqualsNullOverride
        {
            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                throw new Exception();
            }
        }
#pragma warning restore 659

        class ClassThatDoesNotOverrideObjectEquals
        {
        }
    }

}
