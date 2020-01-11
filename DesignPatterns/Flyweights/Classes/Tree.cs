using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Flyweights
{
    //https://refactoring.guru/design-patterns/flyweight
    public class Tree
    {
        public int X { get; set; }
        public int Y { get; set; }
        public TreeType Type { get; set; }
        public void Draw(object canvas) { }
    }

    public class Forest
    {
        public Tree[] Trees;
        public Tree Plant(int x, int y, string name, string color, string texture) => throw new NotImplementedException(MethodBase.GetCurrentMethod().Name);

        public void Draw(object canvas)
        {

        }
    }

    public class TreeFactory
    {
        private ICollection<TreeType> Types = new List<TreeType>();

        private Func<TreeType, string, bool> IsColor = (tree, color) => tree.Color.Equals(color);

        TreeType GetTreeType(string name, string color, string texture)
        {
            var existingType = Types
                .SingleOrDefault(tree => tree.Color.Equals(color) 
                                         && tree.Name.Equals(name) 
                                         && tree.Texture.Equals(texture));
            if (existingType != null)
                return existingType;

            var nextType = new TreeType
            {
                Texture = texture,
                Name = name,
                Color = color
            };

            Types.Add(nextType);

            return null;
        }
    }

    public class TreeType
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string Texture { get; set; }
        public TreeType Tree(string name, string color, string texture) =>
            new TreeType
            {
                Name = name,
                Color = color,
                Texture = texture
            };
    }
}
