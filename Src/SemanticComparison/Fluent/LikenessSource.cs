namespace Ploeh.SemanticComparison.Fluent
{
    /// <summary>
    /// Defines the source-side of a <see cref="Likeness{TSource, TDestination}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source value.</typeparam>
    public class LikenessSource<TSource>
    {
        private readonly TSource value;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikenessSource{TSource}"/> class with the supplied
        /// value.
        /// </summary>
        /// <param name="value">The source value.</param>
        public LikenessSource(TSource value)
        {
            this.value = value;
        }

        /// <summary>
        /// Creates a <see cref="Likeness{TSource, TDestination}"/> instance.
        /// </summary>
        /// <typeparam name="TDestination">The data type of the destination.</typeparam>
        /// <returns>
        /// A new instance of <see cref="Likeness{TSource, TDestination}"/> that contains the
        /// source value defined in the constructor.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public Likeness<TSource, TDestination> OfLikeness<TDestination>()
        {
            return new Likeness<TSource, TDestination>(this.value);
        }
    }
}
