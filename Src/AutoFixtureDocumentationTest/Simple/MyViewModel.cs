﻿using System;
using System.Collections.Generic;

namespace AutoFixtureDocumentationTest.Simple
{
    public class MyViewModel
    {
        private readonly List<MyClass> availableItems;
        private MyClass selectedItem;

        public MyViewModel()
        {
            this.availableItems = new List<MyClass>();
        }

        public ICollection<MyClass> AvailableItems
        {
            get { return this.availableItems; }
        }

        public MyClass SelectedItem
        {
            get => this.selectedItem;
            set
            {
                if (!this.availableItems.Contains(value))
                {
                    throw new ArgumentException("...");
                }
                this.selectedItem = value;
            }
        }
    }
}
