using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Creates a new specimen using a <see cref="Func{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of specimen to create.</typeparam>
    public class SpecimenFactory<T> : ISpecimenBuilder
    {
        private readonly Func<T> create;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecimenFactory&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="factory">The func that will create specimens.</param>
        public SpecimenFactory(Func<T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.create = factory;
        }

        public Func<T> Factory
        {
            get { return this.create; }
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// The specimen created by the Func contained by this instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="request"/> is ignored. Instead, the Func contained by this instance is
        /// used to create a specimen.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContainer container)
        {
            return this.create();
        }

        #endregion
    }

    /// <summary>
    /// Creates a new specimen using a <see cref="Func{TInput, T}"/>.
    /// </summary>
    /// <typeparam name="TInput">The type of the parameter provided to the Func.</typeparam>
    /// <typeparam name="T">The type of specimen to create.</typeparam>
    public class SpecimenFactory<TInput, T> : ISpecimenBuilder
    {
        private readonly Func<TInput, T> create;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecimenFactory&lt;TInput, T&gt;"/> class.
        /// </summary>
        /// <param name="factory">The func that will create specimens.</param>
        /// <remarks>
        /// <para>
        /// The input parameter to <paramref name="factory"/> will be supplied by the
        /// <see cref="ISpecimenContainer"/> passed to the <see cref="Create"/> method.
        /// </para>
        /// </remarks>
        public SpecimenFactory(Func<TInput, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.create = factory;
        }

        public Func<TInput, T> Factory
        {
            get { return this.create; }
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// The specimen created by the Func contained by this instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="request"/> is ignored. Instead, the Func contained by this instance is
        /// used to create a specimen. The parameter for the Func is supplied by
        /// <paramref name="container"/>.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            var p = (TInput)container.Resolve(typeof(TInput));
            return this.create(p);
        }

        #endregion
    }

    /// <summary>
    /// Creates a new specimen using a <see cref="Func{TInput1, TInput2, T}"/>.
    /// </summary>
    /// <typeparam name="TInput1">The type of the first parameter provided to the Func.</typeparam>
    /// <typeparam name="TInput2">The type of the second parameter provided to the Func.</typeparam>
    /// <typeparam name="T">The type of specimen to create.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Necessary to wrap the corresponding Func. However, this particular API is only intended to be used to implement the fluent API, and is not targeted at the typical end-user.")]
    public class SpecimenFactory<TInput1, TInput2, T> : ISpecimenBuilder
    {
        private readonly Func<TInput1, TInput2, T> create;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SpecimenFactory&lt;TInput1, TInput2, T&gt;"/> class.
        /// </summary>
        /// <param name="factory">The func that will create specimens.</param>
        /// <remarks>
        /// <para>
        /// The input parameters to <paramref name="factory"/> will be supplied by the
        /// <see cref="ISpecimenContainer"/> passed to the <see cref="Create"/> method.
        /// </para>
        /// </remarks>
        public SpecimenFactory(Func<TInput1, TInput2, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.create = factory;
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// The specimen created by the Func contained by this instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="request"/> is ignored. Instead, the Func contained by this instance is
        /// used to create a specimen. The parameters for the Func is supplied by
        /// <paramref name="container"/>.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            var p1 = (TInput1)container.Resolve(typeof(TInput1));
            var p2 = (TInput2)container.Resolve(typeof(TInput2));
            return this.create(p1, p2);
        }

        #endregion
    }

    /// <summary>
    /// Creates a new specimen using a <see cref="Func{TInput1, TInput2, TInput3, T}"/>.
    /// </summary>
    /// <typeparam name="TInput1">The type of the first parameter provided to the Func.</typeparam>
    /// <typeparam name="TInput2">The type of the second parameter provided to the Func.</typeparam>
    /// <typeparam name="TInput3">The type of the third parameter provided to the Func.</typeparam>
    /// <typeparam name="T">The type of specimen to create.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Necessary to wrap the corresponding Func. However, this particular API is only intended to be used to implement the fluent API, and is not targeted at the typical end-user.")]
    public class SpecimenFactory<TInput1, TInput2, TInput3, T> : ISpecimenBuilder
    {
        private readonly Func<TInput1, TInput2, TInput3, T> create;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SpecimenFactory&lt;TInput1, TInput2, TInput3, T&gt;"/> class.
        /// </summary>
        /// <param name="factory">The func that will create specimens.</param>
        /// <remarks>
        /// <para>
        /// The input parameters to <paramref name="factory"/> will be supplied by the
        /// <see cref="ISpecimenContainer"/> passed to the <see cref="Create"/> method.
        /// </para>
        /// </remarks>
        public SpecimenFactory(Func<TInput1, TInput2, TInput3, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.create = factory;
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// The specimen created by the Func contained by this instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="request"/> is ignored. Instead, the Func contained by this instance is
        /// used to create a specimen. The parameters for the Func is supplied by
        /// <paramref name="container"/>.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            var p1 = (TInput1)container.Resolve(typeof(TInput1));
            var p2 = (TInput2)container.Resolve(typeof(TInput2));
            var p3 = (TInput3)container.Resolve(typeof(TInput3));
            return this.create(p1, p2, p3);
        }

        #endregion
    }

    /// <summary>
    /// Creates a new specimen using a <see cref="Func{TInput1, TInput2, TInput3, TInput4, T}"/>.
    /// </summary>
    /// <typeparam name="TInput1">The type of the first parameter provided to the Func.</typeparam>
    /// <typeparam name="TInput2">The type of the second parameter provided to the Func.</typeparam>
    /// <typeparam name="TInput3">The type of the third parameter provided to the Func.</typeparam>
    /// <typeparam name="TInput4">The type of the fourth parameter provided to the Func.</typeparam>
    /// <typeparam name="T">The type of specimen to create.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = "Necessary to wrap the corresponding Func. However, this particular API is only intended to be used to implement the fluent API, and is not targeted at the typical end-user.")]
    public class SpecimenFactory<TInput1, TInput2, TInput3, TInput4, T> : ISpecimenBuilder
    {
        private readonly Func<TInput1, TInput2, TInput3, TInput4, T> create;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SpecimenFactory&lt;TInput1, TInput2, TInput3, TInput4, T&gt;"/> class.
        /// </summary>
        /// <param name="factory">The func that will create specimens.</param>
        /// <remarks>
        /// <para>
        /// The input parameters to <paramref name="factory"/> will be supplied by the
        /// <see cref="ISpecimenContainer"/> passed to the <see cref="Create"/> method.
        /// </para>
        /// </remarks>
        public SpecimenFactory(Func<TInput1, TInput2, TInput3, TInput4, T> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.create = factory;
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="container">A container that can be used to create other specimens.</param>
        /// <returns>
        /// The specimen created by the Func contained by this instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <paramref name="request"/> is ignored. Instead, the Func contained by this instance is
        /// used to create a specimen. The parameters for the Func is supplied by
        /// <paramref name="container"/>.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            var p1 = (TInput1)container.Resolve(typeof(TInput1));
            var p2 = (TInput2)container.Resolve(typeof(TInput2));
            var p3 = (TInput3)container.Resolve(typeof(TInput3));
            var p4 = (TInput4)container.Resolve(typeof(TInput4));
            return this.create(p1, p2, p3, p4);
        }

        #endregion
    }

}
