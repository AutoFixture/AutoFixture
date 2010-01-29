using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixtureDocumentationTest.Contact.Parsing
{
    public class Contact
    {
        public Contact(string name, string phoneNumber)
        {
            this.Name = name;
            this.PhoneNumber = 
                Contact.ParsePhoneNumber(phoneNumber);
        }

        public string Name { get; set; }

        public int PhoneNumber { get; set; }

        private static int ParsePhoneNumber(string phoneNumber)
        {
            return int.Parse(phoneNumber);
        }
    }
}
