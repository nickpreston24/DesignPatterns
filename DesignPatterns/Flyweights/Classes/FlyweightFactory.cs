using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Flyweight
{
    public enum DictionaryType
    {
        BinaryTree
    }


    public class FlyweightFactory<TKey, TConcreteFlyweight> : ISingleton
    {
        private IFlyweightCache<TKey, TConcreteFlyweight> _cache;

        [ContractInvariantMethod] private void ContractInvariant() { Contract.Invariant(_cache != null, "The cache cannot be null"); }

        private FlyweightFactory(DictionaryType type) { CreateCache(type); }
        private FlyweightFactory() : this(DictionaryType.BinaryTree) { }


        public TConcreteFlyweight GetFlyweight(TKey key)
        {
            Contract.Requires<ArgumentNullException>(key != null, "Argument cannot be null");
            Contract.Ensures(Contract.Result<TConcreteFlyweight>() != null);
            TConcreteFlyweight flyweight;

            lock (this)
            {
                if (!GetFlyweight(key, out flyweight))
                {
                    Construct(key, out flyweight);
                    _cache.Add(key, flyweight);
                }
            }
            return flyweight;
        }

        public TConcreteFlyweight this[TKey key] { get { return GetFlyweight(key); } }
        public int Count { get { return _cache.Count; } }


        protected virtual void Construct(TKey key, out TConcreteFlyweight flyweight)
        {
            Contract.Requires<ArgumentNullException>(key != null, "Argument key cannot be null");
            Contract.Ensures(flyweight != null);

            var args = new object[1];
            args[0] = key;

            flyweight = CreateHelper<TConcreteFlyweight>.CreateFromPrivateConstructor(args);
        }


        private void CreateCache(DictionaryType type)
        {
            Contract.Ensures(cache != null);
            var factory = new FlyweightCacheFactory<TKey, TConcreteFlyweight>();
            _cache = factory.Create(type);
        }

        private IFlyweightCache<TKey, TConcreteFlyweight> GetCache() { return _cache; }
        private bool GetFlyweight(TKey key, out TConcreteFlyweight flyweight)
        {
            Contract.Ensures(flyweight != null);
            return _cache.TryGetValue(key, out flyweight);
        }

    }
}
