using System.Collections.Generic;

namespace TestTypeFoundation;

public class TypeWithIndexer
{
    private readonly Dictionary<string, string> _dict;

    public TypeWithIndexer()
    {
        _dict = new Dictionary<string, string>();
    }

    public string this[string index]
    {
        get
        {
            return _dict[index];
        }
        set
        {
            _dict[index] = value;
        }
    }
}