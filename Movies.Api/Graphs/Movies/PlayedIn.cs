using Graphs;
using System;
using System.Linq.Expressions;

namespace Movies.Api.Graphs
{
    public sealed class PlayedIn : Relationship<MovieNode>
    {
        private object actor;

        public PlayedIn(Actor actor)
            : base(actor)
            => this.actor = actor;

        public override Expression<Func<MovieNode, bool>> Condition()
            => (film)
            => film != null && actor != null;

        /*actor.Films.Any(f => f.title)*/

        //=> actor.Films.Any(film => film.Title.ToLowerInvariant()
        //.Equals(movie.Title.ToLowerInvariant()));
    }
}