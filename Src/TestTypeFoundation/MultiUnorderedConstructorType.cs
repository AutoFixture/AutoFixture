using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class MultiUnorderedConstructorType
    {
        private readonly string text;
        private readonly int number;

        public MultiUnorderedConstructorType(ParameterObject paramObj)
            : this(paramObj.Text, paramObj.Number)
        {
        }

        public MultiUnorderedConstructorType()
            : this(string.Empty, 0)
        {
        }

        public MultiUnorderedConstructorType(string text, int number)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            this.text = text;
            this.number = number;
        }

        public string Text
        {
            get { return this.text; }
        }

        public int Number
        {
            get { return this.number; }
        }

        public class ParameterObject
        {
            private readonly string text;
            private readonly int number;

            public ParameterObject(string text, int number)
            {
                if (text == null)
                {
                    throw new ArgumentNullException("text");
                }

                this.text = text;
                this.number = number;
            }

            public string Text
            {
                get { return this.text; }
            }

            public int Number
            {
                get { return this.number; }
            }
        }
    }
}
