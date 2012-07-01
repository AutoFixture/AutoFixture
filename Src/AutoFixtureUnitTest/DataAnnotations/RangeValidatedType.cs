using System.ComponentModel.DataAnnotations;

namespace Ploeh.AutoFixtureUnitTest.DataAnnotations
{
    public class RangeValidatedType
    {
        public const int Minimum = 10;
        public const int Maximum = 20;

        [Range(Minimum, Maximum)]
        public decimal Property { get; set; }
    }
}