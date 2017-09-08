using System;

namespace Ploeh.AutoFixtureDocumentationTest.Intermediate
{
    public class Person
    {
        private Person spouse;

        public DateTime BirthDay { get; set; }

        public string Name { get; set; }

        public Person Spouse 
        {
            get { return this.spouse; }
            set
            {
                this.spouse = value;
                if (value != null)
                {
                    value.spouse = this;
                }
            }
        }
    }
}
