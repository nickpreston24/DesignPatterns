using System.Linq;

namespace Movies.Data
{
    internal class SessionFactory
    {
        internal static ISession OpenSession()
        {
            return new Session();
        }
    }

    internal class Session : ISession
    {
        public void Dispose()
        {
            //Todo: dispose of connection here
        }

        public IQueryable<T> Query<T>() => Enumerable.Empty<T>().AsQueryable();


    }
}