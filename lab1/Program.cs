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
            Bar _bar = new Bar(tracer);
            AnotherClass _anotherObject = new AnotherClass(tracer);

            _foo.MyMethod();
            _foo.MyMethod();

            Thread secondThread = new Thread(new ThreadStart(_anotherObject.AnotherMethod));
            secondThread.Start();

            Thread thirdThread = new Thread(new ThreadStart(_bar.InnerMethod));
            thirdThread.Start();

            Thread.Sleep(1000);

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
            Thread.Sleep(50);
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
            
            Thread.Sleep(50);
            _tracer.StopTrace();
        }
    }

    public class AnotherClass
    {
        private ITracer _tracer;
        private Bar _bar;
        private int n = 3;

        internal AnotherClass(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }

        public void AnotherMethod()
        {
            _tracer.StartTrace();
            while (n != 0)
            {
                n--;
                AnotherMethod();
                _bar.InnerMethod();
            }
            Thread.Sleep(50);
            _tracer.StartTrace();
        }
    }
}