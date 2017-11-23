using System.Collections.Generic;

namespace Fare
{
    internal static class LinkedListExtensions
    {
        internal static T RemoveAndReturnFirst<T>(this LinkedList<T> linkedList)
        {
            T first = linkedList.First.Value;
            linkedList.RemoveFirst();
            return first;
        }
    }
}