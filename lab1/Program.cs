using System;
using System.Threading;
using Tracer;
using lab1.Serialization;
using lab1.Writing;

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

            tracer.StartTrace();
            _anotherObject.AnotherMethod();
            _bar.InnerMethod();
            tracer.StopTrace();

            Thread secondThread = new Thread(new ThreadStart(_foo.MyMethod));
            secondThread.Start();

            Thread thirdThread = new Thread(new ThreadStart(_bar.InnerMethod));
            thirdThread.Start();

            secondThread.Join();
            thirdThread.Join();

            TraceResult traceResult = tracer.GetTraceResult();

            ISerializer serializerJson = new JsonSerializer();
            ISerializer serializerXml = new myXmlSerializer();
            IWriter consoleWriter = new ConsoleWriter();
            IWriter fileWriter = new FileWriter(Environment.CurrentDirectory + "\\" + "FileName" + "." + "txt");

            string json = serializerJson.Serialize(traceResult);
            string xml = serializerXml.Serialize(traceResult);

            consoleWriter.Write(json);
            consoleWriter.Write(xml);

            fileWriter.Write(json);
            //fileWriter.Write(xml);
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