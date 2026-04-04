using System;

namespace AutoFixtureDocumentationTest.Simple;

public class Filter
{
    private int _max;
    private int _min;

    public int Max
    {
        get => _max;
        set
        {
            if (value < Min)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            _max = value;
        }
    }

    public int Min
    {
        get => _min;
        set
        {
            if (value > Max)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            _min = value;
        }
    }
}