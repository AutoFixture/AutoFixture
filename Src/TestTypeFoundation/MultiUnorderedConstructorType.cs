using System;

namespace TestTypeFoundation
{
    public class MultiUnorderedConstructorType
    {
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
                throw new ArgumentNullException(nameof(text));
            }

            this.Text = text;
            this.Number = number;
        }

        public string Text { get; }

        public int Number { get; }

        public class ParameterObject
        {
            public ParameterObject(string text, int number)
            {
                if (text == null)
                {
                    throw new ArgumentNullException(nameof(text));
                }

                this.Text = text;
                this.Number = number;
            }

            public string Text { get; }

            public int Number { get; }
        }
    }
}
