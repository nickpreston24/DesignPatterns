using System;
using System.Collections.Generic;

namespace Decorators
{
    public static class Client
    {
        public static void Run()
        {
            // Conventional use:
            IProcess process = new Process();
            Console.WriteLine("Simplest process running: " + process.Start("Notepad++.exe"));

            var decorator1 = new RetryableProcess(process);
            var decorator2 = new CacheableProcess(decorator1);

            decorator2.Start("Kitties!");

            // Using a builder:
            IProcess compoundProcess = ProcessBuilder.Process
                .Cache()
                .Synchronize()
                .Build();

            compoundProcess.Start("Running many operations...");
        }
    }

    public class ProcessBuilder
    {
        public static ProcessBuilder Process => new ProcessBuilder();

        IProcess process = new Process();

        public ProcessBuilder Synchronize()
        {
            process = new RetryableProcess(process);
            return this;
        }

        public ProcessBuilder Cache()
        {
            process = new CacheableProcess(process);
            return this;
        }

        public IProcess Build()
        {
            IProcess result = process;

            process = new Process(); //Clears for reuse.

            return result;
        }
    }

    // Concrete Component
    public class Process : ProcessComponent
    {
        public override bool Start(string args) => throw new NotImplementedException();
        public override bool Kill() => throw new NotImplementedException();
        public override ProcessInfo GetInfo() => throw new NotImplementedException();
        public override ProcessStatus GetStatus() => throw new NotImplementedException();
        public override ProcessStats GetStatistics() => throw new NotImplementedException();
    }

    // Base Component
    public abstract class ProcessComponent : IProcess
    {
        public abstract bool Start(string args);
        public abstract bool Kill();
        public abstract ProcessInfo GetInfo();
        public abstract ProcessStatus GetStatus();
        public abstract ProcessStats GetStatistics();
    }

    // Interface
    public interface IProcess
    {
        bool Start(string args);
        bool Kill();
        ProcessInfo GetInfo();
        ProcessStatus GetStatus();
        ProcessStats GetStatistics();
    }

    // Abstract Decorator (https://dzone.com/articles/is-inheritance-dead)
    public abstract class ProcessDecorator : IProcess
    {
        protected IProcess process;
        protected ProcessDecorator(IProcess process) => this.process = process;

        #region Forwarding Methods

        public virtual bool Start(string args) => process.Start(args);
        public virtual bool Kill() => process.Kill();
        public virtual ProcessInfo GetInfo() => process.GetInfo();
        public virtual ProcessStatus GetStatus() => process.GetStatus();
        public virtual ProcessStats GetStatistics() => process.GetStatistics();

        #endregion Forwarding Methods
    }

    //Concrete Decorator
    public class RetryableProcess : ProcessDecorator
    {
        public RetryableProcess(IProcess process)
            : base(process)
        {
        }

        // Override only the Start method
        public override bool Start(string args)
        {
            Console.WriteLine("Retryable");
            return true;
        }

        public bool Retry(int maxTries)
        {
            // Func<string, bool> func = Start("Test");
            // var result = func.Retry(3);
            return true;
        }
    }

    //Concrete Decorator
    internal class CacheableProcess : ProcessDecorator
    {
        public CacheableProcess(IProcess process)
            : base(process)
        {
        }

        /* Override some methods */
        // public override ProcessInfo GetInfo() => throw new System.NotImplementedException();
        // public override ProcessStatus GetStatus() => throw new System.NotImplementedException();
        // public override ProcessStats GetStatistics() => throw new System.NotImplementedException();

        // Some other extra for this class only :)
        public IDictionary<ProcessInfo, string> Cache() => throw new NotImplementedException();
    }

    public class ProcessInfo
    {
    }

    public class ProcessStatus
    {
    }

    public class ProcessStats
    {
    }
}