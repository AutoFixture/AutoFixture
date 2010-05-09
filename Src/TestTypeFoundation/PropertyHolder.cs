using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.TestTypeFoundation
{
    public class PropertyHolder<T>
    {
        public T Property { get; set; }

        public void SetProperty(T value)
        {
            this.Property = value;
        }
    }
}
