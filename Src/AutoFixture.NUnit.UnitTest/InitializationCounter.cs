using System.Collections.Generic;

namespace Ploeh.AutoFixture.NUnit.UnitTest
{
    class InitializationCounter
    {
        private readonly string _countKey;
        private static readonly Dictionary<string,int> CountDictionary = new Dictionary<string, int>();

        public InitializationCounter(string countKey)
        {
            _countKey = countKey;
            if (!CountDictionary.ContainsKey(countKey))
            {
                CountDictionary.Add(countKey,0);
            }

            CountDictionary[countKey]++;
        }

        public int TotalCount
        {
            get { return CountDictionary[_countKey]; }
        }
    }
}
