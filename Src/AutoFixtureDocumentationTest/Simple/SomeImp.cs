using System;

namespace AutoFixtureDocumentationTest.Simple;

public class SomeImp : IBadDesign
{
    private MyClass _mc;
    private string _message;

    public string Message
    {
        get => _message;
        set
        {
            if (_mc == null)
            {
                throw new InvalidOperationException("...");
            }

            _message = value;
            TransformedMessage = _mc.DoStuff(value);
        }
    }

    public void Initialize(MyClass mc)
    {
        _mc = mc;
    }

    public string TransformedMessage { get; private set; }
}