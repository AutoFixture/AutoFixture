using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class WritablePropertyExceptionTest
    {
        [Fact]
        public void SutIsException()
        {
            // Fixture setup
            var dummyProperty = typeof(Version).GetProperties().First();
            // Exercise system
            var sut = new WritablePropertyException(dummyProperty);
            // Verify outcome
            Assert.IsAssignableFrom<Exception>(sut);
            // Teardown
        }

        [Fact]
        public void PropertyIsCorrectWhenConstructedWithSimplestConstructor()
        {
            // Fixture setup
            var expectedProperty = typeof(Version).GetProperties().First();
            var sut = new WritablePropertyException(expectedProperty);
            // Exercise system
            PropertyInfo result = sut.PropertyInfo;
            // Verify outcome
            Assert.Equal(expectedProperty, result);
            // Teardown
        }

        [Fact]
        public void MessageIsNotNull()
        {
            // Fixture setup
            var dummyProperty = typeof(Version).GetProperties().First();
            var sut = new WritablePropertyException(dummyProperty);
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void PropertyIsCorrectWhenConstructedWithMessage()
        {
            // Fixture setup
            var expectedProperty = typeof(Version).GetProperties().First();
            var dummyMessage = "Anonymous message";
            var sut = new WritablePropertyException(expectedProperty, dummyMessage);
            // Exercise system
            PropertyInfo result = sut.PropertyInfo;
            // Verify outcome
            Assert.Equal(expectedProperty, result);
            // Teardown
        }

        [Fact]
        public void MessageIsCorrect()
        {
            // Fixture setup
            var dummyProperty = typeof(Version).GetProperties().First();
            var expected = Guid.NewGuid().ToString();
            var sut = new WritablePropertyException(dummyProperty, expected);
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.Equal<string>(expected, result);
            // Teardown
        }

        [Fact]
        public void PropertyIsCorrectWhenConstructedWithMessageAndInnerException()
        {
            // Fixture setup
            var expectedProperty = typeof(Version).GetProperties().First();
            var dummyMessage = "Anonymous message";
            var dummyInner = new Exception();
            var sut = new WritablePropertyException(expectedProperty, dummyMessage, dummyInner);
            // Exercise system
            PropertyInfo result = sut.PropertyInfo;
            // Verify outcome
            Assert.Equal(expectedProperty, result);
            // Teardown
        }

        [Fact]
        public void MessageIsCorrectWhenConstructedWithInnerException()
        {
            // Fixture setup
            var dummyProperty = typeof(Version).GetProperties().First();
            var expected = Guid.NewGuid().ToString();
            var dummyInner = new Exception();
            var sut = new WritablePropertyException(dummyProperty, expected, dummyInner);
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void InnerExceptionIsCorrect()
        {
            // Fixture setup
            var dummyProperty = typeof(Version).GetProperties().First();
            var expected = new Exception();
            var sut = new WritablePropertyException(dummyProperty, "Anonymous value", expected);
            // Exercise system
            var result = sut.InnerException;
            // Verify outcome
            Assert.Equal<Exception>(expected, result);
            // Teardown
        }

        [Fact]
        public void MessageSerializesCorrectly()
        {
            // Fixture setup
            var dummyProperty = typeof(Version).GetProperties().First();
            var message = Guid.NewGuid().ToString();
            var sut = new WritablePropertyException(dummyProperty, message);

            var formatter = new BinaryFormatter();
            // Exercise system
            using (var s = new MemoryStream())
            {
                formatter.Serialize(s, sut);
                s.Flush();
                s.Position = 0;
                var result = formatter.Deserialize(s);
                // Verify outcome
                var e = Assert.IsAssignableFrom<WritablePropertyException>(result);
                Assert.Equal(message, e.Message);
                // Teardown
            }
        }

        [Fact]
        public void InnerExceptionSerializesCorrectly()
        {
            // Fixture setup
            var dummyProperty = typeof(Version).GetProperties().First();
            var dummyMessage = Guid.NewGuid().ToString();
            var innerException = new Exception(Guid.NewGuid().ToString());
            var sut = new WritablePropertyException(dummyProperty, dummyMessage, innerException);

            var formatter = new BinaryFormatter();
            // Exercise system
            using (var s = new MemoryStream())
            {
                formatter.Serialize(s, sut);
                s.Flush();
                s.Position = 0;
                var result = formatter.Deserialize(s);
                // Verify outcome
                var e = Assert.IsAssignableFrom<WritablePropertyException>(result);
                Assert.Equal(innerException.Message, e.InnerException.Message);
                // Teardown
            }
        }

        [Fact]
        public void PropertyInfosSerializesCorrectly()
        {
            // Fixture setup
            var property = typeof(Version).GetProperties().First();
            var dummyMessage = Guid.NewGuid().ToString();
            var sut = new WritablePropertyException(property, dummyMessage);

            var formatter = new BinaryFormatter();
            // Exercise system
            using (var s = new MemoryStream())
            {
                formatter.Serialize(s, sut);
                s.Flush();
                s.Position = 0;
                var result = formatter.Deserialize(s);
                // Verify outcome
                var e = Assert.IsAssignableFrom<WritablePropertyException>(result);
                Assert.Equal(property, e.PropertyInfo);
                // Teardown
            }
        }
    }
}
