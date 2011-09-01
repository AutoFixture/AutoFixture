using System.ComponentModel.DataAnnotations;

namespace Ploeh.AutoFixtureUnitTest.DataAnnotations
{
    public class RangeValidatedType
    {
        [Range(1, 3)]
        public int Property { get; set; }
    }
}