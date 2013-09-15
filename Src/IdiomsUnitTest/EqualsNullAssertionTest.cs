using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class EqualsNullAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            // Exercise system
            var sut = new EqualsNullAssertion(dummyComposer);
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrect()
        {
            // Fixture setup
            var expectedComposer = new Fixture();
            var sut = new EqualsNullAssertion(expectedComposer);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void ConstructWithNullComposerThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new EqualsNullAssertion(null));
            // Teardown
        }

        [Fact]
        public void VerifyNullMethodThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsNullAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Verify((MethodInfo)null));
            // Teardown
        }

        [Fact]
        public void VerifyClassThatDoesNotOverrideObjectEqualsDoesNothing()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsNullAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(typeof(ClassThatDoesNotOverrideObjectEquals)));
            // Teardown
        }

        [Fact]
        public void VerifyWellBehavedEqualsNullOverrideDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsNullAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(typeof(WellBehavedEqualsNullOverride)));
            // Teardown            
        }

        [Fact]
        public void VerifyIllbehavedEqualsNullBehaviourThrows()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsNullAssertion(dummyComposer);
            // Exercise system and verify outcome
            Assert.Throws<EqualsOverrideException>(() =>
                sut.Verify(typeof(IllbehavedEqualsNullOverride)));
            // Teardown
        }

        [Fact]
        public void VerifyAnonymousMethodWithNoDeclaringOrReflectedTypeDoesNothing()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new EqualsNullAssertion(dummyComposer);
            var method = (MethodInfo)(new MethodInfoWithNullDeclaringAndReflectedType());

            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                sut.Verify(method));
            // Teardown
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
