using System;
using System.Collections.Generic;

namespace AutoFixtureDocumentationTest.Simple;

public class MyViewModel
{
    private readonly List<MyClass> _availableItems;
    private MyClass _selectedItem;

    public MyViewModel()
    {
        _availableItems = new List<MyClass>();
    }

    public ICollection<MyClass> AvailableItems
    {
        get { return _availableItems; }
    }

    public MyClass SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (!_availableItems.Contains(value))
            {
                throw new ArgumentException("...");
            }
            _selectedItem = value;
        }
    }
}