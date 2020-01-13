using System.Diagnostics;

/// <summary>
/// An example of the logical class that shall be generated as a logical link between benefactors (multiple inherited classes),
/// their children (base classes) and grandchildren (derived classes).
/// Given benefactor classes A and B, generate a class that C will inherit from P, 
/// P being the arbiter of that code logic by implicitly (unknowingly) passing on the methods from A & B to C.
/// All P requires is an IBenefactor interface tag.  
/// The heavy lifting is done by Reflection and extension methods that generate and pass on the functionality from A,B ==> C 'through' P.
/// C still iherits from P as well.
/// </summary>
public abstract class BenefactorLogic : IIntrafactor
{
    BenefactorA A;
    BenefactorB B;

    IIntrafactor Intrafactor => this;

    protected BenefactorLogic()
    {
        A = new BenefactorA();
        B = new BenefactorB();
    }

    //todo: try to do this dynamically in your Inheritance Extensions.Benefit() method:
    public string Bar() => Intrafactor.Bar();

    public string Foo() => Intrafactor.Foo();

    string IIntrafactor.Bar() => A.Foo();

    string IIntrafactor.Foo() => B.Bar();

}

public class Parent
{
    public void OtherMethods() => Debug.WriteLine("I do other stuff!");
}

public class Child //: BenefactorLogic, IParent
{
    public void RunInheritedLogic()
    {
        Debug.WriteLine("...");
    }
}

//public interface IInheritor
//{
//    //does nothing but allow extensions to work.
//}

public class BenefactorA
{
    public virtual string Foo() => "Foo()";
}

public class BenefactorB
{
    public virtual string Bar() => "Bar()";
}

//Holds common benefactor behaviors (optional and not entirely necessary).
//sits as a contract between A, B and the abstract base class
public interface IIntrafactor
{
    string Foo();
    string Bar();
}

public static class InheritanceExtensions
{
    //Inject T's base class (Benefactor) with explicitly implemented methods, each paired with an exposed (public) method of the same name in their base class.
    public static Child Benefit<Child, Parent, Benefactor>(this Child child)// where Child : Benefactor
        =>
            //keep a dictionary of types and their invokable methods (explicit and implicit methods will have the same name)
            //T inherits B's methods, where B is a benefactor of T and T is a Child class derived from a Parent.
            child; //the modified child who got Benefactor's powers.
}
