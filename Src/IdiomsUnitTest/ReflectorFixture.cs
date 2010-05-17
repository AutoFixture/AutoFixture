using System;
using System.Reflection;
using System.Linq.Expressions;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    /// <summary>
    /// Original source code can be found at http://clarius.codeplex.com/, changeset 35515. The namespace and the unit testing framework has been changed here. 
    /// </summary>
    public class ReflectorFixture
    {
        [Fact]
        public void ShouldThrowIfNullMethodLambda()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
                Reflect<Mock>.GetMethod((Expression<Action<Mock>>)null));
        }

        [Fact]
        public void ShouldThrowIfNullPropertyLambda()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
                Reflect<Mock>.GetProperty((Expression<Func<Mock, object>>)null));
        }

        [Fact]
        public void ShouldThrowIfNotMethodLambda()
        {
            Assert.Throws(typeof(ArgumentException), () =>
                Reflect<Mock>.GetMethod(x => new object()));
        }

        [Fact]
        public void ShouldThrowIfNotPropertyLambda()
        {
            Assert.Throws(typeof(ArgumentException), () =>
                Reflect<Mock>.GetProperty(x => x.PublicField));
        }

        [Fact]
        public void ShouldGetPublicProperty()
        {
            PropertyInfo info = Reflect<Mock>.GetProperty(x => x.PublicProperty);
            Assert.True(info == typeof(Mock).GetProperty("PublicProperty"));
        }

        [Fact]
        public void ShouldGetPublicVoidMethod()
        {
            MethodInfo info = Reflect<Mock>.GetMethod(x => x.PublicVoidMethod());
            Assert.True(info == typeof(Mock).GetMethod("PublicVoidMethod"));
        }

        [Fact]
        public void ShouldGetPublicMethodParameterless()
        {
            MethodInfo info = Reflect<Mock>.GetMethod(x => x.PublicMethodNoParameters());
            Assert.True(info == typeof(Mock).GetMethod("PublicMethodNoParameters"));
        }

        [Fact]
        public void ShouldGetPublicMethodParameters()
        {
            MethodInfo info = Reflect<Mock>.GetMethod<string, int>(
                (x, y, z) => x.PublicMethodParameters(y, z));
            Assert.True(info == typeof(Mock).GetMethod("PublicMethodParameters", new Type[] { typeof(string), typeof(int) }));
        }

        [Fact]
        public void ShouldGetNonPublicProperty()
        {
            PropertyInfo info = Reflect<ReflectorFixture>.GetProperty(x => x.NonPublicProperty);
            Assert.True(info == typeof(ReflectorFixture).GetProperty("NonPublicProperty", BindingFlags.Instance | BindingFlags.NonPublic));
        }

        [Fact]
        public void ShouldGetNonPublicMethod()
        {
            MethodInfo info = Reflect<ReflectorFixture>.GetMethod(x => x.NonPublicMethod());
            Assert.True(info == typeof(ReflectorFixture).GetMethod("NonPublicMethod", BindingFlags.Instance | BindingFlags.NonPublic));
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