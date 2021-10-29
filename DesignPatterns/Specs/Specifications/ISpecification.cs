namespace DesignPatterns.Specifications
{
    /// <summary>
    /// In computer programming, the specification pattern is a particular software design pattern, whereby business rules can be recombined by chaining the business rules together using boolean logic.
    /// It allows us to encapsulate some piece of domain knowledge into a single unit – specification – and reuse it in different parts of the code base.
    /// The pattern is frequently used in the context of domain-driven design.
    /// </summary>
    public partial interface ISpecification<in TSubject>
    {
        bool IsSatisfiedBy(TSubject candidate);
    }
}