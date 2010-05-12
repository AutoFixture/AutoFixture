using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Linq.Expressions;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    /// <summary>
    /// Original source code can be found at http://clarius.codeplex.com/, changeset 35515. The namespace and the unit testing framework has been changed here. 
    /// </summary>
    [TestClass]
    public class ReflectorFixture
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ShouldThrowIfNullMethodLambda()
        {
            Reflect<Mock>.GetMethod((Expression<Action<Mock>>)null);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ShouldThrowIfNullPropertyLambda()
        {
            Reflect<Mock>.GetProperty((Expression<Func<Mock, object>>)null);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ShouldThrowIfNotMethodLambda()
        {
            Reflect<Mock>.GetMethod(x => new object());
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ShouldThrowIfNotPropertyLambda()
        {
            Reflect<Mock>.GetProperty(x => x.PublicField);
        }

        [TestMethod]
        public void ShouldGetPublicProperty()
        {
            PropertyInfo info = Reflect<Mock>.GetProperty(x => x.PublicProperty);
            Assert.IsTrue(info == typeof(Mock).GetProperty("PublicProperty"));
        }

        [TestMethod]
        public void ShouldGetPublicVoidMethod()
        {
            MethodInfo info = Reflect<Mock>.GetMethod(x => x.PublicVoidMethod());
            Assert.IsTrue(info == typeof(Mock).GetMethod("PublicVoidMethod"));
        }

        [TestMethod]
        public void ShouldGetPublicMethodParameterless()
        {
            MethodInfo info = Reflect<Mock>.GetMethod(x => x.PublicMethodNoParameters());
            Assert.IsTrue(info == typeof(Mock).GetMethod("PublicMethodNoParameters"));
        }

        [TestMethod]
        public void ShouldGetPublicMethodParameters()
        {
            MethodInfo info = Reflect<Mock>.GetMethod<string, int>(
                (x, y, z) => x.PublicMethodParameters(y, z));
            Assert.IsTrue(info == typeof(Mock).GetMethod("PublicMethodParameters", new Type[] { typeof(string), typeof(int) }));
        }

        [TestMethod]
        public void ShouldGetNonPublicProperty()
        {
            PropertyInfo info = Reflect<ReflectorFixture>.GetProperty(x => x.NonPublicProperty);
            Assert.IsTrue(info == typeof(ReflectorFixture).GetProperty("NonPublicProperty", BindingFlags.Instance | BindingFlags.NonPublic));
        }

        [TestMethod]
        public void ShouldGetNonPublicMethod()
        {
            MethodInfo info = Reflect<ReflectorFixture>.GetMethod(x => x.NonPublicMethod());
            Assert.IsTrue(info == typeof(ReflectorFixture).GetMethod("NonPublicMethod", BindingFlags.Instance | BindingFlags.NonPublic));
        }

        private int NonPublicField;

        private int NonPublicProperty
        {
            get { return NonPublicField; }
            set { NonPublicField = value; }
        }

        private object NonPublicMethod()
        {
            throw new NotImplementedException();
        }

        public class Mock
        {
            public int Value;
            public bool PublicField;
            private int valueProp;

            public Mock()
            {
            }

            public Mock(string foo, int bar)
            {
            }

            public int PublicProperty
            {
                get { return valueProp; }
                set { valueProp = value; }
            }

            public bool PublicMethodNoParameters()
            {
                throw new NotImplementedException();
            }

            public bool PublicMethodParameters(string foo, int bar)
            {
                throw new NotImplementedException();
            }

            public void PublicVoidMethod()
            {
                throw new NotImplementedException();
            }
        }
    }
}