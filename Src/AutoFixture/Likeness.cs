using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Provides Reflection-based object equality comparison for use when comparing two
    /// semantically equivalent, but different, objects.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use Ploeh.SemanticComparison.Likeness<TSource, TDestination> instead. Anonymous types can be used as sources by using the AsSource extension method (requires import of the Ploeh.SemanticComparison.Fluent namespace).")]
    public class Likeness
    {
        private readonly object likenObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Likeness"/> class with the supplied object
        /// as a blueprint.
        /// </summary>
        /// <param name="value">
        /// The value against which <see cref="Equals"/> will be evaluated.
        /// </param>
        public Likeness(object value)
        {
            this.likenObject = value;
        }

        /// <summary>
        /// Compares the internal "blueprint" value with the supplied object for equality.
        /// </summary>
        /// <param name="obj">
        /// An object to evaluate against the internal "blueprint" object.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> contains <em>all</em> the same
        /// properties as the internal blueprint object, and all values are equal; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (this.likenObject == null && obj == null)
            {
                return true;
            }
            if (this.likenObject == null || obj == null)
            {
                return false;
            }

            var comparisonMembers = Likeness.GetMembersOf(obj);

            var ownMembers = from m in this.GetOwnMembers()
                             let a = AccessorFactory.Create(m).CreateAccessor()
                             where a.CanRead
                             select a;
            foreach (Accessor a in ownMembers)
            {
                if (!this.IsMatch(a, comparisonMembers))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a hash code for the current internal "blueprint" object.
        /// </summary>
        /// <returns>A hash code for the current internal "blueprint" object.</returns>
        public override int GetHashCode()
        {
            return this.likenObject.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the contained object.
        /// </summary>
        /// <returns>A <see cref="string"/> representation of the contained object.</returns>
        public override string ToString()
        {
            return "Likeness of " + this.likenObject.ToString();
        }

        /// <summary>
        /// Gets the value of the contained object.
        /// </summary>
        protected object Value
        {
            get { return this.likenObject; }
        }

        /// <summary>
        /// Gets the members that are used to evaluate the contained object against other objects.
        /// </summary>
        /// <returns>
        /// The members that are used to evaluate the contained object against other objects.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This operation can potentially be resource-intensive.")]
        protected virtual IEnumerable<MemberInfo> GetOwnMembers()
        {
            return this.likenObject.GetType().GetMembers();
        }

        private static Dictionary<string, object> GetMembersOf(object obj)
        {
            return (from m in obj.GetType().GetMembers()
                    group m by m.Name into g
                    where g.Count() == 1
                    let a = AccessorFactory.Create(g.Single()).CreateAccessor()
                    where a.CanRead
                    select new { Value = a.ReadFrom(obj), Name = a.Member.Name }).ToDictionary(x => x.Name, x => x.Value);
        }

        private bool IsMatch(Accessor a, Dictionary<string, object> comparisonMembers)
        {
            object ownValue = a.ReadFrom(this.likenObject);
            object comparisonValue;
            if (comparisonMembers.TryGetValue(a.Member.Name, out comparisonValue))
            {
                if (ownValue != null)
                {
                    if (!ownValue.Equals(comparisonValue))
                    {
                        return false;
                    }
                }
                else
                {
                    if (comparisonValue != null)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Provides Reflection-based object equality comparison for use when comparing two
    /// semantically equivalent, but different, objects.
    /// </summary>
    /// <typeparam name="T">The type of object to compare.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use Ploeh.SemanticComparison.Likeness<TSource, TDestination> instead. Anonymous types can be used as sources by using the AsSource extension method (requires import of the Ploeh.SemanticComparison.Fluent namespace).")]
    public class Likeness<T> : Likeness, IEquatable<T>
    {
        private readonly IEnumerable<MemberInfo> omittedMembers;

        /// <summary>
        /// Initializes a new instance of the <see cref="Likeness{T}"/> class with the supplied
        /// object as a blueprint.
        /// </summary>
        /// <param name="value"></param>
        public Likeness(object value)
            : this(value, Enumerable.Empty<MemberInfo>())
        {
        }

        internal Likeness(object value, IEnumerable<MemberInfo> omittedMembers)
            : base(value)
        {
            this.omittedMembers = omittedMembers;
        }

        /// <summary>
        /// Returns a new <see cref="Likeness{T}"/> that ignores a particular property when
        /// comparing values.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property or field to ignore.</typeparam>
        /// <param name="propertyPicker">
        /// An expression that identifies the property or field to be ignored.
        /// </param>
        /// <returns>
        /// A new <see cref="Likeness{T}"/> that ignores the property identified by
        /// <paramref name="propertyPicker"/> when determining equality.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "The explicit use of Expression<Func<T>> enables type inference from the test code. With only the base class LambdaExpression, calling code would have to explicitly spell out the property type as a generic type parameter. This would hurt readability of the calling code.")]
        public Likeness<T> Without<TProperty>(Expression<Func<T, TProperty>> propertyPicker)
        {
            var me = (MemberExpression)propertyPicker.Body;
            return new Likeness<T>(this.Value, this.omittedMembers.Concat(new[] { me.Member }));
        }

        #region IEquatable<T> Members

        /// <summary>
        /// Compares the internal "blueprint" value with the supplied object for equality.
        /// </summary>
        /// <param name="other">
        /// An object to evaluate against the internal "blueprint" object.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> contains the same properties as
        /// <paramref name="T"/>, and all values are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(T other)
        {
            return this.Equals((object)other);
        }

        #endregion

        /// <summary>
        /// Gets the members that are used to evaluate the contained object against other objects.
        /// </summary>
        /// <returns>
        /// The members that are used to evaluate the contained object against other objects.
        /// </returns>
        protected override IEnumerable<MemberInfo> GetOwnMembers()
        {
            var comparer = new MemberInfoNameComparer();
            return from tm in typeof(T).GetMembers()
                   where !this.omittedMembers.Contains(tm, comparer)
                   let a = AccessorFactory.Create(tm).CreateAccessor()
                   where a.CanRead
                   join vm in this.Value.GetType().GetMembers() on a.Member.Name equals vm.Name into g
                   from xm in g.DefaultIfEmpty(new UnexpectedInfo())
                   select xm;
        }
    }
}
