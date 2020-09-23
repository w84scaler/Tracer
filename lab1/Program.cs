using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Tracer;
using lab1.Serialisation;

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

            _anotherObject.AnotherMethod();

            Thread secondThread = new Thread(new ThreadStart(_foo.MyMethod));
            secondThread.Start();

            Thread thirdThread = new Thread(new ThreadStart(_bar.InnerMethod));
            thirdThread.Start();

            secondThread.Join();
            thirdThread.Join();

            TraceResult traceResult = tracer.GetTraceResult();

            ISerializator serializatorJson = new JsonSerializator();
            Console.WriteLine(serializatorJson.Serialize(traceResult));

            ISerializator serializatorXml = new XmlSerializator();
            Console.WriteLine(serializatorXml.Serialize(traceResult));
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
            _tracer.StopTrace();
        }
    }
}