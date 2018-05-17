using System;
using System.Collections;
using System.Diagnostics;

namespace DesignPatterns
{
    /// <summary>
    /// @Me: Yes, I, Michael Preston wrote this code.
    /// Don't let the #region confuse.
    /// I'm trying to create a 'Minion' pattern combining Adapter + Facade + Observer and maybe Singleton.
    /// Steps:
    ///     1. Set up 3 or more kingdoms as the targets: ex: Android, Windows, Apple
    ///     2. For each of these targets, provide an adapter that 'updates' them, etc.
    ///     3. Place a facade method or class over the call to the 'for' loop that runs through all 3 and updates them.
    ///     4. Register each Adapter as observables (or 'minion' adapters, hence the name) with the 'overlord' (Observer) class receiving messages.
    ///     5. Make generic as possible, but don't overdo it - the observer pattern can be tricky to implement correctly with threading.
    ///     6. (Optional) Overlord class can be singleton.
    ///     
    /// Ultimately, I want:
    /// A) a generic example, not T-generic, which adequately shows how these GOF patterns can marry and become something far more powerful.
    /// B) This entire pattern to be a snippet for code reusability.
    /// 
    /// </summary>

    #region Adapter impl

    // Target 
    class Target
    {
        public virtual void TargetRequest()
        {
            Debug.WriteLine("base request");
        }
    }

    // Adapter 
    class Adapter : Target
    {
        private Adaptee adaptee = new Adaptee();
        public override void TargetRequest()
        {
            // Possibly do some other work
            //   and then call AdapteeRequest

            Debug.WriteLine("doing other work...");
            adaptee.AdapteeRequest();
        }
    }

    // Adaptee
    class Adaptee
    {
        public void AdapteeRequest()
        {
            Debug.WriteLine("adaptee request");
        }
    }

    #endregion Adapter impl

    #region Facade Impl


    // SubSystemA 
    class SubSystemA
    {
        public void MethodA()
        {
        }
    }

    // SubSystemB
    class SubSystemB
    {
        public void MethodB()
        {
        }
    }

    // Facade
    class Facade
    {
        SubSystemA A;
        SubSystemB B;

        public Facade()
        {
            A = new SubSystemA();
            B = new SubSystemB();
        }

        public void Method1()
        {
        }

        public void Method2()
        {
        }
    }


    #endregion Facade Impl

    #region observer impl
    //Subject
    abstract class Subject
    {
        private ArrayList observers = new ArrayList();

        public void Attach(Observer observer)
        {
            observers.Add(observer);
        }

        public void Detach(Observer observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (Observer o in observers)
            {
                o.Update();
            }
        }
    }

    //ConcreteSubject
    class ConcreteSubject : Subject
    {
        private string subjectState;

        // Property
        public string SubjectState
        {
            get { return subjectState; }
            set { subjectState = value; }
        }
    }

    //Observer 
    abstract class Observer
    {
        public abstract void Update();
    }

    //ConcreteObserver
    class ConcreteObserver : Observer
    {
        private string name;
        private string observerState;
        private ConcreteSubject subject;

        // Constructor
        public ConcreteObserver(
            ConcreteSubject subject, string name)
        {
            this.subject = subject;
            this.name = name;
        }

        public override void Update()
        {
            observerState = subject.SubjectState;
            Console.WriteLine("Observer {0}'s new state is {1}",
                name, observerState);
        }

        // Property
        public ConcreteSubject Subject
        {
            get { return subject; }
            set { subject = value; }
        }
    }
    #endregion observer impl
}
