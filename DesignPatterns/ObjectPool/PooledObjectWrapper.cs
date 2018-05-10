using System;

namespace DesignPatterns
{
    public class PooledObjectWrapper<T> : PooledObject
    {
        public Action<T> WrapperReleaseResourcesAction { get; set; }
        public Action<T> WrapperResetStateAction { get; set; }
        public T InternalResource { get; private set; }
        public PooledObjectWrapper(T resource)
        {
            InternalResource = resource == null
                ? resource
                : throw new ArgumentException("resource cannot be null");
        }
        protected override void OnReleaseResources()
        {
            WrapperReleaseResourcesAction?.Invoke(InternalResource);
        }
        protected override void OnResetState()
        {
            WrapperResetStateAction?.Invoke(InternalResource);
        }
    }
}
