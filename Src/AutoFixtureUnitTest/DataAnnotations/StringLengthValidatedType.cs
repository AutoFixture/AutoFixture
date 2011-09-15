using System.ComponentModel.DataAnnotations;

namespace Ploeh.AutoFixtureUnitTest.DataAnnotations
{
    public class StringLengthValidatedType
    {
        [StringLength(3)]
        public string Property { get; set; }
    }
}
