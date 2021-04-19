using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace AutoFixture.NUnit3
{
    internal interface IPatchParameters
    {
        void Patch(TestCaseParameters parameters, IMethodInfo method);
    }
}