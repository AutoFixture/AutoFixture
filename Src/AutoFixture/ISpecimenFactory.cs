using System;
using System.Collections.Generic;

namespace Ploeh.AutoFixture
{
    public interface ISpecimenFactory<T>
    {
        T CreateAnonymous(T seed);

        T CreateAnonymous();

        IEnumerable<T> CreateMany(int repeatCount);

        IEnumerable<T> CreateMany(T seed, int repeatCount);

        IEnumerable<T> CreateMany();

        IEnumerable<T> CreateMany(T seed);
    }
}
