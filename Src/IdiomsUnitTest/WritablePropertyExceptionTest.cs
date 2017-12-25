using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using AutoFixture.Idioms;
using Xunit;

namespace AutoFixture.IdiomsUnitTest
{
    public class WritablePropertyExceptionTest
    {
        [Fact]
        public void SutIsException()
        {
            // Arrange
            var dummyProperty = typeof(Version).GetProperties().First();
            // Act
            var sut = new WritablePropertyException(dummyProperty);
            // Assert
            Assert.IsAssignableFrom<Exception>(sut);
        }

        [Fact]
        public void PropertyIsCorrectWhenConstructedWithSimplestConstructor()
        {
            // Arrange
            var expectedProperty = typeof(Version).GetProperties().First();
            var sut = new WritablePropertyException(expectedProperty);
            // Act
            PropertyInfo result = sut.PropertyInfo;
            // Assert
            Assert.Equal(expectedProperty, result);
        }

        [Fact]
        public void MessageIsNotNull()
        {
            // Arrange
            var dummyProperty = typeof(Version).GetProperties().First();
            var sut = new WritablePropertyException(dummyProperty);
            // Act
            var result = sut.Message;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void PropertyIsCorrectWhenConstructedWithMessage()
        {
            // Arrange
            var expectedProperty = typeof(Version).GetProperties().First();
            var dummyMessage = "Anonymous message";
            var sut = new WritablePropertyException(expectedProperty, dummyMessage);
            // Act
            PropertyInfo result = sut.PropertyInfo;
            // Assert
            Assert.Equal(expectedProperty, result);
        }

        [Fact]
        public void MessageIsCorrect()
        {
            // Arrange
            var dummyProperty = typeof(Version).GetProperties().First();
            var expected = Guid.NewGuid().ToString();
            var sut = new WritablePropertyException(dummyProperty, expected);
            // Act
            var result = sut.Message;
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void PropertyIsCorrectWhenConstructedWithMessageAndInnerException()
        {
            // Arrange
            var expectedProperty = typeof(Version).GetProperties().First();
            var dummyMessage = "Anonymous message";
            var dummyInner = new Exception();
            var sut = new WritablePropertyException(expectedProperty, dummyMessage, dummyInner);
            // Act
            PropertyInfo result = sut.PropertyInfo;
            // Assert
            Assert.Equal(expectedProperty, result);
        }

        [Fact]
        public void MessageIsCorrectWhenConstructedWithInnerException()
        {
            // Arrange
            var dummyProperty = typeof(Version).GetProperties().First();
            var expected = Guid.NewGuid().ToString();
            var dummyInner = new Exception();
            var sut = new WritablePropertyException(dummyProperty, expected, dummyInner);
            // Act
            var result = sut.Message;
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void InnerExceptionIsCorrect()
        {
            // Arrange
            var dummyProperty = typeof(Version).GetProperties().First();
            var expected = new Exception();
            var sut = new WritablePropertyException(dummyProperty, "Anonymous value", expected);
            // Act
            var result = sut.InnerException;
            // Assert
            Assert.Equal<Exception>(expected, result);
        }

        [Fact]
        public void MessageSerializesCorrectly()
        {
            // Arrange
            var dummyProperty = typeof(Version).GetProperties().First();
            var message = Guid.NewGuid().ToString();
            var sut = new WritablePropertyException(dummyProperty, message);

            var formatter = new BinaryFormatter();
            // Act
            using (var s = new MemoryStream())
            {
                formatter.Serialize(s, sut);
                s.Flush();
                s.Position = 0;
                var result = formatter.Deserialize(s);
                // Assert
                var e = Assert.IsAssignableFrom<WritablePropertyException>(result);
                Assert.Equal(message, e.Message);
            }
        }

        [Fact]
        public void InnerExceptionSerializesCorrectly()
        {
            // Arrange
            var dummyProperty = typeof(Version).GetProperties().First();
            var dummyMessage = Guid.NewGuid().ToString();
            var innerException = new Exception(Guid.NewGuid().ToString());
            var sut = new WritablePropertyException(dummyProperty, dummyMessage, innerException);

            var formatter = new BinaryFormatter();
            // Act
            using (var s = new MemoryStream())
            {
                formatter.Serialize(s, sut);
                s.Flush();
                s.Position = 0;
                var result = formatter.Deserialize(s);
                // Assert
                var e = Assert.IsAssignableFrom<WritablePropertyException>(result);
                Assert.Equal(innerException.Message, e.InnerException.Message);
            }
        }

#if SERIALIZABLE_MEMBERINFO
        [Fact]
        public void PropertyInfosSerializesCorrectly()
        {
            // Arrange
            var property = typeof(Version).GetProperties().First();
            var dummyMessage = Guid.NewGuid().ToString();
            var sut = new WritablePropertyException(property, dummyMessage);

            var formatter = new BinaryFormatter();
            // Act
            using (var s = new MemoryStream())
            {
                formatter.Serialize(s, sut);
                s.Flush();
                s.Position = 0;
                var result = formatter.Deserialize(s);
                // Assert
                var e = Assert.IsAssignableFrom<WritablePropertyException>(result);
                Assert.Equal(property, e.PropertyInfo);
            }
        }
#endif
    }
}
