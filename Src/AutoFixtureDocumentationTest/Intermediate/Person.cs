﻿using System;

namespace AutoFixtureDocumentationTest.Intermediate
{
    public class Person
    {
        private Person spouse;

        public DateTime BirthDay { get; set; }

        public string Name { get; set; }

        public Person Spouse
        {
            get => this.spouse;
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
