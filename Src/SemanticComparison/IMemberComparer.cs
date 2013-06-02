using System.Collections;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    public interface IMemberComparer : IEqualityComparer
    {
        bool IsSatisfiedBy(PropertyInfo pi);
        bool IsSatisfiedBy(FieldInfo fi);
    }
}
