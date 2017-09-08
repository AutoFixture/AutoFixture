using System;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class DisposableSpy : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Disposed = true;
            }
        }
    }
}
