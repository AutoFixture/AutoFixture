﻿namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    public class MyClass
    {
        public T Echo<T>(T item)
        {
            return item;
        }
    }
}