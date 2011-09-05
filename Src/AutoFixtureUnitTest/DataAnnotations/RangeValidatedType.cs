using System.ComponentModel.DataAnnotations;

namespace Ploeh.AutoFixtureUnitTest.DataAnnotations
{
    public class RangeValidatedType
    {
        [Range(10, 20)]
        public int Property { get; set; }
    }
}