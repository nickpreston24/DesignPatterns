using Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movies.Api.Graphs
{
    public class Actor : Node
    {
        public List<Movie> Films { get; set; } = new List<Movie>();

        public List<Actor> Costars { get => GetCostars(); }

        private List<Actor> GetCostars()
        {
            //this.Relationships.Where(r => r.issatisfied)

            throw new NotImplementedException();
        }

        public string Name { get; set; }

        public override string ToString()
        {
            string films = Films?.Aggregate(new StringBuilder(), (result, next) =>
            {
                result.AppendLine(next.ToString());
                return result;
            }).ToString();

            string costars = Costars?.Aggregate(new StringBuilder(), (result, next) =>
            {
                result.AppendLine(next.ToString());
                return result;
            }).ToString();

            return $"Id: {Id} Name: {Name} {films}";
        }
    }
}