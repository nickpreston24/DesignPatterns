namespace DesignPatterns.Flyweight
{
    public class Tree
    {
        public int X { get; set; }
        public int Y { get; set; }
        public TreeType Type { get; set; }
    }
    public enum TreeType
    {
        Shady,
        Tall,
        Short,
        Fruit,
        Nut
    }
}
