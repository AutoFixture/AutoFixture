using System;

namespace AutoFixtureDocumentationTest.Extension.Constraints;

public class ConstrainedStringGenerator
{
    private readonly int _minimumLength;
    private readonly int _maximumLength;

    public ConstrainedStringGenerator(int minimumLength,
        int maximumLength)
    {
        if (maximumLength < 0)
        {
            throw new ArgumentOutOfRangeException("...");
        }
        if (minimumLength > maximumLength)
        {
            throw new ArgumentOutOfRangeException("...");
        }

        _minimumLength = minimumLength;
        _maximumLength = maximumLength;
    }

    public string CreateaAnonymous(string seed)
    {
        var s = string.Empty;
        while (s.Length < _minimumLength)
        {
            s += ConstrainedStringGenerator.CreateAnonymous(seed);
        }
        if (s.Length > _maximumLength)
        {
            s = s.Substring(0, _maximumLength);
        }
        return s;
    }

    private static string CreateAnonymous(string seed)
    {
        return seed + Guid.NewGuid();
    }
}