using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Tracer
{
    public class TracerClass : ITracer
    {
        private Stopwatch time = new Stopwatch();
        private TraceResult traceResult = new TraceResult();
        public TraceResult GetTraceResult()
        {
            traceResult.Threads = new List<Threads>();
            traceResult.Threads.Add(new Threads());
            traceResult.Threads[0].Time = time.ElapsedMilliseconds;

            return traceResult;
        }

        public void StartTrace()
        {
            time.Reset();
            time.Start();
        }

        public void StopTrace()
        {
            time.Stop();
        }
    }
}
