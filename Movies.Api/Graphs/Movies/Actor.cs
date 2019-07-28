using Graphs;

namespace Movies.Api.Graphs
{
    public class Actor : Node
    {
        //public Movie[] Films { get; internal set; }
        //Films: {Films?.Rank.ToString() ?? "None"}

        public string Name { get; set; }

        public override string ToString()
            => $"Name: {Name}";

        public Actor WorksWith(Actor actor)
        {
            //var worksWith = new WorksWith(actor).Condition().Compile();
            //Relate(worksWith, actor);
            //Relate(worksWith, null);

            return this;
        }
    }
}