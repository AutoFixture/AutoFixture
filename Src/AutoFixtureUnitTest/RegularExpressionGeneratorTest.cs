using System.Text.RegularExpressions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class RegularExpressionGeneratorTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new RegularExpressionGenerator();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new RegularExpressionGenerator();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = sut.Create(null, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Fact]
        public void CreateWithAnonymousRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new RegularExpressionGenerator();
            var request = new object();
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }

        [Theory]
        [InlineData("[ab]{4,6}")]
        [InlineData("[ab]{4,6}c")]
        [InlineData("(a|b)*ab")]
        [InlineData("[A-Za-z0-9]")]
        [InlineData("[A-Za-z0-9_]")]
        [InlineData("[A-Za-z]")]
        [InlineData("[ \t]")]
        [InlineData(@"[(?<=\W)(?=\w)|(?<=\w)(?=\W)]")]
        [InlineData("[\x00-\x1F\x7F]")]
        [InlineData("[0-9]")]
        [InlineData("[^0-9]")]
        [InlineData("[\x21-\x7E]")]
        [InlineData("[a-z]")]
        [InlineData("[\x20-\x7E]")]
        [InlineData("[ \t\r\n\v\f]")]
        [InlineData("[^ \t\r\n\v\f]")]
        [InlineData("[A-Z]")]
        [InlineData("[A-Fa-f0-9]")]
        [InlineData("in[du]")]
        [InlineData("x[0-9A-Z]")]
        [InlineData("[^A-M]in")]
        [InlineData("W*in")]
        [InlineData("[xX][0-9a-z]")]
        [InlineData(@"\(\(\(ab\)*c\)*d\)\(ef\)*\(gh\)\{2\}\(ij\)*\(kl\)*\(mn\)*\(op\)*\(qr\)*")]
        [InlineData(@"^[a-zA-Z0-9\-\.]+\.(com|org|net|mil|edu|COM|ORG|NET|MIL|EDU)$")]
        [InlineData(@"((mailto\:|(news|(ht|f)tp(s?))\://){1}\S+)")]
        [InlineData(@"^http\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$")]
        [InlineData(@"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?")]
        [InlineData(@"^(http|https|ftp)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&amp;%\$\-]+)*@)?((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.[a-zA-Z]{2,4})(\:[0-9]+)?(/[^/][a-zA-Z0-9\.\,\?\'\\/\+&amp;%\$#\=~_\-@]*)*$")]
        [InlineData(@"^([1-zA-Z0-1@.\s]{1,255})$")]
        [InlineData("[A-Z][0-9A-Z]{10}")]
        [InlineData("[A-Z][A-Za-z0-9]{10}")]
        [InlineData("[A-Z][a-zA-Z0-9]{10}")]
        [InlineData("[A-Za-z0-9]{11}")]
        [InlineData("[A-Za-z]{11}")]
        [InlineData(@"^[a-zA-Z''-'\s]{1,40}$")]
        [InlineData(@"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$")]
        [InlineData(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$")]
        [InlineData(@"\d{8}")]
        [InlineData(@"\d{5}(-\d{4})?")]
        [InlineData(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")]
        [InlineData(@"^(?:[a-z0-9])+$")]
        [InlineData(@"^(?i:[a-z0-9])+$")]
        [InlineData(@"^(?s:[a-z0-9])+$")]
        [InlineData(@"^(?m:[a-z0-9])+$")]
        [InlineData(@"^(?n:[a-z0-9])+$")]
        [InlineData(@"^(?x:[a-z0-9])+$")]
        public void CreateWithRegularExpressionRequestReturnsCorrectResult(string pattern)
        {
            // Fixture setup
            var sut = new RegularExpressionGenerator();
            var request = new RegularExpressionRequest(pattern);
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            Assert.True(Regex.IsMatch(result.ToString(), pattern), string.Format("result: {0}", result));
            // Teardown
        }

        [Theory]
        [InlineData("[")]
        [InlineData(@"(?\[Test\]|\[Foo\]|\[Bar\])?(?:-)?(?\[[()a-zA-Z0-9_\s]+\])?(?:-)?(?\[[a-zA-Z0-9_\s]+\])?(?:-)?(?\[[a-zA-Z0-9_\s]+\])?(?:-)?(?\[[a-zA-Z0-9_\s]+\])?")]
        public void CreateWithNotSupportedRegularExpressionRequestReturnsCorrectResult(string pattern)
        {
            // Fixture setup
            var sut = new RegularExpressionGenerator();
            var request = new RegularExpressionRequest(pattern);
            var dummyContext = new DelegatingSpecimenContext();
            // Exercise system
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            Assert.Equal(new NoSpecimen(), result);
            // Teardown
        }
    }
}
