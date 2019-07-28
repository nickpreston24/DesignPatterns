using Graphs;
using System;
using System.Linq.Expressions;

namespace Movies.Api.Graphs
{
    internal sealed class PlayedIn : Relationship<Actor, MovieNode>
    {
        public override Expression<Func<Actor, MovieNode, bool>> ToExpression()
            => (actor, movie)
            => actor != null

            /*actor.Films.Any(f => f.title)*/;

        //=> actor.Films.Any(film => film.Title.ToLowerInvariant()
        //.Equals(movie.Title.ToLowerInvariant()));
    }
}