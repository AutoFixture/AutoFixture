using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixtureDocumentationTest.Simple
{
    public interface IBadDesign
    {
        string Message { get; set; }

        void Initialize(MyClass mc);
    }
}
