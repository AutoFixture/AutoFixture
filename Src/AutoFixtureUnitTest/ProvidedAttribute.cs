using System;

namespace AutoFixtureUnitTest;

internal class ProvidedAttribute
{
    public ProvidedAttribute(Attribute attribute, bool inherited)
    {
        Attribute = attribute;
        Inherited = inherited;
    }

    public Attribute Attribute { get; }

    public bool Inherited { get; }
}