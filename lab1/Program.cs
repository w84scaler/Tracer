using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Tracer;

namespace lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            TracerClass tracer = new TracerClass();
            Foo _foo = new Foo(tracer);
            _foo.MyMethod();
            _foo.MyMethod();

            TraceResult traceResult = tracer.GetTraceResult();
        }
    }

    public class Foo
    {
        private Bar _bar;
        private ITracer _tracer;

        internal Foo(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }
        public void MyMethod()
        {
            _tracer.StartTrace();
            Console.WriteLine("Hello");
            Thread.Sleep(100);
            _bar.InnerMethod();
            _tracer.StopTrace();
        }
    }

    public class Bar
    {
        private ITracer _tracer;

        internal Bar(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void InnerMethod()
        {
            _tracer.StartTrace();
            Console.WriteLine("World!");
            Thread.Sleep(100);
            _tracer.StopTrace();
        }
    }
}
