using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.SemanticComparison.Fluent;

namespace Ploeh.SemanticComparison.UnitTest
{
    public class ProxyGeneratorTest
    {
        [Fact]
        public void CreateProxyDoesNotThrowExceptionWhenAllSourcePropertiesAreNotNull()
        {
            var obj = new SourceType(string.Empty);
            Assert.DoesNotThrow(() => obj.AsSource().OfLikeness<DestinationType>().CreateProxy());            
        }

        [Fact]
        public void CreateProxyDoesNotThrowExceptionWhenATopLevelSourcePropertyIsNull()      
        {
            var obj = new SourceType(null);
            Assert.DoesNotThrow(() => obj.AsSource().OfLikeness<DestinationType>().CreateProxy());            
        }
    }
       
    public class SourceType
    {
        public SourceType(string aString)
        {
            this.TheString = aString;
        }

        public string TheString { get; set; }        
    }

    public class DestinationType
    {
        public DestinationType(string aString)
        {
            this.TheString = aString;
        }

        public string TheString { get; set; }       
    }
}
