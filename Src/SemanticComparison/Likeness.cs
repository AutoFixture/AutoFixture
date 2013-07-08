using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ploeh.SemanticComparison
{
    /// <summary>
    /// Provides convention-based object equality comparison for use when comparing two
    /// semantically equivalent, but different, objects.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the source value (against which the destination value will be compared for
    /// equality).
    /// </typeparam>
    /// <typeparam name="TDestination">
    /// The type of the destination value which will be compared for equality against the source
    /// value.
    /// </typeparam>
    public class Likeness<TSource, TDestination> : IEquatable<TDestination>
    {
        private readonly TSource value;
        private readonly SemanticComparer<TSource, TDestination> comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Likeness{TSource, TDestination}"/> class
        /// with the supplied source value.
        /// </summary>
        /// <param name="value">
        /// The source value against which destination values will be compared when
        /// <see cref="Equals(TDestination)"/> is invoked.</param>
        public Likeness(TSource value)
            : this(value, Enumerable.Empty<MemberEvaluator<TSource, TDestination>>(), SemanticComparer<TSource, TDestination>.DefaultMembers.Generate<TDestination>)
        {
        }

        internal Likeness(TSource value, IEnumerable<MemberEvaluator<TSource, TDestination>> evaluators, Func<IEnumerable<MemberInfo>> defaultMembersGenerator)
        {
            this.value = value;
            this.comparer = new SemanticComparer<TSource, TDestination>(evaluators, defaultMembersGenerator);
        }

        /// <summary>
        /// Gets the source value against which destination values will be compared when
        /// <see cref="Equals(TDestination)"/> is invoked.
        /// </summary>
        public TSource Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Creates a dynamic proxy that overrides Equals using Likeness. 
        /// This method uses the same semantic heuristics, as the default semantic comparison, to map
        /// values from the source constructor parameters to the destination constructor.
        /// </summary>
        public TDestination CreateProxy()
        {
            try
            {
                return ProxyGenerator.CreateLikenessProxy<TSource, TDestination>(
                    this.value,
                    this.comparer,
                    SemanticComparer<TSource, TDestination>.DefaultMembers.Generate<TDestination>());
            }
            catch (TypeLoadException e)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    "The proxy of {0} could not be created. Access is denied on type, or the base type is sealed. Please see inner exception for more details.",
                    typeof(TDestination));
                throw new ProxyCreationException(message, e);
            }
            catch (InvalidOperationException e)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    "The proxy of {0} could not be created using the same semantic heuristics as the default semantic comparison. In order to create proxies of types with non-parameterless constructor the values from the source constructor must be compatible to the parameters of the destination constructor.",
                    typeof(TDestination));
                throw new ProxyCreationException(message, e);
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is semantically equal to the
        /// current <see cref="Value"/>.
        /// </summary>
        /// <param name="obj">The object to compare against <see cref="Value"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is semantically equal to
        /// <see cref="Value"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if ((this.Value == null) && (obj == null))
            {
                return true;
            }

            if (obj is TDestination)
            {
                return this.Equals((TDestination)obj);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for <see cref="Likeness{TSource, TDestination}"/>.
        /// </summary>
        /// <returns>
        /// The hash code for <see cref="Value"/>, or 0 if the value is <see langword="null"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Value == null ? 0 : this.Value.GetHashCode();
        }

        /// <summary>
        /// Turns off implicit default comparison of properties and fields.
        /// </summary>
        /// <returns>
        /// A new <see cref="Likeness{TSource, TDestinaion}"/> that uses only explicitly defined
        /// comparisons of properties and fields.
        /// </returns>
        public Likeness<TSource, TDestination> OmitAutoComparison()
        {
            return new Likeness<TSource, TDestination>(this.Value, this.comparer.Evaluators, Enumerable.Empty<MemberInfo>);
        }

        /// <summary>
        /// Verifies that a value matches the encapsulated value, or throws a descriptive exception
        /// if this is not the case.
        /// </summary>
        /// <param name="other">The value to compare against <see cref="Value"/>.</param>
        /// <exception cref="LikenessException">
        /// <paramref name="other"/> does not match <see cref="Value"/>.
        /// </exception>
        public void ShouldEqual(TDestination other)
        {
            if ((this.Value == null) && (other == null))
            {
                return;
            }
            if (other == null)
            {
                throw new LikenessException("The provided value was null, but an instance was expected.");
            }

            var mismatches = (from me in this.comparer.GetEvaluators()
                              where !me.Evaluator(this.Value, other)
                              select me).ToList();
            if (mismatches.Any())
            {
                var message = this.CreateMismatchMessage(other, mismatches);
                throw new LikenessException(message);
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the contained object.
        /// </summary>
        /// <returns>A <see cref="string"/> representation of the contained object.</returns>
        public override string ToString()
        {
            var valueText = this.Value == null ? "null" : this.Value.ToString();
            return string.Format(CultureInfo.CurrentCulture, "Likeness of {0}", valueText);
        }

        /// <summary>
        /// Returns a <see cref="LikenessMember{TSource, TDestination}"/> that can be used to
        /// define custom comparison behavior for a particular property or field.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property or field.</typeparam>
        /// <param name="propertyPicker">
        /// An expresssion that identifies the property or field.
        /// </param>
        /// <returns>
        /// A new instance of <see cref="LikenessMember{TSource, TDestination}"/> that represents
        /// the property or field identified by <paramref name="propertyPicker"/>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "The explicit use of Expression<Func<T>> enables type inference from the test code. With only the base class LambdaExpression, calling code would have to explicitly spell out the property type as a generic type parameter. This would hurt readability of the calling code.")]
        public LikenessMember<TSource, TDestination> With<TProperty>(Expression<Func<TDestination, TProperty>> propertyPicker)
        {
            if (propertyPicker == null)
            {
                throw new ArgumentNullException("propertyPicker");
            }

            var me = (MemberExpression)propertyPicker.Body;
            return new LikenessMember<TSource, TDestination>(this, me.Member);
        }

        /// <summary>
        /// Opt-in of default equality comparison for a specific property or field. Only relevant
        /// if <see cref="OmitAutoComparison"/> was previously called.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property or field.</typeparam>
        /// <param name="propertyPicker">
        /// An expresssion that identifies the property or field.
        /// </param>
        /// <returns>
        /// A new instance of <see cref="Likeness{TSource, TDestination}"/> that explicitly
        /// includes the member identified by <paramref name="propertyPicker"/> and uses the
        /// default comparison.
        /// </returns>
        public Likeness<TSource, TDestination> WithDefaultEquality<TProperty>(Expression<Func<TDestination, TProperty>> propertyPicker)
        {
            if (propertyPicker == null)
            {
                throw new ArgumentNullException("propertyPicker");
            }

            var me = (MemberExpression)propertyPicker.Body;
            var f = me.Member.ToEvaluator<TSource, TDestination>();
            return this.With(propertyPicker).EqualsWhen(f.Evaluator);
        }

        /// <summary>
        /// Returns a new <see cref="Likeness{TSource, TDestination}"/> that ignores a particular
        /// property when comparing values.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property or field to ignore.</typeparam>
        /// <param name="propertyPicker">
        /// An expression that identifies the property or field to be ignored.
        /// </param>
        /// <returns>
        /// A new <see cref="Likeness{TSource, TDestination}"/> that ignores the property
        /// identified by <paramref name="propertyPicker"/> when determining equality.
        /// </returns>
        public Likeness<TSource, TDestination> Without<TProperty>(Expression<Func<TDestination, TProperty>> propertyPicker)
        {
            return this.With(propertyPicker).EqualsWhen((s, d) => true);
        }

        /// <summary>
        /// Determines whether the specified object is semantically equal to the current
        /// <see cref="Value"/>.
        /// </summary>
        /// <param name="other">The object to compare against <see cref="Value"/>.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> is semantically equal to
        /// <see cref="Value"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(TDestination other)
        {
            if ((this.Value == null) && (other == null))
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }

            return this.comparer.Equals(this.Value, other);
        }

        internal Likeness<TSource, TDestination> AddEvaluator(MemberEvaluator<TSource, TDestination> evaluator)
        {
            return new Likeness<TSource, TDestination>(this.Value, this.comparer.Evaluators.Concat(new[] { evaluator }), this.comparer.DefaultMembersGenerator);
        }

        private string CreateMismatchMessage(TDestination other, IEnumerable<MemberEvaluator<TSource, TDestination>> mismatches)
        {
            return string.Concat(
                new[]
                {
                    string.Format(CultureInfo.CurrentCulture, "The provided value {0} did not match the expected value {1}. The following members did not match:{2}", this.Value, other, Environment.NewLine)
                }
                .Concat(mismatches.Select(me => string.Format(CultureInfo.CurrentCulture, "- {0}.{1}", me.Member.Name, Environment.NewLine)))
                .ToArray());
        }
    }

    /// <summary>
    /// Provides convention-based object equality comparison for use when 
    /// comparing two semantically equivalent objects.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value which will be compared for equality.
    /// </typeparam>
    public class Likeness<T> : IEquatable<T>
    {
        private readonly T value;
        private readonly IEqualityComparer<T> comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Likeness&lt;T&gt;"/> 
        /// class with the supplied value.
        /// </summary>
        /// <param name="value">The value which will be compared for equality.
        /// </param>
        public Likeness(T value)
            : this(value, new SemanticComparer<T>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Likeness&lt;T&gt;"/> 
        /// class with the supplied value.
        /// </summary>
        /// <param name="value">The value which will be compared for equality.
        /// </param>
        /// <param name="comparer">
        /// The comparer to support the comparison of objects for equality.
        /// </param>
        public Likeness(T value, IEqualityComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            this.value = value;
            this.comparer = comparer;
        }

        /// <summary>
        /// Gets the supplied value which will be compared for equality.
        /// </summary>
        /// <value>
        /// The supplied value which will be compared for equality.
        /// </value>
        public T Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is semantically
        /// equal to the current <see cref="Value"/>.
        /// </summary>
        /// <param name="obj">
        /// The object to compare against <see cref="Value"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is semantically
        /// equal to <see cref="Value"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return (this.Value == null && obj == null)
                ? true
                : (obj is T
                    ? this.Equals((T)obj)
                    : base.Equals(obj));
        }

        /// <summary>
        /// Serves as a hash function for <see cref="Likeness&lt;T&gt;"/>.
        /// </summary>
        /// <returns>
        /// The hash code for <see cref="Value"/>, or 0 if the value is 
        /// <see langword="null"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Value == null 
                ? 0 
                : this.Value.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the contained object.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> representation of the contained object.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.CurrentCulture, 
                "Likeness of {0}", 
                this.Value == null ? "null" : this.Value.ToString());
        }

        /// <summary>
        /// Determines whether the specified object is semantically equal to 
        /// the current <see cref="Value"/>.
        /// </summary>
        /// <param name="other">
        /// The object to compare against <see cref="Value"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> is semantically
        /// equal to <see cref="Value"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(T other)
        {
            if (this.Value == null && other == null)
                return true;

            if (other == null)
                return false;

            return this.comparer.Equals(this.Value, other);
        }

        /// <summary>
        /// Turns the <see cref="Likeness&lt;T&gt;"/> into a Resemblance by 
        /// dynamically emitting a derived class that overrides Equals in the 
        /// way that the <see cref="Likeness&lt;T&gt;"/> (re)defines equality.
        /// </summary>
        /// <returns>
        /// A dynamically emitted derived class that overrides Equals in the 
        /// way that the <see cref="Likeness&lt;T&gt;"/> (re)defines equality.
        /// </returns>
        /// <exception cref="ProxyCreationException"></exception>
        public T ToResemblance()
        {
            try
            {
                return ProxyGenerator.CreateLikenessResemblance<T>(this);
            }
            catch (TypeLoadException e)
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    "The resemblance of {0} could not be created. Access is denied on type, or the base type is sealed. Please see inner exception for more details.",
                    typeof(T));
                throw new ProxyCreationException(message, e);
            }
        }
    }
}