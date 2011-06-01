using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using System.Reflection;

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
    }
}
