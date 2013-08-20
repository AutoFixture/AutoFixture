namespace AutoSpecificationFor.Tests.Models
{
    public class Simple : ISimple
    {
        public string PropertyExample { get; set; }

        public string MethodExample(string value)
        {
            return value;
        }
    }
}