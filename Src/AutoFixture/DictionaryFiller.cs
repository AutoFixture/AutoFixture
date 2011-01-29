using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using System.Collections;

namespace Ploeh.AutoFixture
{
    public static class DictionaryFiller
    {
        public static void AddMany(object specimen, ISpecimenContext context)
        {
            if (specimen == null)
            {
                throw new ArgumentNullException("specimen");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var typeArguments = specimen.GetType().GetGenericArguments();
            if (typeArguments.Length != 2)
            {
                throw new ArgumentException("The specimen must be an instance of IDictionary<TKey, TValue>.", "specimen");
            }

            if (!typeof(IDictionary<,>).MakeGenericType(typeArguments).IsAssignableFrom(specimen.GetType()))
            {
                throw new ArgumentException("The specimen must be an instance of IDictionary<TKey, TValue>.", "specimen");
            }

            var kvpType = typeof(KeyValuePair<,>).MakeGenericType(typeArguments);
            var enumerable = context.Resolve(new MultipleRequest(kvpType)) as IEnumerable;
            foreach (var item in enumerable)
            {
                var addMethod = typeof(ICollection<>).MakeGenericType(kvpType).GetMethod("Add", new[] { kvpType });
                addMethod.Invoke(specimen, new[] { item });
            }
        }
    }
}
