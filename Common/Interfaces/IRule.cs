namespace Shared
{
    public interface IRule
    {
        string PreConditions { get; set; }
        string PostConditions { get; set; }
        string Namespace { get; set; }
        string Assembly { get; set; }
        string Type { get; set; }
    }
}
