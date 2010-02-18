using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Ploeh.AutoFixture
{
    internal class CustomizedObjectFactory
    {
        private readonly IDictionary<Type, Func<object, object>> typeMappings;
        private readonly int repeatCount;
        private readonly Func<Type, object> resolve;

        internal CustomizedObjectFactory(IDictionary<Type, Func<object, object>> typeMappings, int repeatCount, Func<Type, object> resolvCallback)
        {
            this.typeMappings = typeMappings;
            this.repeatCount = repeatCount;
            this.resolve = resolvCallback;
        }

        internal int RepeatCount
        {
            get { return this.repeatCount; }
        }

        internal T CreateAnonymous<T>(T seed)
        {
            Func<object, object> overridingCreate;
            if (this.typeMappings.TryGetValue(typeof(T), out overridingCreate))
            {
                return (T)overridingCreate(seed);
            }

            return new LatentObjectBuilder<T>(this.typeMappings, this.repeatCount, this.resolve).CreateAnonymous(seed);
        }

        internal object CreateAnonymous(Type t, object seed)
        {
            Func<object, object> overridingCreate;
            if (this.typeMappings.TryGetValue(t, out overridingCreate))
            {
                return overridingCreate(seed);
            }

            Type builderType = typeof(LatentObjectBuilder<>).MakeGenericType(new[] { t });
            IBuilder builder = (IBuilder)Activator.CreateInstance(builderType, this.typeMappings, this.repeatCount, this.resolve);
            return builder.Create(seed);
        }

        internal Accessor CreateAssigment(MemberInfo m)
        {
            return AccessorFactory.Create(m).CreateAssignment(this.CreateNamedObject);
        }

        internal ConstructingObjectBuilder<T> CreateConstructingBuilder<T>(Func<T, T> creator)
        {
            return new ConstructingObjectBuilder<T>(this.typeMappings, this.repeatCount, this.resolve, creator);
        }

        internal LatentObjectBuilder<T> CreateLatentBuilder<T>()
        {
            return new LatentObjectBuilder<T>(this.typeMappings, this.repeatCount, this.resolve);
        }

        internal object CreateNamedObject(Type t, string name)
        {
            object seed = null;
            if (t == typeof(string))
            {
                seed = name;
            }
            return this.CreateAnonymous(t, seed);
        }

        internal object Construct(Type t)
        {
            IEnumerable<ConstructorInfo> constructors = t.GetConstructors().OrderBy(c => c.GetParameters().Length);
            if (!constructors.Any())
            {
                var candidate = this.resolve(t);
                if (candidate != null)
                {
                    return candidate;
                }
                throw new ObjectCreationException(string.Format(CultureInfo.InvariantCulture, "AutoFixture was unable to create an instance of type {0}, since it has no public constructor.", t));
            }
            ConstructorInfo selectedConstructor = constructors.First();

            List<object> parameters = new List<object>();
            foreach (ParameterInfo pi in selectedConstructor.GetParameters())
            {
                object parameter = this.CreateNamedObject(pi.ParameterType, pi.Name);
                parameters.Add(parameter);
            }
            return constructors.First().Invoke(parameters.ToArray());
        }
    }
}
