using Graphs;
using System;
using System.Linq.Expressions;

namespace Movies.Api.Graphs
{
    //Dummy class for working relationships
    internal class WorksWith : Relationship<Actor>
    {
        private Actor costar;

        public WorksWith(Actor costar) => this.costar = costar;

        public override Expression<Func<Actor, Actor, bool>> ToExpression()
            => (actor, other)
            => actor != null && costar != null;

        //actor.Films.Where(playedIn);
        //TODO: actor.PlayedIn(movie).With(other);

        //=> actor.MovieNode.Id == other.MovieNode.Id
        //|| actor.Movie.Title.ToLowerInvariant()
        //        .Equals(other.Movie.Title.ToLowerInvariant());
    }
}