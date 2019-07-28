using Graphs;

namespace Movies.Api.Graphs
{
    public class Actor : Node
    {
        //public Movie[] Films { get; internal set; }

        public string Name { get; set; }

        public override string ToString()
            => $"Name: {Name}";

        //Films: {Films?.Rank.ToString() ?? "None"}
    }
}