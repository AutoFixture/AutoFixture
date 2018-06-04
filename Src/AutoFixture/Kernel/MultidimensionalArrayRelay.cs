using System;
using System.Linq;

namespace AutoFixture.Kernel
{
    /// <summary>
    /// Relays a request for a multidimensional array to a jagged array and converts the result
    /// to the desired multidimensional array type.
    /// </summary>
    public class MultidimensionalArrayRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new multidimensional array based on a request.
        /// </summary>
        /// <param name="request">
        /// The request that describes what to create.
        /// </param>
        /// <param name="context">
        /// A context that can be used to create other specimens.
        /// </param>
        /// <returns>
        /// A multidimensional array of the requested type if possible; otherwise a
        /// <see cref="NoSpecimen" /> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var arrayType = request as Type;
            if (arrayType == null || !IsMultidimensionalArray(arrayType))
                return new NoSpecimen();

            return CreateMultidimensionalArray(arrayType, context);
        }

        private static bool IsMultidimensionalArray(Type type)
        {
            return type.IsArray && type.GetArrayRank() > 1;
        }

        private static object CreateMultidimensionalArray(Type arrayType, ISpecimenContext context)
        {
            Type elementType = arrayType.GetElementType();
            int dimension = arrayType.GetArrayRank();

            Type jaggedArrayType = MakeJaggedArrayType(elementType, dimension);
            var jagged = (Array)context.Resolve(jaggedArrayType);

            int length = GetInnerLength(jagged, dimension);
            int[] lengths = Enumerable.Repeat(length, dimension).ToArray();

            Array array = Array.CreateInstance(elementType, lengths);

            Copy(array, dimension, jagged);

            return array;
        }

        private static Type MakeJaggedArrayType(Type elementType, int dimension)
        {
            Type jaggedArrayType = elementType;
            for (int i = 0; i < dimension; i++)
                jaggedArrayType = jaggedArrayType.MakeArrayType();

            return jaggedArrayType;
        }

        private static int GetInnerLength(Array jagged, int dimension)
        {
            Array current = jagged;
            for (int i = 0; i < dimension - 1; i++)
                current = (Array)current.GetValue(0);

            return current.Length;
        }

        private static void Copy(Array array, int dimension, Array jagged, params int[] indices)
        {
            if (dimension > 1)
                CopyNextDimension(array, dimension, jagged, indices);
            else
                CopyLastDimension(array, jagged, indices);
        }

        private static void CopyNextDimension(Array array, int dimension, Array jagged, int[] indices)
        {
            for (int i = 0; i < jagged.Length; i++)
                Copy(array, dimension - 1, (Array)jagged.GetValue(i), indices.Concat(new[] { i }).ToArray());
        }

        private static void CopyLastDimension(Array array, Array jagged, int[] indices)
        {
            for (int i = 0; i < jagged.Length; i++)
                array.SetValue(jagged.GetValue(i), indices.Concat(new[] { i }).ToArray());
        }
    }
}