using System;
using System.Threading;
using Tracer;

namespace lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            TracerClass tracer = new TracerClass();
            tracer.StartTrace();
            Console.WriteLine("Hello World!");
            Thread.Sleep(100);
            tracer.StopTrace();
            Console.WriteLine(tracer.GetTraceResult().Threads[0].Time);
        }
    }
}
