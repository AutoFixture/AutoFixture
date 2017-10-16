using System.ComponentModel.DataAnnotations;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class StringLengthValidatedType
    {
        public const int MaximumLength = 3;

        [StringLength(MaximumLength)]
        public string Property { get; set; }
    }
}