namespace AutoSpecificationFor.Tests.Models
{
    public interface ISimple
    {
        string PropertyExample { get; set; }

        string MethodExample(string value);
    }
}