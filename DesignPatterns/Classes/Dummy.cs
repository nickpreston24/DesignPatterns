using System;

namespace DesignPatterns.Classes
{
    class Dummy
    {

        private bool _disposed;
        ~Dummy()
        {
            Dispose(false);
        }

        /// <summary>
        /// References:
        /// https://lostechies.com/chrispatterson/2012/11/29/idisposable-done-right/
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                //free managed objects that implement IDisposable only
            }
            //release any unmanaged objects here and set object references to null

            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
