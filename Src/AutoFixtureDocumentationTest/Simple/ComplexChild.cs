namespace Ploeh.AutoFixtureDocumentationTest.Simple
{
    public class ComplexChild
    {
        private readonly string name;

        public ComplexChild(string name)
        {
            this.name = name;
        }

        public ComplexChild(string name, int number)
        {
            this.name = name;
            this.Number = number;
        }

        public string Name
        {
            get { return this.name; }
        }

        public int Number { get; set; }
    }
}
