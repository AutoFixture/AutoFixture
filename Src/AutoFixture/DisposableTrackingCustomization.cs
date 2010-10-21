using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Encapsulates a customization that adds tracking of disposable specimens to an
    /// <see cref="IFixture"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Customize an <see cref="IFixture"/> to enable tracking of disposable specimens. Invoke
    /// <see cref="Dispose()"/> on the instance to dispose of all tracked instances.
    /// </para>
    /// </remarks>
    /// <seealso cref="DisposableTrackingBehavior"/>
    public class DisposableTrackingCustomization : ICustomization, IDisposable
    {
        private readonly DisposableTrackingBehavior behavior;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableTrackingCustomization"/> class.
        /// </summary>
        public DisposableTrackingCustomization()
        {
            this.behavior = new DisposableTrackingBehavior();
        }

        /// <summary>
        /// Gets the behavior that this customization adds to <see cref="IFixture"/> instances.
        /// </summary>
        /// <seealso cref="DisposableTrackingBehavior"/>
        public DisposableTrackingBehavior Behavior
        {
            get { return this.behavior; }
        }

        #region ICustomization Members

        /// <summary>
        /// Customizes the specified fixture by applying <see cref="Behavior"/>.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            fixture.Behaviors.Add(this.Behavior);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes <see cref="Behavior"/>.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// Disposes <see cref="Behavior"/>.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.behavior.Dispose();
            }
        }
    }
}
