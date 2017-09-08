namespace Ploeh.AutoFixtureDocumentationTest.Simple
{
    public class ComplexChild
    {
        public ComplexChild(string name)
        {
            this.Name = name;
        }

        public ComplexChild(string name, int number)
        {
            this.Name = name;
            this.Number = number;
        }

        public string Name { get; }

        public int Number { get; set; }
    }
}
