using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.AutoRhinoMock
{
    public class DefaultGenericDictionaryBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var t = request as Type;
            if (t == null || !t.IsGenericType)
            {
                return new NoSpecimen(request);
            }

            if (t.GetGenericArguments().Count() == 2)
            {
                var dictionaryType = typeof(Dictionary<,>).MakeGenericType(t.GetGenericArguments());
                if (t.IsAssignableFrom(dictionaryType))
                {
                    return Activator.CreateInstance(dictionaryType, false);
                }
            }

            return new NoSpecimen(request);
        }
    }
}
