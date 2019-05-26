using System;
using System.Threading;

namespace DesignPatterns.WebSamples
{
    /// <summary>
    /// PooledObject base calss
    /// </summary>
    public abstract class PooledObject : IDisposable
    {
        /// <summary>
        /// Internal Action that is initialized by the pool while creating the object, this allow that object to re-add itself back to the pool.
        /// </summary>
        internal Action<PooledObject, bool> ReturnToPool { get; set; }
        /// <summary>
        /// Internal flag that is being managed by the pool to describe the object state - primary used to void cases where the resources are being releases twice.
        /// </summary>
        internal bool Disposed { get; set; }

        /// <summary>
        /// Releases the object resources
        /// This method will be called by the pool manager when there is no need for this object anymore (decreasing pooled objects count, pool is being destroyed)
        /// </summary>
        /// <returns></returns>
        internal bool ReleaseResources()
        {
            bool successFlag = true;
            try
            {
                OnReleaseResources();
            }
            catch (Exception)
            {
                successFlag = false;
            }
            return successFlag;
        }
        /// <summary>
        /// Reset the object state
        /// This method will be called by the pool manager just before the object is being returned to the pool
        /// </summary>
        /// <returns></returns>
        internal bool ResetState()
        {
            bool successFlag = true;
            try
            {
                OnResetState();
            }
            catch (Exception)
            {
                successFlag = false;
            }
            return successFlag;
        }

        /// <summary>
        /// Reset the object state to allow this object to be re-used by other parts of the application.
        /// </summary>
        protected virtual void OnResetState()
        {
        }
        /// <summary>
        /// Releases the object's resources
        /// </summary>
        protected virtual void OnReleaseResources()
        {
        }

        private void HandleReAddingToPool(bool reRegisterForFinalization)
        {
            if (!Disposed)
            {
                // If there is any case that the re-adding to the pool failes, release the resources and set the internal Disposed flag to true
                try
                {
                    // Notifying the pool that this object is ready for re-adding to the pool.
                    ReturnToPool(this, reRegisterForFinalization);
                }
                catch (Exception)
                {
                    Disposed = true;
                    ReleaseResources();
                }
            }
        }
        ~PooledObject()
        {
            // Resurrecting the object
            HandleReAddingToPool(true);
        }
        public void Dispose()
        {
            // Returning to pool
            ThreadPool.QueueUserWorkItem(new WaitCallback((o) => HandleReAddingToPool(false)));
        }
    }
}
